using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using MyBlog.Data.Abstract;
using MyBlog.Data.Concrete;
using MyBlog.Data.Concrete.EntityFramework.Contexts;
using MyBlog.Services.Abstract;
using MyBlog.Services.Concrete;

namespace MyBlog.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection) //
        {
            serviceCollection.AddDbContext<MyBlogContext>(); //AddDbContext'te ozunde bir Scope'tur...

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();
            serviceCollection.AddScoped<IArticleService, ArticleManager>();

            return serviceCollection;
        }
    }
}
