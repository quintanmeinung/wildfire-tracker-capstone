using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using project_wildfire_web.Models;

public class NotificationService
{
    private readonly FireDataDbContext _context;
    private readonly ILogger<NotificationService> _logger;
    private readonly string _textbeltApiKey;

    public NotificationService(
        FireDataDbContext context,
        ILogger<NotificationService> logger,
        IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _textbeltApiKey = configuration["Textbelt:ApiKey"];
    }

   public async Task CheckFiresNearUserLocationsAsync(string userId, string PhoneNumber)
{
    var fires = await _context.Fires.ToListAsync();

    var userLocations = await _context.UserLocations
        .Where(loc => loc.UserId == userId)
        .ToListAsync();

    foreach (var location in userLocations)
    {
        foreach (var fire in fires)
        {
            double distance = CalculateDistance(
                (double)location.Latitude,
                (double)location.Longitude,
                (double)fire.Latitude,
                (double)fire.Longitude);

            if (distance <= location.NotificationRadius)
            {
                string alertMessage = $"🔥 Fire alert near '{location.Title}' at {location.Address}! Within {distance:F2} miles.";

                _logger.LogInformation(alertMessage);

                await SendSmsNotificationAsync(PhoneNumber, alertMessage);
            }
        }
    }
}


    // Haversine formula for calculating distance between two coordinates in miles
    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        double R = 3958.8; 
        double dLat = DegreesToRadians(lat2 - lat1);
        double dLon = DegreesToRadians(lon2 - lon1);
        double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                   Math.Cos(DegreesToRadians(lat1)) *
                   Math.Cos(DegreesToRadians(lat2)) *
                   Math.Pow(Math.Sin(dLon / 2), 2);
        double c = 2 * Math.Asin(Math.Sqrt(a));
        return R * c;
    }

    private double DegreesToRadians(double deg)
    {
        return deg * Math.PI / 180;
    }

    private async Task SendSmsNotificationAsync(string phone, string message)
    {
        using var httpClient = new HttpClient();

        var payload = new
        {
            phone = phone,
            message = message,
            key = _textbeltApiKey  // e.g., "textbelt_test"
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("https://textbelt.com/text", content);

        var responseContent = await response.Content.ReadAsStringAsync();
        _logger.LogInformation($"📨 SMS API response: {responseContent}");
    }

}
