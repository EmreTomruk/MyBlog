using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using ImageProcessor;
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
                else
                {
                    foreach (var error in result.Errors) //Identity tarafinda bir hata alirsak...
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var userAddAjaxErrorViewModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserAddDto = userAddDto,
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
