using System.Text.Json.Serialization;


namespace project_wildfire_web.Models.ArcGis;

public class ArcGisFeatureResponse
{
    [JsonPropertyName("features")]
    public List<ArcGisFeatureEnvelope>? Features { get; set; }
}
