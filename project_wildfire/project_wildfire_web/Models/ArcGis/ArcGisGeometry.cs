using System.Text.Json;
using System.Text.Json.Serialization;

namespace project_wildfire_web.Models.ArcGis;

public class ArcGisGeometry
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }
}