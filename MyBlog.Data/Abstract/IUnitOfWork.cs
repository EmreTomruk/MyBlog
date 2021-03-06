using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;

namespace MyBlog.Data.Abstract
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IArticleRepository Articles { get; } // unitofwork.Articles
        ICategoryRepository Categories { get; }
        ICommentRepository Comments { get; }

        // _unitOfWork.Categories.AddAsync(category);
        //_unitOfWork.Users.AddAsync(user);
        //_unitOfWork.SaveAsync();
        Task<int> SaveAsync();
    }
}
