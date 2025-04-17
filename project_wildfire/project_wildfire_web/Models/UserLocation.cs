using System;
using System.Collections.Generic;

namespace project_wildfire_web.Models;

public partial class UserLocation
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;

    public string? Title { get; set; }

    public string? Address { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

<<<<<<< HEAD
    public string? Address { get; set; }

    public int NotificationRadius { get; set; } 
=======
    public int NotificationRadius { get; set; }
>>>>>>> dev

    public virtual User User { get; set; } = null!;
}
