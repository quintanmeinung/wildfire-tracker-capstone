using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace project_wildfire_web.Models;

public partial class FireDatum
{
    public int FireId { get; set; }

    public Geometry Location { get; set; } = null!;

    public decimal RadiativePower { get; set; }

    public Geometry Polygon { get; set; } = null!;

    public int? WeatherId { get; set; }

    public virtual WeatherDatum? Weather { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
