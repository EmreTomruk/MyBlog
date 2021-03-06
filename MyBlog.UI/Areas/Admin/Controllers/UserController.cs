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
using MyBlog.UI.Helpers.Abstract;
using MyBlog.UI.Helpers.Concrete;

namespace MyBlog.UI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IImageHelper _imageHelper;

        public UserController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IImageHelper imageHelper)
        {
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _imageHelper = imageHelper;
        }

        [Authorize(Roles = "Admin")]
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

                    if (result.Succeeded) 
                        return RedirectToAction("Index", "Home");

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
            else
                return View("UserLogin"); 
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
                var uploadedImageDtoResult = await _imageHelper.UploadUserImage(userAddDto.UserName, userAddDto.PictureFile);
                userAddDto.Picture = uploadedImageDtoResult.ResultStatus == 
                    ResultStatus.Success ? uploadedImageDtoResult.Data.FullName : "userImages/defaultUser.png";

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
                    errorMessages = $"*{error.Description}\n";
            
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
                    var uploadedImageDtoResult = await _imageHelper.UploadUserImage(userUpdateDto.UserName, userUpdateDto.PictureFile);
                    userUpdateDto.Picture = uploadedImageDtoResult.ResultStatus == ResultStatus.Success ? uploadedImageDtoResult.Data.FullName : "userImages/defaultUser.png"; //Success olmazsa Default resmi yukle!

                    if (oldUserPicture != "userImages/defaultUser.png") //Eski resmin silinmesini(bu resmi kullananlar olabilir) onledik! 
                        isNewPictureUpload = true;
                }
                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);

                if (result.Succeeded) //Islem basariyla gerceklesip veritabanina gonderildiyse
                {
                    if (isNewPictureUpload)
                        _imageHelper.Delete(oldUserPicture);

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
                        ModelState.AddModelError(String.Empty, error.Description);

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

        [Authorize]
        public async Task<ViewResult> ChangeDetails() //JS islemi(modal form acilmayacak) kullanilmayacak, MVC mimarisinin kendisi kullanilacak...
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var updateDto = _mapper.Map<UserUpdateDto>(user);

            return View(updateDto);           
        }

        [Authorize]
        [HttpPost]
        public async Task<ViewResult> ChangeDetails(UserUpdateDto userUpdateDto) 
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUpload = false;
                var oldUser = await _userManager.GetUserAsync(HttpContext.User);
                var oldUserPicture = oldUser.Picture;

                if (userUpdateDto.PictureFile != null)
                {
                    var uploadedImageDtoResult = await _imageHelper.UploadUserImage(userUpdateDto.UserName, userUpdateDto.PictureFile);
                    userUpdateDto.Picture = uploadedImageDtoResult.ResultStatus == 
                        ResultStatus.Success ? uploadedImageDtoResult.Data.FullName : "userImages/defaultUser.png";

                    if (oldUserPicture != "userImages/defaultUser.png") 
                        isNewPictureUpload = true; //Eski resmin silinmesini(bu resmi kullananlar olabilir) onledik!
                }
                var updatedUser = _mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
                var result = await _userManager.UpdateAsync(updatedUser);

                if (result.Succeeded) //Islem basariyla gerceklesip veritabanina gonderildiyse
                {
                    if (isNewPictureUpload)
                        _imageHelper.Delete(oldUserPicture);
                    TempData.Add("SuccessMessage", $"{updatedUser.UserName} adlı kullanıcı başarıyla güncellenmiştir.");

                    return View(userUpdateDto);
                }
                else 
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(String.Empty, error.Description);

                    return View(userUpdateDto); //Hata alinirsa validation-summary kisminda gozukecektir...
                }
            }
            else return View(userUpdateDto);
        }

        [Authorize]
        public  ViewResult PasswordChange()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<ViewResult> PasswordChange(UserPasswordChangeDto userPasswordChangeDto)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var isVerified = await _userManager.CheckPasswordAsync(user, userPasswordChangeDto.CurrentPassword);

                if (isVerified)
                {
                    var result = await _userManager.ChangePasswordAsync(user, userPasswordChangeDto.CurrentPassword, userPasswordChangeDto.NewPassword);

                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(user); //Yakin zamanda bir degisiklik oldugunu gosterir...
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(user, userPasswordChangeDto.NewPassword, true, false);
                        TempData.Add("SuccessMessage", $"Şifreniz başarıyla güncellenmiştir.");

                        return View();
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(String.Empty, error.Description);
                        }
                        return View(userPasswordChangeDto);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, errorMessage: "Lütfen, girmiş olduğunuz şifrenizi kontrol ediniz...");
                    return View(userPasswordChangeDto);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, errorMessage: "Lütfen, girmiş olduğunuz şifrenizi kontrol ediniz...");
                return View(userPasswordChangeDto);
            }
        }

        [HttpGet]
        public ViewResult AccessDenied()
        {
            return View();
        }
    }
}
