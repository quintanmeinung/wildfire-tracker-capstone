using System.Text.Json.Serialization;

namespace project_wildfire_web.Models.ArcGis;

public class ArcGisFeatureEnvelope
{
    [JsonPropertyName("attributes")]
    public UsaArcGisAttributes Attributes { get; set; } = null!;

    [JsonPropertyName("geometry")]
    public ArcGisGeometry Geometry { get; set; } = null!;
}