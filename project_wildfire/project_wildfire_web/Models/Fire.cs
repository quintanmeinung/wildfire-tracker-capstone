using NetTopologySuite.Geometries;

namespace project_wildfire_web.Models;

public partial class Fire
{
    public int FireId { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public decimal? RadiativePower { get; set; }

    public Geometry? Polygon { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<UserFireSubscription> UserSubscriptions { get; set; } = new List<UserFireSubscription>();
}
