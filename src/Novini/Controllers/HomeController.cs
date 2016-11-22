using Microsoft.AspNetCore.Mvc;
using Novini.Models;
using Novini.Repository;
using System.Linq;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Novini.Services;

namespace Novini.Controllers
{
    public class HomeController : Controller
    {
        [ResponseCache(Duration = 300)]
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
            var templates = scrapperRepository.GetAll();
            foreach(var template in templates)
            {
                newsList.AddRange( await service.GetNewsFromUrlAsync(template.Url, template.HtmlElement, template.Class) );
            }
            repository.AddNewsRange(newsList);
        }
    }
}
