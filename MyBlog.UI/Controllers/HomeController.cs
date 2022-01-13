using Microsoft.AspNetCore.Mvc;

namespace MyBlog.UI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
