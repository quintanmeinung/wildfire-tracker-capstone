using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_wildfire_web.Models;

public partial class UserLocation
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string UserId { get; set; } = null!;

    public string? Title { get; set; }

    public string? Address { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public int TimeInterval { get; set; } 
    
    public DateTime? LastNotificationSent { get; set; }
    
    [Range(1, 100, ErrorMessage = "Radius must be 1-100 miles")]
    public int NotificationRadius { get; set; } = 25; //Used to be this     public int NotificationRadius { get; set; } just in case it breaks anything

    public virtual User User { get; set; } = null!;

    
}
