using System.Text.Json.Serialization;

namespace project_wildfire_web.Models.ArcGis;

public class ArcGisResponse
{
    [JsonPropertyName("features")]
    public List<ArcGisFeature>? Features { get; set; }
}