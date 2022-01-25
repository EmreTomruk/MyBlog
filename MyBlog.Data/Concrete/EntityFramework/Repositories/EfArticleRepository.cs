using Microsoft.EntityFrameworkCore;
using MyBlog.Data.Abstract;
using MyBlog.Data.Concrete.EntityFramework.Contexts;
using MyBlog.Entities.Concrete;
using MyBlog.Shared.Data.Concrete.EntityFramework;
using System.Threading.Tasks;

namespace MyBlog.Data.Concrete.EntityFramework.Repositories
{
    public class EfArticleRepository : EfEntityRepositoryBase<Article>, IArticleRepository
    {
        public EfArticleRepository(DbContext context) : base(context) { }

        public async Task<Article> GetById(int articleId)
        {
            return await _myBlogContext.Articles.SingleOrDefaultAsync(a => a.Id== articleId) ;
        }

        public MyBlogContext _myBlogContext
        {
            get 
            {
                return _context as MyBlogContext;
            }
        }
    }
}
