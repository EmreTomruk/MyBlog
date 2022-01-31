using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBlog.Data.Abstract;
using MyBlog.Data.Concrete;
using MyBlog.Data.Concrete.EntityFramework.Contexts;
using MyBlog.Entities.Concrete;
using MyBlog.Services.Abstract;
using MyBlog.Services.Concrete;
using MyBlog.Services.Utilities;

namespace MyBlog.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection, string connectionString) //UI katmani ile diger katmanlar arasinda kopru gorevi gorur...
        {
            serviceCollection.AddDbContext<MyBlogContext>(options => options.UseSqlServer(connectionString)); //AddDbContext'te ozunde bir Scope'tur...
            serviceCollection.AddIdentity<User, Role>(options => //Kaydedilecek kullaniciyla alakali ayarlar(sifre, e-mail'in unique olmasi vs.) yapilir...
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 10;
                options.Password.RequiredUniqueChars = 2;
                options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireLowercase = true; //Default: true
                //options.Password.RequireUppercase = false;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<MyBlogContext>();

            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();
            serviceCollection.AddScoped<IArticleService, ArticleManager>();
            serviceCollection.AddScoped<ICommentService, CommentManager>();
            serviceCollection.AddScoped<IMessages, Messages>();

            return serviceCollection;
        }
    }
}
