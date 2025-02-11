using System;
using System.Collections.Generic;

namespace project_wildfire_web.Models;

public partial class WeatherDatum
{
    public int WeatherId { get; set; }

    public int Temperature { get; set; }

    public string WindSpeedDirection { get; set; } = null!;

    public int AirQualityIndex { get; set; }

    public virtual ICollection<FireDatum> FireData { get; set; } = new List<FireDatum>();
}
