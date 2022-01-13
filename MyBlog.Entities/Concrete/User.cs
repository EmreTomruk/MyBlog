using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MyBlog.Entities.Concrete
{
    public class User : IdentityUser<int>
    {

        public string Picture { get; set; }
        public ICollection<Article> Articles { get; set; }
    }
}
