using AppointmentAgent.Worker.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentAgent.Worker.Services
{
    public class OrsMonitorService
    {
        private readonly PlaywrightService _playwright;
        private readonly TelegramService _telegram;
        private readonly NotificationCacheService _cache;

        public OrsMonitorService(PlaywrightService playwright, TelegramService telegram, NotificationCacheService cache)
        {
            _playwright = playwright;
            _telegram = telegram;
            _cache = cache;
        }

        public async Task CheckSlotsAsync()
        {
            try
            {
                await OpenAppointmentPageAsync();

                await SelectStateAsync();

                await SelectHospitalAsync();

                await SelectAppointmentTypeAsync();

                await SelectCenterAsync();

                await SelectDepartmentAsync();

                var slots = await ReadCalendarAsync();

                if (slots.Any(x => x.Holiday))
                {
                    await NotifyAsync(slots);
                }
            }            
            finally
            {
                await _playwright.CloseBrowserAsync();
            }
        }

        private async Task OpenAppointmentPageAsync()
        {
            await _playwright.NavigateAsync(OrsLocators.NavigationUrl);

            await _playwright.WaitForVisibleAsync(OrsLocators.BookAppointmentButton);

            await _playwright.ClickAsync(OrsLocators.BookAppointmentButton);
        }

        private async Task SelectStateAsync()
        {
            await _playwright.ClickAsync(OrsLocators.State);

            await _playwright.WaitForVisibleAsync(OrsLocators.Hospital);
        }

        private async Task SelectHospitalAsync()
        {
            await _playwright.ClickAsync(OrsLocators.Hospital);
        }

        private async Task SelectAppointmentTypeAsync()
        {
            await _playwright.ClickAsync(OrsLocators.Appointment);

            await _playwright.ClickAsync(OrsLocators.NewAppointment);
        }

        private async Task SelectCenterAsync()
        {
            await _playwright.ClickAsync(OrsLocators.MainHospitalRakOpd);
        }

        private async Task SelectDepartmentAsync()
        {
            await _playwright.ClickAsync(OrsLocators.Orthopedics);

            if (await _playwright.ExistsAsync(OrsLocators.PopOkButton))
            {
                await _playwright.ClickAsync(OrsLocators.PopOkButton);

                await _playwright.ClickAsync(OrsLocators.Orthopedics);
            }

            await _playwright.WaitForVisibleAsync(OrsLocators.Calendar);
        }

        private async Task<List<SlotInfo>> ReadCalendarAsync()
        {
            var slots = new List<SlotInfo>();

            bool stopReading = false;

            var startDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

            while (!stopReading)
            {
                var currentMonth = await _playwright.GetTextAsync(OrsLocators.MonthTitle);

                await _playwright.WaitAsync(OrsLocators.Calendar);

                var days = await _playwright.GetElementsAsync(OrsLocators.CalendarDays);

                foreach (var day in days)
                {
                    var date = await day.GetAttributeAsync("data-date");

                    if (string.IsNullOrWhiteSpace(date))
                        continue;

                    if (!DateOnly.TryParse(date, out var slotDate))
                        continue;

                    // Ignore today's and previous dates
                    if (slotDate < startDate)
                        continue;

                    var slot = new SlotInfo
                    {
                        Date = date
                    };

                    var eventLocator = day.Locator(OrsLocators.DayEvent);

                    if (await eventLocator.CountAsync() == 0)
                        continue;

                    slot.BackgroundColor = await eventLocator.EvaluateAsync<string>(
                        "e => getComputedStyle(e).backgroundColor");

                    var titleLocator = eventLocator.Locator(OrsLocators.EventTitle);

                    if (await titleLocator.CountAsync() > 0)
                        slot.Status = (await titleLocator.InnerTextAsync()).Trim();

                    switch (slot.BackgroundColor)
                    {
                        // GREEN
                        case "rgb(60, 179, 113)":
                            slot.Available = true;
                            break;

                        // ORANGE
                        case "rgb(233, 150, 122)":
                            slot.Full = true;
                            break;

                        // GREY
                        case "rgb(211, 211, 211)":
                            slot.Holiday = true;
                            break;

                        // WHITE
                        case "rgb(255, 255, 255)":

                            slot.NotOpened = true;

                            stopReading = true;

                            break;
                    }

                    slots.Add(slot);

                    if (stopReading)
                        break;
                }

                if (stopReading)
                    break;
                
                await _playwright.ClickAsync(OrsLocators.NextButton);

                await _playwright.WaitUntilTextChangesAsync(OrsLocators.MonthTitle, currentMonth);

            }

            return slots;
        }

        private async Task NotifyAsync(List<SlotInfo> slots)
        {
            var availableSlots = slots.Where(x => x.Holiday).ToList();            

            var newSlots = availableSlots.Where(x => !_cache.IsNotified($"AIIMS-NewDelhi|Orthopedics|{x.Date}")).ToList();

            if (!newSlots.Any())
                return;            

            var availableDates = string.Join(Environment.NewLine, newSlots.Select(x => $"• {x.Date:dd-MMM-yyyy}"));            

            var message = $"""
                🚨 AIIMS Appointment Alert 🚨
                
                🎉 Good News! Appointment slots are now available.
                
                🏥 Hospital: AIIMS New Delhi
                🩺 Department: Orthopedics
                
                📅 Available Dates:
                {availableDates}
                
                ⏰ Detected At: {DateTime.Now:dd-MMM-yyyy hh:mm tt}
                
                ⚡ Open ORS and book your appointment as soon as possible. Slots can fill up very quickly.

                {OrsLocators.NavigationUrl}
                
                🤖 Powered by AIIMS Appointment Agent
                """;

            await _telegram.SendMessageAsync(message);
            
            foreach (var slot in newSlots)
            {
                var key = $"AIIMS-NewDelhi|Orthopedics|{slot.Date}";
                _cache.Add(key);
            }
        }
    }
}
