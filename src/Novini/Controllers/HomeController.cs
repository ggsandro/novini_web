using Microsoft.AspNetCore.Mvc;
using Novini.Models;
using Novini.Repository;

namespace Novini.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var repository = new NewsRepository();
            var model = repository.TakeApprovedNews();
            return View();
        }

        public void AddNewsItem(NewsModel newsModel)
        {
            var repository = new NewsRepository();
            repository.AddNewsItem(newsModel);
        }
    }
}
