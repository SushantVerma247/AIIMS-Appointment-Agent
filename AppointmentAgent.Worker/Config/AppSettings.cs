using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentAgent.Worker.Config
{
    public class AppSettings
    {
        public TelegramSettings Telegram { get; set; } = new();
    }

    public class TelegramSettings
    {
        public string BotToken { get; set; } = string.Empty;
        public string ChatId { get; set; } = string.Empty;
    }
}
