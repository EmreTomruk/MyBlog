using MyBlog.Entities.Concrete;
using System.Collections.Generic;

namespace MyBlog.UI.Areas.Admin.Models
{
    public class UserWithRolesViewModel
    {
        public User User { get; set; }
        public IList<string> Roles { get; set; }
    }
}
