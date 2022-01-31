using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MyBlog.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using MyBlog.Entities.Concrete;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using MyBlog.UI.Areas.Admin.Models;

namespace MyBlog.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin, Editor")] //Kullanici burada bir islem yapabilmek icin "Login" olmak zorunda!

    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public HomeController(ICategoryService categoryService, IArticleService articleService,
            ICommentService commentService, UserManager<User> userManager)
        {
            _categoryService = categoryService;
            _articleService = articleService;
            _commentService = commentService;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var categoriesCountResult = await _categoryService.CountByIsDeleted();
            var articlesCountResult = await _articleService.CountByIsDeleted();
            var commentsCountResult = await _commentService.CountByIsDeleted();
            var usersCount = await _userManager.Users.CountAsync();
            var articlesResult = await _articleService.GetAll();

            if (categoriesCountResult.ResultStatus == ResultStatus.Success &&
                articlesCountResult.ResultStatus == ResultStatus.Success &&
                commentsCountResult.ResultStatus == ResultStatus.Success &&
                usersCount > -1 &&
                articlesCountResult.ResultStatus == ResultStatus.Success)
            {
                return View(new DashboardViewModel
                {
                    CategoriesCount = categoriesCountResult.Data,
                    ArticlesCount = articlesCountResult.Data,
                    CommentsCount = commentsCountResult.Data,
                    UsersCount = usersCount,
                    Articles = articlesResult.Data
                });
            }
            return NotFound();
        }
    }
}
