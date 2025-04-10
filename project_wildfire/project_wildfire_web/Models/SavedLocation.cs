using System.ComponentModel.DataAnnotations;

public class SavedLocation
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } // FK to IdentityUser

    [Required]
    public string Name { get; set; }

    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }
}
