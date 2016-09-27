using Microsoft.AspNetCore.Mvc;
using Novini.Models;
using Novini.Repository;
using System.Linq;

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

        public string AddNewsItem(NewsModel newsModel)
        {
            var repository = new NewsRepository();
            repository.AddNewsItem(newsModel);
            return Json(new { Status = "ok" });
        }

        public string MoreNews(int currentPageSize)
        {
            var repository = new NewsRepository();
            var model = repository.TakeApprovedNews(currentPageSize, AppSettings.AppSettings.NewsOnPage);
            return PartialView(model).ToString();
        }
    }
}
