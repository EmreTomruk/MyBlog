using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyBlog.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Editor")] //Kullanici burada bir islem yapabilmek icin "Login" olmak zorunda!

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
