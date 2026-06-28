using AppointmentAgent.Worker.Models;
using AppointmentAgent.Worker.Services;
using Microsoft.Playwright;

namespace AppointmentAgent.Worker
{
    public class Worker : BackgroundService
    {

        private readonly ILogger<Worker> _logger;
        private readonly OrsMonitorService _monitorService;

        public Worker(ILogger<Worker> logger, TelegramService telegram, OrsMonitorService monitorService)
        {
            _logger = logger;
            _monitorService = monitorService;            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Appointment Agent Started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _monitorService.CheckSlotsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while checking ORS slots");
                }

                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }

        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    _logger.LogInformation("Appointment Agent Started");

        //    while (!stoppingToken.IsCancellationRequested)
        //    {
        //        try
        //        {
        //            await _monitorService.CheckSlotsAsync();
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Error while checking ORS slots");
        //        }

        //        await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
        //    }

        //    await _playwrightService.NavigateAsync(OrsLocators.NavigationUrl);
                  
        //    await _playwrightService.ClickAsync(OrsLocators.BookAppointmentButton);
                  
        //    await _playwrightService.SelectOptionAsync("#ddlHospital", "AIIMS");
                  
        //    await _playwrightService.ClickAsync("#department");

        //    await _telegram.SendMessageAsync("ORS Website Opened Successfully ✅");
        //    await Task.Delay(Timeout.Infinite, stoppingToken);                       
        //}
    }
}
