namespace project_wildfire_web.Models;
public class ProfileViewModel
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Location { get; set; }
    public required ICollection<UserLocation> SavedLocations { get; set; }
    public required ICollection<Fire> FireSubscriptions { get; set; }
}