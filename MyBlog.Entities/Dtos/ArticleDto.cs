using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyBlog.Shared.Entities.Abstract;
using MyBlog.Entities.Concrete;
using MyBlog.Shared.Utilities.Results.ComplexTypes;

namespace MyBlog.Entities.Dtos
{
    public class ArticleDto : DtoGetBase
    {
        public Article Article { get; set; }
        public override ResultStatus ResultStatus { get; set; }
    }
}
