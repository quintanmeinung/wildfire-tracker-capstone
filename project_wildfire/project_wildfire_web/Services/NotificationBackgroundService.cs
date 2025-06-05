using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using project_wildfire_web.Models;

public class NotificationBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<NotificationBackgroundService> _logger;

    public NotificationBackgroundService(IServiceProvider serviceProvider, ILogger<NotificationBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       /* _logger.LogInformation("🔥 NotificationBackgroundService is starting...");

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckUserLocationsAsync();

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Run every 1 minute
        }*/
    }

private async Task CheckUserLocationsAsync()
{
    using (var scope = _serviceProvider.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<FireDataDbContext>();
        var notificationService = scope.ServiceProvider.GetRequiredService<NotificationService>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        var now = DateTime.UtcNow;

        var userLocations = await context.UserLocations.ToListAsync();

        foreach (var location in userLocations)
        {
            var lastSent = location.LastNotificationSent ?? DateTime.MinValue;
            var intervalMinutes = location.TimeInterval == 0 ? 1 : location.TimeInterval * 60; // convert hours to minutes

            if ((now - lastSent).TotalMinutes >= intervalMinutes)
            {
                var identityUser = await userManager.FindByIdAsync(location.UserId);

                if (identityUser != null && !string.IsNullOrEmpty(identityUser.PhoneNumber))
                {
                    _logger.LogInformation($"Checking fires for user {identityUser.Id}, location {location.Title}");

                    await notificationService.CheckFiresNearUserLocationsAsync(identityUser.Id, identityUser.PhoneNumber);

                    location.LastNotificationSent = now;
                }
            }
        }

        await context.SaveChangesAsync();
    }
}


}
