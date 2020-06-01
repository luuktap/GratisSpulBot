using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace GratisSpulBot.Tweakers
{
    class Program
    {

        private static IConfiguration _configuration;

        public Program(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args);
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            try
            {
                var someKey = _configuration["telegram_group_id"];

                var tweakersDecember = new TweakersDecember();
                var tweakersDecemberLinks = await tweakersDecember.GetTodaysLinks();
                var telegramToken = "801203183:AAG1YqRtYb-0tfqU8amM5IadYIux0uXSFes";
                var telegramGroupId = "-1001156213500";
                var telegram = new Telegram(telegramToken);
                if (tweakersDecemberLinks.Count == 0)
                {
                    Console.WriteLine("No links found for today!");
                    telegram.SendMessage(telegramGroupId, "Found no Tweakers December Prijzen Parade link for today!");
                }
                else if (tweakersDecemberLinks.Count == 1)
                {
                    Console.WriteLine("Found a link for today!");
                    telegram.SendMessage(telegramGroupId, $"Found a Tweakers December Prijzen Parade link for today!");
                    Thread.Sleep(500);
                    telegram.SendMessage(telegramGroupId, tweakersDecemberLinks[0]);

                }
                else if (tweakersDecemberLinks.Count > 1)
                {
                    Console.WriteLine("Found multiple links!");
                    telegram.SendMessage(telegramGroupId, $"Found {tweakersDecemberLinks.Count} Tweakers December Prijzen Parade links for today!");
                    Thread.Sleep(500);
                    foreach (var link in tweakersDecemberLinks)
                    {
                        Console.WriteLine(link);
                        telegram.SendMessage(telegramGroupId, link);
                        Thread.Sleep(500);
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Failed to execute console job!");
                Console.WriteLine(e);
            }
        }
    }
}
