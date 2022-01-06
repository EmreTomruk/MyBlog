using MyBlog.Entities.Dtos;

namespace MyBlog.UI.Areas.Admin.Models
{
    public class CategoryUpdateAjaxViewModel
    {
        public CategoryUpdateDto CategoryAddDto { get; set; }
        public string CategoryUpdatePartial { get; set; }
        public CategoryDto CategoryDto { get; set; }
    }
}
