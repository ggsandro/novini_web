using Microsoft.AspNetCore.Mvc;
using Novini.Models;
using Novini.Repository;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Novini.Services;
using System;

namespace Novini.Controllers
{
    public class HomeController : Controller
    {
        //[ResponseCache(Duration = 300)]
        public IActionResult Index()
        {
            var repository = new NewsRepository();
            var model = HttpContext.User.Identity.IsAuthenticated ?
                repository.TakeNews(0, AppSettings.AppSettings.NewsOnPage).ToList() :
                repository.TakeApprovedNews(0, AppSettings.AppSettings.NewsOnPage).ToList();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpdateNews(List<NewsModel> newsList)
        {
            var repository = new NewsRepository();
            repository.UpdateNews(newsList);
            return RedirectToAction("Index");
        }

        public IActionResult AddNewsItem(NewsModel newsModel)
        {
            var repository = new NewsRepository();
            repository.AddNewsItem(newsModel);
            return Ok();
        }

        public string MoreNews(int currentElements)
        {
            var repository = new NewsRepository();
            var model = HttpContext.User.Identity.IsAuthenticated ?
                repository.TakeNews(currentElements, AppSettings.AppSettings.NewsOnPage).ToList() :
                repository.TakeApprovedNews(currentElements, AppSettings.AppSettings.NewsOnPage).ToList();
            return JsonConvert.SerializeObject(model);
        }

        public async void SearchNews(NewsModel newsModel)
        {
            var newsList = new List<NewsModel>();
            var repository = new NewsRepository();
            var scrapperRepository = new ScrapperRepository();
            var service = new WebScrapperService();
            var templates = new List<ScrapperTemplateModel>();            //scrapperRepository.GetAll();

            templates.Add(new ScrapperTemplateModel { Url = "http://ua.korrespondent.net/", TitleHtmlSelector = "div.article__title a", LinkHtmlSelector = "div.article__title a" });
            templates.Add(new ScrapperTemplateModel { Url = "http://tsn.ua/", TitleHtmlSelector = "div.g_item a.link span.title", LinkHtmlSelector = "div.g_item a.link" });
            // ?? templates.Add(new ScrapperTemplateModel { Url = "http://www.pravda.com.ua/news/", TitleHtmlSelector = "div.article__title a", LinkHtmlSelector = "div.article__title a" });
            templates.Add(new ScrapperTemplateModel { Url = "http://www.unian.ua/", TitleHtmlSelector = "div.label a", LinkHtmlSelector = "div.label a" });
            templates.Add(new ScrapperTemplateModel { Url = "http://gazeta.ua/", TitleHtmlSelector = "section.item a", LinkHtmlSelector = "section.item a" });
            templates.Add(new ScrapperTemplateModel { Url = "http://ukr.obozrevatel.com/", TitleHtmlSelector = "a.ttl", LinkHtmlSelector = "a.ttl" });
            templates.Add(new ScrapperTemplateModel { Url = "http://zik.ua/news/all", TitleHtmlSelector = "ul.news-list div.news-title", LinkHtmlSelector = "ul.news-list a" });

            foreach (var template in templates)
            {
                try
                {
                    var news = await service.GetNewsFromUrlAsync(template);
                    newsList.AddRange(news);
                } catch(Exception ex)
                {
                    //Send Email
                }
            }

            //Delete items that already in database
            var dbList = repository.TakeNews(0, int.MaxValue).Select(x => x.Url).ToList();
            newsList = newsList.Where(x => !dbList.Contains(x.Url)).ToList();

            repository.AddNewsRange(newsList);
        }
    }
}
