using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace GratisSpulBot.TweakersDecemberPrijzenParade
{
    class Telegram
    {
        private readonly IConfiguration _config;
        public TelegramBotClient telegramBotClient;

        public Telegram()
        {
            //_config = config;
            telegramBotClient = new TelegramBotClient(_config["telegram_token"]);
        }

        public async void SendMessage(string message)
        {
            await telegramBotClient.SendTextMessageAsync(_config["telegram_group_id"], text: message);
        }
    }
}
