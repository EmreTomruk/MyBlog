using MyBlog.Entities.Concrete;
using MyBlog.Entities.Dtos;
using System.Collections.Generic;

namespace MyBlog.UI.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int CategoriesCount { get; set; }
        public int ArticlesCount { get; set; } //aktif makaleler
        public int CommentsCount { get; set; }
        public int UsersCount { get; set; }


        public ArticleListDto Articles { get; set; } //tum makaleler
    }
}
