using MyBlog.Entities.Dtos;

namespace MyBlog.UI.Areas.Admin.Models
{
    public class UserAddAjaxViewModel
    {
        public UserAddDto UserAddDto { get; set; } //Hata mesajlari icin kullanacagiz...
        public string UserAddPartial { get; set; } //Post islemi yapildiginda doner...
        public UserDto UserDto { get; set; }
    }
}
