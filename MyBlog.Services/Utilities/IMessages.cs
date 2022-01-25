using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Services.Utilities
{
    public interface IMessages
    {
        public string NotFound(bool isPlural, string pictureName);
    }
}
