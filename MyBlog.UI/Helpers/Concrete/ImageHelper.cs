using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MyBlog.Entities.Dtos;
using MyBlog.Shared.Utilities.Extensions;
using MyBlog.Shared.Utilities.Results.Abstract;
using MyBlog.Shared.Utilities.Results.ComplexTypes;
using MyBlog.Shared.Utilities.Results.Concrete;
using MyBlog.UI.Helpers.Abstract;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MyBlog.UI.Helpers.Concrete
{
    public class ImageHelper : IImageHelper
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _wwwroot;
        private readonly string imgFolder = "img";

        public ImageHelper(IWebHostEnvironment env)
        {
            _env = env;
            _wwwroot = _env.WebRootPath;
        }

        public async Task<IDataResult<ImageUploadedDto>> UploadUserImage(string userName, IFormFile pictureFile, string folderName="userImages") //Kullanici resimlerini "userImages" klasorune eklemek isteriz! Boylece her kullanici resmi yukledigimizde 2 parametre ile islemi gerceklestirebiliyor olacagiz...
        {
            if (!Directory.Exists($"{_wwwroot}/{imgFolder}/{folderName}")) //Eger boyle bir klasor yoksa...
            {
                Directory.CreateDirectory($"{_wwwroot}/{imgFolder}/{folderName}"); //Olustur...
            }

            string oldFileName = Path.GetFileNameWithoutExtension(pictureFile.FileName);      
            string fileExtension = Path.GetExtension(pictureFile.FileName);
            DateTime dateTime = DateTime.Now;

            string newFileName = $"{userName}_{dateTime.FullDateAndTimeStringWithUnderscore()}{fileExtension}";
            var path = Path.Combine($"{_wwwroot}/{imgFolder}/{folderName}", newFileName); 

            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await pictureFile.CopyToAsync(stream); //Islem sonunda resmimiz "~/img/..." klasorune aktarilmis oluyor...
            }

            return new DataResult<ImageUploadedDto>(ResultStatus.Success, $"{userName} adlı kullanıcının resmi başarıyla yüklenmiştir.", 
            new ImageUploadedDto
            {
                FullName = $"{folderName}/{newFileName}",
                OldName = oldFileName,
                Extension = fileExtension,
                FolderName = folderName,
                Path = path,
                Size = pictureFile.Length 
            }); //EmreTomruk_587_5_38_12_3_10_2020.png -> "~/img/user.Picture"            
        }

        public IDataResult<ImageDeletedDto> Delete(string pictureName)
        {
            var fileToDelete = Path.Combine($"{_wwwroot}/{imgFolder}", pictureName);  //silinecek dosyayi bul

            if (System.IO.File.Exists(fileToDelete))
            {
                var fileInfo = new FileInfo(fileToDelete);
                var imageDeletedDto = new ImageDeletedDto //fileInfo silinecegi icin onu DataResult icinde kullanamayiz, bu yuzden imageDeletedDto'yu urettik...
                {
                    FullName=pictureName,
                    Extendion=fileInfo.Extension,
                    Path=fileInfo.FullName,
                    Size=fileInfo.Length
                };
                System.IO.File.Delete(fileToDelete);

                return new DataResult<ImageDeletedDto>(ResultStatus.Success, imageDeletedDto);
            }
            else
            {
                return new DataResult<ImageDeletedDto>(ResultStatus.Error, $"Böyle bir resim bulunamadı", null);
            }
        }
    }
}
