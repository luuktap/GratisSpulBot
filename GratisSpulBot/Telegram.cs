using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace GratisSpulBot
{
    class Telegram
    {
        public TelegramBotClient telegramBotClient;
        public Telegram(string token)
        {
            telegramBotClient = new TelegramBotClient(token);
            WelcomeMessage();
        }

        public async void WelcomeMessage()
        {
            var me = await telegramBotClient.GetMeAsync();
            Console.WriteLine($"Hello, World! I am user {me.Id} and my name is {me.FirstName}.");
        }

        public async void SendMessage(string chatId, string message)
        {
            await telegramBotClient.SendTextMessageAsync(chatId, text: message);
        }
    }
}
