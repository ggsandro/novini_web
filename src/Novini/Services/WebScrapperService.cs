using HtmlAgilityPack;
using Novini.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Novini.Services
{
    public class WebScrapperService
    {
        public async Task<List<NewsModel>> GetNewsFromUrlAsync(string url, string htmlElem, string htmlClass)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;

            string urlToLoad = url;
            HttpWebRequest request = HttpWebRequest.Create(urlToLoad) as HttpWebRequest;
            request.Method = "GET";

            /* Sart browser signature */
            //request.UserAgent = "Mozilla/5.0 (Windows NT 6.3; WOW64; rv:31.0) Gecko/20100101 Firefox/31.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en-us,en;q=0.5");
            /* Sart browser signature */

            Console.WriteLine(request.RequestUri.AbsoluteUri);
            WebResponse response = await request.GetResponseAsync();

            var resultList = new List<NewsModel>();

            htmlDoc.Load(response.GetResponseStream(), true);
            if (htmlDoc.DocumentNode != null)
            {
                var titleDivNode = htmlDoc.DocumentNode.Descendants(htmlElem)
                    .Where(x => x.Attributes["class"] != null && x.Attributes["class"].Value == htmlClass).ToList();

                foreach (var elem in titleDivNode)
                {
                    var title = WebUtility.HtmlDecode(elem.FirstChild.InnerText.Trim());
                    var elemUrl = elem.FirstChild.Attributes["href"].Value;
                    var item = new NewsModel
                    {
                        Title = title,
                        Url = elemUrl,
                        Content = title
                    };
                    resultList.Add(item);
                }
            }
            return resultList;
        }



    }
}
