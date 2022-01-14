﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MyBlog.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin, Editor")]

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
