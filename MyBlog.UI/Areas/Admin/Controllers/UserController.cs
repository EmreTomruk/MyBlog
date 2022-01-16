using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos;
using MyBlog.Shared.Utilities.Extensions;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using MyBlog.UI.Areas.Admin.Models;

namespace MyBlog.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, IWebHostEnvironment env, IMapper mapper, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _env = env;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync(); //Bu islemle tum kullanicilar gelir. Bunu UserListDto icine attiktan sonra View'e gonderecegiz...
            
            return View(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            });
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View("UserLogin");
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(userLoginDto.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, userLoginDto.RememberMe, false); //userLoginDto.RememberMe(isPersistent): Flag indicating whether the sign-in cookie should persist after the browser is closed.

                    if (result.Succeeded) return RedirectToAction("Index", "Home");

                    else
                    {
                        ModelState.AddModelError(String.Empty, "E-posta veya şifreniz yanlıştır...");
                        return View("UserLogin");
                    }
                }
                else
                {
                    ModelState.AddModelError(String.Empty, "E-posta veya şifreniz yanlıştır...");
                    return View("UserLogin");
                }
            }
            else return View("UserLogin"); 
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home", new { Area=String.Empty});
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync(); 

            var userListDto = JsonSerializer.Serialize(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            }, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(userListDto);
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if (ModelState.IsValid)
            {
                userAddDto.Picture = await ImageUpload(userAddDto.UserName, userAddDto.PictureFile);
                var user = _mapper.Map<User>(userAddDto); //Bize User donecek...
                var result = await _userManager.CreateAsync(user, userAddDto.Password); //Sifreyi hashleyip veritabanina kaydeder...

                if (result.Succeeded) //Kullanici basariyla eklendi mi? - Burada ModelState kontrolunun yanisira Identity tarafinda 2. bir kontrol yapilir(result -> IdentityResult'tir)...
                {
                    var userAddAjaxViewModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{user.UserName} adlı kullanıcı başarıyla eklenmiştir.",
                            User = user
                        },
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjaxViewModel);
                }

                else //Identity tarafinda bir hata alinirsa bunu Front-End'de donebilmek icin kullanacagiz...
                {
                    foreach (var error in result.Errors) //Donen hatalari ekliyoruz...
                    {
                        ModelState.AddModelError(String.Empty, error.Description);
                    }

                    var userAddAjaxErrorViewModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserAddDto = userAddDto, //Hatalarin dpnmesi icin gerekli...
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjaxErrorViewModel);
                }
            }

            var userAddAjaxModelStateErrorViewModel = JsonSerializer.Serialize(new UserAddAjaxViewModel //!ModelState.IsValid ise
            {
                UserAddDto = userAddDto,
                UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
            });
            return Json(userAddAjaxModelStateErrorViewModel);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<JsonResult> Delete(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                var deletedUser = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus = ResultStatus.Success,
                    Message = $"{user.UserName} adlı kullanıcı başarıyla silinmiştir.",
                    User = user
                });
                return Json(deletedUser);
            }

            else
            {
                string errorMessages = String.Empty;

                foreach (var error in result.Errors)
                {
                    errorMessages = $"*{error.Description}\n";
                }
            
                var deleteduserErrorModel = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{user.UserName} adlı kullanıcı silinememiştir.\n{errorMessages}",
                    User = user
                });
                return Json(deleteduserErrorModel);
            }     
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<PartialViewResult> Update(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId); //FindByIdAsync() metodu da kullanilabilirdi...
            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);

            return PartialView("_UserUpdatePartial", userUpdateDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUpload = false;
                var oldUser = await _userManager.FindByIdAsync(userUpdateDto.Id.ToString());
                var oldUserPicture = oldUser.Picture;

                if (userUpdateDto.PictureFile != null)
                {
                    userUpdateDto.Picture = await ImageUpload(userUpdateDto.UserName, userUpdateDto.PictureFile);
                    isNewPictureUpload = true;
                }
                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);

                if (result.Succeeded) //Islem basariyla gerceklesip veritabanina gonderildiyse
                {
                    if (isNewPictureUpload)
                    {
                        ImageDelete(oldUserPicture);
                    }
                    var userUpdateViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir.",
                            User = updatedUser
                        },
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    });
                    return Json(userUpdateViewModel);
                }

                else //Identity hatalari
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var userUpdateErrorViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserUpdateDto = userUpdateDto,
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    });
                    return Json(userUpdateErrorViewModel);
                }
            }

            else //Data Annotation hatalari
            {
                var userUpdateModelStateErrorViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                {
                    UserUpdateDto = userUpdateDto,
                    UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                });
                return Json(userUpdateModelStateErrorViewModel);
            }
        }

        [Authorize(Roles ="Admin, Editor")]
        public async Task<string> ImageUpload(string userName, IFormFile pictureFile)
        {
            // ~/img/user.Picture
            string wwwroot = _env.WebRootPath;
            // EmreTomruk     
            // string fileName2 = Path.GetFileNameWithoutExtension(pictureFile.FileName);
            //.png
            string fileExtension = Path.GetExtension(pictureFile.FileName);
            DateTime dateTime = DateTime.Now;
            // EmreTomruk_587_5_38_12_3_10_2020.png
            string fileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
            var path = Path.Combine($"{wwwroot}/img", fileName);

            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream); //Islem sonunda resmimiz "~/img" klasorune aktarilmis oluyor...
            }
            return fileName; // EmreTomruk_587_5_38_12_3_10_2020.png -> "~/img/user.Picture"
        }

        [Authorize(Roles ="Admin, Editor")]
        public bool ImageDelete(string pictureName)
        {
            string wwwroot = _env.WebRootPath;
            var fileToDelete = Path.Combine($"{wwwroot}/img", pictureName);

            if (System.IO.File.Exists(fileToDelete))
            {
                System.IO.File.Delete(fileToDelete);

                return true;
            }
            else
                return false;
        }

        [HttpGet]
        public ViewResult AccessDenied()
        {
            return View();
        }
    }
}
