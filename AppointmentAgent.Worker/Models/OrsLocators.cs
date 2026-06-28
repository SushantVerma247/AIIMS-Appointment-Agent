using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentAgent.Worker.Models
{
    public static class OrsLocators
    {
        public const string NavigationUrl = "https://ors.gov.in";

        public const string BookAppointmentButton = "button:has-text('Book Appointment')";

        public const string State = "div.list-li >> text=Delhi";

        public const string Hospital = "text=AIIMS, New Delhi";        

        public const string Appointment = "#survey-answers >> nth=0 >> li";        

        public const string NewAppointment = "text=New Appointment";

        public const string MainHospitalRakOpd = "text=Main Hospital, Rajkumari Amrit Kaur (RAK) Out Patient Department";        

        public const string Orthopedics = "text=Orthopedics";

        public const string PopOkButton = "button:has-text('Ok')";

        // Calendar
        public const string Calendar = ".fc-daygrid";

        // All day cells
        public const string CalendarDays = "td[data-date]";

        // Single day cell
        public const string SingleDayCell = "data-date";

        // Day Number
        public const string DayNumber = ".fc-daygrid-day-number";

        // Background Event
        public const string DayEvent = ".fc-bg-event";

        // Event Title (NA, Holiday etc.)
        public const string EventTitle = ".fc-event-title";

        // Next Month
        public const string NextButton = ".fc-next-button";

        // Previous Month
        public const string PreviousButton = ".fc-prev-button";

        // Month Header
        public const string MonthTitle = ".fc-toolbar-title";
    }
}
