using Microsoft.AspNetCore.Http;
using MyBlog.Entities.Dtos;
using MyBlog.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;

namespace MyBlog.UI.Helpers.Abstract
{
    public interface IImageHelper
    {
        Task<IDataResult<ImageUploadedDto>> UploadUserImage(string userName, IFormFile pictureFile, string folderName = "userImages");

        IDataResult<ImageDeletedDto> Delete(string pictureName);
    }
}
