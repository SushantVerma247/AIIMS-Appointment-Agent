using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentAgent.Worker.Models
{
    public class SlotInfo
    {
        public string Date { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;

        public bool Available { get; set; }
        public bool Full { get; set; }
        public bool Holiday { get; set; }
        public bool NotOpened { get; set; }
    }
}
