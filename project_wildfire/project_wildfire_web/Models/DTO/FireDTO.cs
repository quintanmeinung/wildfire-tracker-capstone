using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using project_wildfire_web.Models;
using project_wildfire_web.Models.DTO;

namespace project_wildfire_web.Models.DTO
{
    public partial class FireDTO
    {
        public int FireId { get; set; }

        [Required]
        [Name("latitude")]
        public decimal Latitude { get; set; }

        [Required]
        [Name("longitude")]
        public decimal Longitude { get; set; }

        [Required]
        [Name("frp")]
        public decimal RadiativePower { get; set; }

         public bool IsAdminFire { get; set; } 

}

}

namespace project_wildfire_web.ExtensionsMethods
{
    public static class FireDtoExtensions
    {
        public static FireDTO ToFireDTO (this Fire fire)
        {   
            // Check for null radiative power
            decimal? radPower = fire.RadiativePower;

            // Set to 0 if null
            radPower = radPower ?? 0.0m;

            return new FireDTO
            {
                FireId = fire.FireId,
                Latitude = fire.Latitude,
                Longitude = fire.Longitude,

                // Cast radPower back to non-null decimal
                RadiativePower = (decimal)radPower,
                IsAdminFire = fire.IsAdminFire
            };

        }

        public static Fire ToFire (this FireDTO fireDTO)
        {
            return new Fire
            {
                Latitude = fireDTO.Latitude,
                Longitude = fireDTO.Longitude,
                RadiativePower = fireDTO.RadiativePower,
                IsAdminFire = fireDTO.IsAdminFire
            };
        }
    }
}
