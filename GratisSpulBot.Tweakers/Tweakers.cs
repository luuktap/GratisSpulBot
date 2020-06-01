using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GratisSpulBot.TweakersDecemberPrijzenParade
{
    class Tweakers
    {
        private static CookieContainer cookieContainer;
        private static HttpClientHandler clienthandler;
        private HttpClient client;

        public Tweakers()
        {
            cookieContainer = new CookieContainer();
            clienthandler = new HttpClientHandler { AllowAutoRedirect = true, UseCookies = true, CookieContainer = cookieContainer };
            client = new HttpClient(clienthandler);
        }

        private async Task<FormUrlEncodedContent> GetCookieWallPayload()
        {
            var response = await client.GetAsync("https://tweakers.net/");
            var pageContents = await response.Content.ReadAsStringAsync();
            Console.WriteLine(pageContents);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(pageContents);

            var returnToInput = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='cookieAcceptForm']/input[1]").Attributes[2].Value;
            var fragment = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='cookieAcceptForm']/input[2]").Attributes[2].Value;
            var tweakersToken = htmlDocument.DocumentNode.SelectSingleNode("//*[@id='cookieAcceptForm']/input[3]").Attributes[2].Value;

            var values = new Dictionary<string, string>
            {
                { "decision", "accept" },
                { "returnTo", returnToInput },
                { "fragment", fragment },
                { "tweakers_token", tweakersToken }
            };

            var content = new FormUrlEncodedContent(values);
            return content;
        }

        public async Task<List<string>> GetTodaysLinks()
        {
            //sleep 5 sec so we don't trigger the DDOS protection
            Thread.Sleep(5000);

            var content = await GetCookieWallPayload();

            //sleep 5 sec so we don't trigger the DDOS protection
            Thread.Sleep(5000);

            var postResponse = await client.PostAsync("https://tweakers.net/my.tnet/cookies/", content);
            var postPageContents = await postResponse.Content.ReadAsStringAsync();
            //Console.WriteLine(postPageContents);

            HtmlDocument postHtmlDocument = new HtmlDocument();
            postHtmlDocument.LoadHtml(postPageContents);

            var today = DateTime.Now.ToString("yyyy-MM-dd");

            var elements = postHtmlDocument.DocumentNode.SelectNodes($".//*[@id='news']/div[@data-date='{today}']/table/tr[@class='headline promo']/td[3]/a");

            List<string> urls = new List<string>();

            if (elements == null || elements.Count <= 0)
            {
                return urls;
            }

            foreach (var element in elements)
            {
                if (element.InnerText.IndexOf("December Prijzen Parade") != -1)
                {
                    //Console.WriteLine(element.Attributes[0].Value);
                    urls.Add(element.Attributes[0].Value);
                }
            }

            return urls;
        }

    }
}
