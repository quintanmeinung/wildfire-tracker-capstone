using project_wildfire_web.Models;
using Microsoft.EntityFrameworkCore;

namespace project_wildfire_web;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var DbPassword = builder.Configuration["WildfireProj:DBPassword"];
        var PartialConnectionString = builder.Configuration.GetConnectionString("WildfireAzure");
        var FullConnectionString = PartialConnectionString.Replace("{password}", DbPassword);

        builder.Services.AddDbContext<WildfireDbContext>(options =>
            options.UseSqlServer(FullConnectionString));

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();

        app.Run();
    }
}
