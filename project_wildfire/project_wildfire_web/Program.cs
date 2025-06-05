using project_wildfire_web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using project_wildfire_web.Areas.Identity.Data;
using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.DAL.Concrete;
using System.Text.Json;
using System.Text.Json.Serialization;
using project_wildfire_web.Services;
using System.Net.Http.Headers;

namespace project_wildfire_web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        

        // Retrieve primary DB connection string
        var WebfireConnectionString = builder.Configuration.GetConnectionString("WebfireConnectionString");
        Console.WriteLine($"[STARTUP] WebfireConnectionString: {WebfireConnectionString ?? "null"}");
        var dbPassword = builder.Configuration["WildfireProj:DBPassword"];
        WebfireConnectionString = WebfireConnectionString.Replace("placeholder", dbPassword);
        
        // Add primary DB Context with NetTopologySuite support
        builder.Services.AddDbContext<FireDataDbContext>(options =>
            options.UseSqlServer(
                WebfireConnectionString,
                x => x.UseNetTopologySuite())
                 .EnableSensitiveDataLogging()
                );

        // Retrieve Identity DB connection string
        var AuthConnectionString = builder.Configuration.GetConnectionString("AuthConnectionString");

        // Add Identity DB context
        builder.Services.AddDbContext<WebfireIdentityDbContext>(options =>
            options.UseSqlServer(AuthConnectionString));

        // Add Identity services
        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<WebfireIdentityDbContext>();


        // Add API configuration
        string? firmsApiKey = builder.Configuration["NASA:FirmsApiKey"];
        if (string.IsNullOrEmpty(firmsApiKey))
        {
            throw new Exception("NASA:FirmsApiKey is missing from configuration");
        }
        string apiBaseUrl = "https://firms.modaps.eosdis.nasa.gov/api/country/csv/ApiKeyHere/VIIRS_SNPP_NRT/PER/1/2025-04-20";

        string fullUri = apiBaseUrl.Replace("ApiKeyHere", firmsApiKey);

        builder.Services.AddHttpClient<INasaService, NasaService>((httpClient, services) =>
        {
            // Verify with Nasa API if headers are correct
            var configuration = services.GetRequiredService<IConfiguration>();

            httpClient.BaseAddress = new Uri(fullUri);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            return new NasaService(httpClient, services.GetRequiredService<ILogger<NasaService>>(), configuration);
        });

        builder.Services.AddHttpClient<IArcGisService, ArcGisService>(client =>
        {
            client.BaseAddress = new Uri("https://services3.arcgis.com/T4QMspbfLg3qTGWY/arcgis/rest/services/WFIGS_Incident_Locations_Current/FeatureServer/0/");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        //Adding Notification Service
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddHostedService<NotificationBackgroundService>();

        builder.Services.AddScoped<OpenAIService>();

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Logging.ClearProviders(); 
        builder.Logging.AddConsole();
        
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals;
        });

        // Add repository services
        builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IWildfireRepository, WildfireRepository>();
        builder.Services.AddScoped<ILocationRepository, LocationRepository>();
        builder.Services.AddScoped<IUserFireSubRepository, UserFireSubsRepository>();
        builder.Services.AddHttpClient();
        
        //adding swagger
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Add session management
        builder.Services.AddSession();
        builder.Configuration.AddUserSecrets<Program>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

         app.UseSwagger();
         app.UseSwaggerUI(options => {
                options.SwaggerEndpoint("/swagger/v1/swagger.json","wildfire API v1");
                options.RoutePrefix = "swagger";
         });

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();
        app.MapRazorPages();

        //Session storage middleware
        app.UseSession();
        //for aqi controller
        app.MapControllers();

        //User RoleManager + UserManager Scope
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

            // Seed "Admin" role if it doesn't exist
            string adminRole = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
                Console.WriteLine("[ROLES] Admin role created.");
            }

            //Admin Accounts
            var adminEmails = new List<string>
            {
                "quintanscotmeinung@gmail.com",
                "miked@wou.edu",
                "jonnyr687@gmail.com",
                "adminone@example.com"
            };

            foreach (var email in adminEmails)
            {
                var user = await userManager.FindByEmailAsync(email);
                if (user != null && !(await userManager.IsInRoleAsync(user, "Admin")))
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                    Console.WriteLine($"[ROLES] {email} assigned to Admin role.");
                }
                else if (user == null)
                {
                    Console.WriteLine($"[ROLES] Admin user {email} not found.");
                }
            }
        }
        await app.RunAsync();
    }
}
