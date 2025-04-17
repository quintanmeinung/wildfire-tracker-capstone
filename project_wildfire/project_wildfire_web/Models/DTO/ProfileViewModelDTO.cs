using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using project_wildfire_web.Models;
using NetTopologySuite.Geometries;
using project_wildfire_web.Models.DTO;



namespace project_wildfire_web.Models.DTO
{
    public partial class ProfileViewModelDTO
    {
        [Required]
        [Name("userId")]
        public required string UserId { get; set; }

        [Name("FirstName")]
        public string? FirstName { get; set; }

        [Name("LastName")]
        public string? LastName { get; set; }

        [Required]
        [Name("Email")]
        public required string Email { get; set; }

        [Name("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [Name("Location")]
        public string? Location { get; set; }

        [Name("SavedLocations")]
        public ICollection<UserLocationDTO> SavedLocations { get; set; } = new List<UserLocationDTO>();

        [Name("FireSubscriptions")]
        public ICollection<FireDTO> FireSubscriptions { get; set; } = new List<FireDTO>();

    }
}


namespace project_wildfire_web.ExtensionsMethods
{
    public static class ProfileViewModelDtoExtensions
    {
        public static ProfileViewModelDTO ToProfileViewModelDTO (this ProfileViewModel pvm)
        {   
            if (pvm.Email == null)
            {
                pvm.Email = string.Empty;
            }
            return new ProfileViewModelDTO
            {
            UserId = pvm.Id,
            FirstName = pvm.FirstName,
            LastName = pvm.LastName,
            Email = pvm.Email,
            PhoneNumber = pvm.PhoneNumber,
            Location = pvm.Location,
            SavedLocations = pvm.SavedLocations.Select(ul => ul.ToUserLocationDTO()).ToList(),
            FireSubscriptions = pvm.FireSubscriptions.Select(f => f.ToFireDTO()).ToList()
            };

        }

        public static ProfileViewModel ToProfileViewModel (this ProfileViewModelDTO pvm)
        {
            if (pvm == null)
            {
                throw new ArgumentNullException(nameof(pvm), "UserLocationDTO cannot be null.");
            }
            if (string.IsNullOrEmpty(pvm.UserId))
            {
                throw new ArgumentException("UserId cannot be null or empty.", nameof(pvm.UserId));
            }
            return new ProfileViewModel
            {
                Id = pvm.UserId,
                FirstName = pvm.FirstName,
                LastName = pvm.LastName,
                Email = pvm.Email,
                PhoneNumber = pvm.PhoneNumber,
                Location = pvm.Location,
                SavedLocations = pvm.SavedLocations.Select(ul => ul.ToUserLocation()).ToList(),
                FireSubscriptions = pvm.FireSubscriptions.Select(f => f.ToFire()).ToList()
            };
        }
    }

}