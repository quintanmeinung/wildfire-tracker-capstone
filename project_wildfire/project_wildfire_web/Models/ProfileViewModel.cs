using Microsoft.AspNetCore.Identity;

namespace project_wildfire_web.Models;
public class ProfileViewModel
{
    public required string Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Location { get; set; }

    // SavedLocations has its own table, so it will need to be populated elsewhere
    public ICollection<UserLocation> SavedLocations { get; set; } = [];
    public required ICollection<Fire> FireSubscriptions { get; set; }

}