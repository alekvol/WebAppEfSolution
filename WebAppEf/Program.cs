using Microsoft.Extensions.ObjectPool;
using System.Text;
using WebAppEF.Models;

namespace WebAppEF
{
    public class Program
    {
        internal static IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddSqlServer<ApplicationContext>(
                config.GetConnectionString("DefaultConnection"));

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Student}/{action=Index}/{id?}")
                .WithStaticAssets();


            app.Run();
        }
    }
}