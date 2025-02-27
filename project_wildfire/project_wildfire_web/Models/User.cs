using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace project_wildfire_web.Models;

public partial class User
{
    public required string UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName {get; set; }

    public Geometry? UserLocation { get; set; }

    public virtual ICollection<FireDatum> Fires { get; set; } = new List<FireDatum>();

    public virtual ICollection<SavedLocation> Locations { get; set; } = new List<SavedLocation>();
}
