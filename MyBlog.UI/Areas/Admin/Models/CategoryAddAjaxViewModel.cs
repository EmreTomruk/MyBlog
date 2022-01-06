using MyBlog.Entities.Dtos;

namespace MyBlog.UI.Areas.Admin.Models
{
    public class CategoryAddAjaxViewModel
    {
        public CategoryAddDto CategoryAddDto { get; set; }
        public string CategoryAddPartial { get; set; } //Post islemi yapildiginda doner...
        public CategoryDto CategoryDto { get; set; }
    }
}
