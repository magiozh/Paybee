using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Data;
using LibraryApp.Data.Models;
using LibraryApp.Data.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stripe;

namespace LibraryApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            StripeConfiguration.ApiKey = configuration.GetValue<string>("Stripe:SecretKey");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IBookService, BookService>();
            services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase(new Guid().ToString()));
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthorization();

            var scope = app.ApplicationServices.CreateScope();
            AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            SeedDatabase(context);

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Books}/{action=Index}/{id?}");
            });
        }

        private static void SeedDatabase(AppDbContext context)
        {
            context.Database.EnsureCreated();

            var _books = new List<Book>()
            {
                new Book()
                {
                    Id = new Guid("ab2bd817-98cd-4cf3-a80a-53ea0cd9c200"),
                    Title="Mendalami MS. Power BI",
                    Description="Mau jadi ahli data analyst? Atau pengen tambah skill keren jadi data analyst? Gabung yuk di Webinar Premium Strategi Sukses Data Analyst: Mendalami Microsoft Power BI.",
                    Author= "Elank",
                    Thumbnail = "PowerBI.png",
                    Price = "75000"
                },
                new Book()
                {
                    Id= new Guid("117366b8-3541-4ac5-8732-860d698e26a2"),
                    Title="Analisis Data Dengan Power BI",
                    Description="Dalam acara ini akan membahas mengenai penggunaan Power BI sebagai alat untuk analisis data, pembuatan visualisasi data, transformasi data dan lain sebagainya.",
                    Author= "Tarissa",
                    Thumbnail = "Analisis.png",
                    Price = "49000"
                },
                new Book()
                {
                    Id= new Guid("66ff5116-bcaa-4061-85b2-6f58fbb6db25"),
                    Title="Menguak Rahasia Scrapping Data",
                    Description="Siap untuk memulai petualangan Data Scraping? Bergabunglah dalam Kegiatan MariBelajar Webinar Premium \"Menguak Rahasia Scrapping Data\", dalam kegiatan ini peserta akan mempelajari teknik, tools, etika, dan contoh penggunaannya dalam mengumpulkan data dari berbagai sumber.",
                    Author= "Irfan",
                    Thumbnail = "Scrapping.png",
                    Price = "75000"
                },
                new Book()
                {
                    Id =  new Guid("cd5089dd-9754-4ed2-b44c-488f533243ef"),
                    Title = "Analisis Data dengan Ms. Excel",
                    Description = "Maribelajar mengadakan Webinar Premium Analisis Data dengan Microsoft Excel Power Pivot. Dalam kegiatan ini peserta akan mempelajari cara mengolah data secara efektif menggunakan Microsoft Excel Power Pivot.",
                    Author = "Siti",
                    Thumbnail = "Pivot.png",
                    Price = "25000"
                },
            };

            context.Books.AddRange(_books);
            context.SaveChanges();
        }
    }
}
