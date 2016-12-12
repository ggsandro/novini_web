
using Novini.Models;
using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;
using System.IO;

namespace Novini.Services
{
    public class WebScrapperService
    {
        public async Task<List<NewsModel>> GetNewsFromUrlAsync(ScrapperTemplateModel template)
        {
            var html = await GetHtml(template.Url);
            return ParseResult(html, template);
        }

        private async Task<HtmlDocument> GetHtml(string url)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.OptionFixNestedTags = true;

            string urlToLoad = url;
            HttpWebRequest request = HttpWebRequest.Create(urlToLoad) as HttpWebRequest;
            request.ContinueTimeout = 5000;
            request.Method = "GET";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";

            WebResponse response = await request.GetResponseAsync();
            htmlDoc.Load(response.GetResponseStream(), true);
            return htmlDoc;
        }

        private List<NewsModel> ParseResult(HtmlDocument htmlDoc, ScrapperTemplateModel template)
        {
            var resultList = new List<NewsModel>();
            if (htmlDoc.DocumentNode != null)
            {
                var titleNodes = htmlDoc.QuerySelectorAll(template.TitleHtmlSelector);
                var urlNodes = htmlDoc.QuerySelectorAll(template.LinkHtmlSelector);

                if (titleNodes.Count == urlNodes.Count) {
                    for (var i = 0; i < titleNodes.Count; i++)
                    {
                        var title = WebUtility.HtmlDecode(titleNodes[i].InnerText.Trim());
                        var elemUrl = urlNodes[i].Attributes["href"].Value;
                        if(!elemUrl.Contains("http"))
                        {
                            Uri result = null;
                            if (Uri.TryCreate(new Uri(template.Url), elemUrl, out result))
                            {
                                elemUrl = result.AbsoluteUri;
                            }
                        }
                        var item = new NewsModel
                        {
                            Title = title,
                            Url = elemUrl,
                            Content = title
                        };
                        resultList.Add(item);
                    }
                }
            }
            return resultList;
        }

    }
}
