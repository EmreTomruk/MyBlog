using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MyBlog.Entities.Concrete;
using MyBlog.Shared.Data.Abstract;

namespace MyBlog.Data.Abstract
{
    public interface ICategoryRepository : IEntityRepository<Category>
    {
        Task<Category> GetById(int categoryId);
    }
}
