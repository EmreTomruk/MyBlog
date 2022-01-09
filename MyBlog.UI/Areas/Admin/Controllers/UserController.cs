using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using System.Threading.Tasks;

namespace MyBlog.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync(); //Bu islemle tum kullanicilar gelir. Bunu UserListDto icine attiktan sonra View'e gonderecegiz...

            return View(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            });
        }
    }
}
