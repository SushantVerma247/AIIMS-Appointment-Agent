using AppointmentAgent.Worker.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace AppointmentAgent.Worker.Services
{
    public class TelegramService
    {
        private readonly TelegramBotClient _bot;
        private readonly long _chatId;

        public TelegramService(IOptions<AppSettings> options)
        {
            _bot = new TelegramBotClient(options.Value.Telegram.BotToken);
            _chatId = long.Parse(options.Value.Telegram.ChatId);
        }

        public async Task SendMessageAsync(string message)
        {
            await _bot.SendMessage(
                chatId: _chatId,
                text: message);
        }
    }
}
