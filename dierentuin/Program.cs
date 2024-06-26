using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using dierentuin.Data;
namespace dierentuin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<dierentuinContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("dierentuinContext") ?? throw new InvalidOperationException("Connection string 'dierentuinContext' not found.")));
            builder.Services.AddScoped<dierentuinSeeder>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            var serviceProvider = app.Services.CreateScope().ServiceProvider;
            var seeder = serviceProvider.GetRequiredService<dierentuinSeeder>();
            seeder.DataSeeder();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
