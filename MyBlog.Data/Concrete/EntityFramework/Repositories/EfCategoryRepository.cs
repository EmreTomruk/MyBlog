using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abstract;
using MyBlog.Data.Concrete.EntityFramework.Contexts;
using MyBlog.Entities.Concrete;
using MyBlog.Shared.Data.Concrete.EntityFramework;

namespace MyBlog.Data.Concrete.EntityFramework.Repositories
{
    public class EfCategoryRepository : EfEntityRepositoryBase<Category>, ICategoryRepository
    {
        public EfCategoryRepository(DbContext context) : base(context) { }

        public async Task<Category> GetById(int categoryId)
        {
           return await MyBlogContext.Categories.SingleOrDefaultAsync(c => c.Id == categoryId);
        }

        private MyBlogContext MyBlogContext
        {
            get
            {
                return _context as MyBlogContext; //_context MyBlogContext oldugunu bilmiyordu, onun icin bu islemi yapti...
            }
        }
    }
}
