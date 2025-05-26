using System.Text.Json;
using System.Text.Json.Serialization;


namespace project_wildfire_web.Models.ArcGis;

public class UsaArcGisAttributes
        {
            [JsonPropertyName("IncidentName")]
            public string IncidentName { get; set; } = string.Empty;

            [JsonPropertyName("PercentContained")]
            public double? PercentContained { get; set; }

            [JsonPropertyName("OBJECTID")]
            public int ObjectId { get; set; }

            [JsonPropertyName("UniqueFireIdentifier")]
            public string? UniqueFireIdentifier { get; set; }

            [JsonPropertyName("FireDiscoveryDateTime")]
            public long? FireDiscoveryDateTime { get; set; }

            [JsonPropertyName("DiscoveryAcres")]
            public double? DiscoveryAcres { get; set; }

            [JsonPropertyName("POOCounty")]
            public string? POOCounty { get; set; }

            [JsonPropertyName("POOState")]
            public string? POOState { get; set; }

            [JsonPropertyName("FireCause")]
            public string? FireCause { get; set; }
        }