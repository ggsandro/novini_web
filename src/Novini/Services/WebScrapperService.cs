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
            var html = await GetHtml(url);
            return ParseResult(html, htmlElem, htmlClass);
        }

        private async Task<HtmlDocument> GetHtml(string url)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;

            string urlToLoad = url;
            HttpWebRequest request = HttpWebRequest.Create(urlToLoad) as HttpWebRequest;
            request.Method = "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            WebResponse response = await request.GetResponseAsync();
            htmlDoc.Load(response.GetResponseStream(), true);
            return htmlDoc;
        }

        private List<NewsModel> ParseResult(HtmlDocument htmlDoc, string htmlElem, string htmlClass)
        {
            var resultList = new List<NewsModel>();
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
