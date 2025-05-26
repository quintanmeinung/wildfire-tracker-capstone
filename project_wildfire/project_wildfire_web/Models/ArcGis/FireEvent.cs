using System;

namespace project_wildfire_web.Models.ArcGis

{
    /// <summary>
    /// Data model representing a wildfire event as returned by the ArcGIS service.
    /// </summary>
    public class FireEvent
    {
        /// <summary>The unique identifier for the fire event (OBJECTID).</summary>
        public string UniqueFireIdentifier { get; set; } = string.Empty;

        /// <summary>The name of the fire.</summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>Latitude of the fire (WGS84).</summary>
        public double Latitude { get; set; }

        /// <summary>Longitude of the fire (WGS84).</summary>
        public double Longitude { get; set; }

        /// <summary>Total area burned in acres.</summary>
        public double AcreageBurned { get; set; }

        /// <summary>Percentage of the fire contained (0–100).</summary>
        public double PercentageContained { get; set; }

        /// <summary>County of origin</summary>
        public string POOCounty { get; set; } = string.Empty;

        /// <summary>State of origin</summary>
        public string POOState { get; set; } = string.Empty;

        /// <summary>Cause of the fire</summary>
        public string FireCause { get; set; } = string.Empty;

        /// <summary>UTC start date/time of the fire event.</summary>
        public DateTime StartDate { get; set; }
    }
}