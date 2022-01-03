using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using MyBlog.Services.AutoMapper.Profiles;
using MyBlog.Services.Extensions;
using System.Text.Json.Serialization;

namespace MyBlog.Mvc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation().AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); //
                opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; //Nested Objelerde bu objeler birbirini referans ettiginde bunlari sorunsuz bicimde cevirmemizi saglar(category-makale)...
            }); 
            services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile)); //Derleme esnasinda AutoMapper CategoryProfile ve ArticleProfile siniflarini tarar ve ekler.  
            services.LoadMyServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages(); //Bulunmayan bir sayfaya gidildiginde hata kodunu/mesajini verir... 
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}" 
                    );
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
