using Microsoft.AspNetCore.Mvc;
using Novini.Models;
using Novini.Repository;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace Novini.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var repository = new NewsRepository();
            var model = repository.TakeApprovedNews(0, AppSettings.AppSettings.NewsOnPage).ToList();
            return View(model);
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
            var model = repository.TakeApprovedNews(currentElements, AppSettings.AppSettings.NewsOnPage);
            return JsonConvert.SerializeObject(model);
        }
    }
}
