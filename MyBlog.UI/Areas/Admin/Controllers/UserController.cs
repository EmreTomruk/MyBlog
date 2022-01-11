using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
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
        private readonly IWebHostEnvironment _env;
        private readonly IMapper _mapper;

        public UserController(UserManager<User> userManager, IWebHostEnvironment env, IMapper mapper)
        {
            _userManager = userManager;
            _env = env;
            _mapper = mapper;
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

        public IActionResult Add()
        {
            return PartialView("_UserAddPartial");
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            if (ModelState.IsValid)
            {
                userAddDto.Picture = await ImageUpload(userAddDto);
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
                        ModelState.AddModelError("", error.Description);
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

        public async Task<JsonResult> Delete(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                var deletedUser = JsonSerializer.Serialize(new UserDto
                {
                    User = user,
                    ResultStatus = ResultStatus.Success,
                    Message = $"{user.UserName} adlı kullanıcı başarıyla silinmiştir."                
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
                    User = user,
                    ResultStatus = ResultStatus.Error,
                    Message = $"{user.UserName} adlı kullanıcı silinememiştir.\n{errorMessages}"
                    
                });

                return Json(deleteduserErrorModel);
            }     
        }

        public async Task<PartialViewResult> Update(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId); //FindByIdAsync() metodu da kullanilabilirdi...
            var userUpdateDto = _mapper.Map<UserUpdateDto>(user);

            return PartialView("_UserUpdatePartial", userUpdateDto);
        }


        //public async Task<PartialViewResult> Update(int userId)
        //{
        //    var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId); //FindByIdAsync() metodu da kullanilabilirdi...
        //    var userUpdateDto = _mapper.Map<UserUpdateDto>(user);

        //    return PartialView("_UserUpdatePartial", userUpdateDto);
        //}

        public async Task<string> ImageUpload(UserAddDto userAddDto)
        {
            // ~/img/user.Picture
            string wwwroot = _env.WebRootPath;
            // EmreTomruk     
            // string fileName2 = Path.GetFileNameWithoutExtension(userAddDto.PictureFile.FileName);
            //.png
            string fileExtension = Path.GetExtension(userAddDto.PictureFile.FileName);
            DateTime dateTime = DateTime.Now;
            // EmreTomruk_587_5_38_12_3_10_2020.png
            string fileName = $"{userAddDto.UserName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
            var path = Path.Combine($"{wwwroot}/img", fileName);

            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await userAddDto.PictureFile.CopyToAsync(stream); //Islem sonunda resmimiz "~/img" klasorune aktarilmis oluyor...
            }

            return fileName; // EmreTomruk_587_5_38_12_3_10_2020.png -> "~/img/user.Picture"
        }
    }
}
