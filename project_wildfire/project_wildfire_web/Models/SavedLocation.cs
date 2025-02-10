using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace project_wildfire_web.Models;

public partial class SavedLocation
{
    public int LocationId { get; set; }

    public string Title { get; set; } = null!;

    public Geometry Location { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
