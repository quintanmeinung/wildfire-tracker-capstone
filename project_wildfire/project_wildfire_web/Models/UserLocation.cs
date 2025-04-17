using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace project_wildfire_web.Models;

public partial class UserLocation
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;

    public string? Title { get; set; }

    public string? Address { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }
    
    [Range(1, 100, ErrorMessage = "Radius must be 1-100 miles")]
    public int NotificationRadius { get; set; } = 10;

    public virtual User User { get; set; } = null!;

    
}
