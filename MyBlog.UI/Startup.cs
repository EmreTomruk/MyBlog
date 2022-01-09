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

            services.AddSession(); //Kullanici siteye giris yaptigi anda acilan server'da olusturulan oturumdur. Global bir degisken gibi dusunulebilir...
            services.AddAutoMapper(typeof(CategoryProfile), typeof(ArticleProfile)); //Derleme esnasinda AutoMapper CategoryProfile ve ArticleProfile siniflarini tarar ve ekler.  
            services.LoadMyServices();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Admin/User/Login"); //User:controller, Login:action
                options.LogoutPath = new PathString("/Admin/User/Logout");
                options.Cookie = new CookieBuilder
                {
                    Name = "MyBlog",
                    HttpOnly = true, //Guvenlik 
                    SameSite = SameSiteMode.Strict, //Cookie bilgileri sadece kendi sitemizden geldiginde islenecektir. Aksi halde sunucu bu bilgilerin nereden geldigine bakmaz sadece dogruluguna bakar...
                    SecurePolicy = CookieSecurePolicy.SameAsRequest //Http ise Http, Https ise Https'tir. Projeyi canliya almadan once "CookieSecurePolicy.Always" (Https -> Https) yap...
                };
                options.SlidingExpiration = true; //Kullaniciya siteye girdikten sonra sure tanir, bu sure zarfinda kullanicinin tekrar giris yapmasina gerek yoktur(ayni cookie bilgileri uzerinden). Sure tamamlandiktan sonra kullanici tekrar giris yaptiginda sure tekrar uzatilir...
                options.ExpireTimeSpan = System.TimeSpan.FromDays(7); //Eger kullanici 3. gun tekrar girerse tekrar 7 gun uzatilir...
                options.AccessDeniedPath = new PathString("/Admin/User/AccessDenied"); //Sisteme giris yapmis ama yetkisi olmayan bir yere giris yapmaya calisan kullanici icin...
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages(); //Bulunmayan bir sayfaya gidildiginde hata kodunu/mesajini verir... 
            }

            app.UseSession(); //Siralama onemli!!!
            app.UseStaticFiles();
            app.UseRouting(); 
            app.UseAuthentication();
            app.UseAuthorization();

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
