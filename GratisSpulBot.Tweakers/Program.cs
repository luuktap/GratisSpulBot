using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GratisSpulBot.TweakersDecemberPrijzenParade
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            try
            {
                var telegram = new Telegram();
                var tweakers = new Tweakers();
                var tweakersDecemberLinks = await tweakers.GetTodaysLinks();
                if (tweakersDecemberLinks.Count == 0)
                {
                    Console.WriteLine("No links found for today!");
                    telegram.SendMessage("Found no Tweakers December Prijzen Parade link for today!");
                }
                else if (tweakersDecemberLinks.Count == 1)
                {
                    Console.WriteLine("Found a link for today!");
                    telegram.SendMessage($"Found a Tweakers December Prijzen Parade link for today!");
                    Thread.Sleep(500);
                    telegram.SendMessage(tweakersDecemberLinks[0]);

                }
                else if (tweakersDecemberLinks.Count > 1)
                {
                    Console.WriteLine("Found multiple links!");
                    telegram.SendMessage($"Found {tweakersDecemberLinks.Count} Tweakers December Prijzen Parade links for today!");
                    Thread.Sleep(500);
                    foreach (var link in tweakersDecemberLinks)
                    {
                        Console.WriteLine(link);
                        telegram.SendMessage(link);
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
