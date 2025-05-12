using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using project_wildfire_web.Models;
using project_wildfire_web.Models.DTO;



namespace project_wildfire_web.Models.DTO
{
public partial class UserLocationDTO
{
    [Required]
    [Name("userId")]
    public string? UserId { get; set; }

    [Required]
    [Name("title")]
    public string? Title { get; set; }

    [Name("address")]
    public string? Address { get; set; }

    [Required]
    [Name("latitude")]
    public string? Latitude { get; set; }

    [Required]
    [Name("longitude")]
    public string? Longitude { get; set; }

    [Required]
    [Name("radius")]
    public string? Radius { get; set; }

}

}

namespace project_wildfire_web.ExtensionsMethods
{
    public static class UserLocationDtoExtensions
    {
        public static UserLocationDTO ToUserLocationDTO (this UserLocation ul)
        {   
            return new UserLocationDTO
            {
            UserId = ul.UserId,
            Latitude = ul.Latitude.ToString(),
            Longitude = ul.Longitude.ToString(),
            Title = ul.Title,
            Address = ul.Address,
            Radius = ul.NotificationRadius.ToString()
            };

        }

        public static UserLocation ToUserLocation (this UserLocationDTO ul)
        {
            if (ul == null)
            {
                throw new ArgumentNullException(nameof(ul), "UserLocationDTO cannot be null.");
            }
            if (string.IsNullOrEmpty(ul.UserId))
            {
                throw new ArgumentException("UserId cannot be null or empty.", nameof(ul.UserId));
            }
            if (string.IsNullOrEmpty(ul.Latitude) || string.IsNullOrEmpty(ul.Longitude))
            {
                throw new ArgumentException("Latitude and Longitude cannot be null or empty.", nameof(ul));
            }
            return new UserLocation
            {
                UserId = ul.UserId,
                Latitude = decimal.Parse(ul.Latitude),
                Longitude = decimal.Parse(ul.Longitude),
                Title = ul.Title,
                Address = ul.Address,
                NotificationRadius = int.Parse(ul.Radius)
            };
        }
    }
}