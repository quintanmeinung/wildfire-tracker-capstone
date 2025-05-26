using System.Text.Json;
using System.Text.Json.Serialization;

namespace project_wildfire_web.Models.ArcGis;

public class ArcGisAttributes
{
    [JsonPropertyName("SourceOID")]
    public int SourceOID { get; set; }

    [JsonPropertyName("IncidentName")]
    public string IncidentName { get; set; } = string.Empty;

    [JsonPropertyName("InitialLatitude")]
    public double InitialLatitude { get; set; }

    [JsonPropertyName("InitialLongitude")]
    public double InitialLongitude { get; set; }

    [JsonPropertyName("DiscoveryAcres")]
    public double? DiscoveryAcres { get; set; }

    [JsonPropertyName("FinalAcres")]
    public double? FinalAcres { get; set; }

    [JsonPropertyName("FireDiscoveryDateTime")]
    public long? FireDiscoveryDateTime { get; set; }

    [JsonPropertyName("IncidentSize")]
    public double? IncidentSize { get; set; }

    [JsonPropertyName("UniqueFireIdentifier")]
    public string? UniqueFireIdentifier { get; set; }

    [JsonPropertyName("PercentContained")]
    public double? PercentContained { get; set; }
}