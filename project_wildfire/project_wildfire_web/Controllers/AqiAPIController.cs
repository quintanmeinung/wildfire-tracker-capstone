using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using project_wildfire_web.Models;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("api/[controller]")]
public class AqiController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly FireDataDbContext _context;

    public AqiController(IConfiguration configuration, FireDataDbContext context)
    {
        _configuration = configuration;
        _context = context;
    }

    [HttpGet("get-aqi-data")]
    public async Task<IActionResult> GetAqiData([FromQuery] string stationId)
    {
        var apiKey = _configuration["AQI_API_KEY"];

        if (string.IsNullOrEmpty(apiKey))
        {
            return StatusCode(500, "API Key is not configured.");
        }

        var url = $"https://api.waqi.info/feed/{stationId}/?token={apiKey}";

        using var client = new HttpClient();
        var response = await client.GetStringAsync(url);
        var json = JsonDocument.Parse(response);

        if (json.RootElement.GetProperty("status").GetString() != "ok")
        {
            return BadRequest("Failed to fetch AQI data.");
        }

        var data = json.RootElement.GetProperty("data");

        var aqi = data.GetProperty("aqi").GetInt32();
        var location = data.GetProperty("city").GetProperty("name").GetString();
        var lat = data.GetProperty("city").GetProperty("geo")[0].GetDouble();
        var lon = data.GetProperty("city").GetProperty("geo")[1].GetDouble();
        var dominantPollutant = data.GetProperty("dominentpol").GetString();

        // Handle nullable fields
        var pm25 = data.GetProperty("iaqi").TryGetProperty("pm25", out var pm25Value) ? pm25Value.GetProperty("v").GetDouble() : (double?)null;
        var pm10 = data.GetProperty("iaqi").TryGetProperty("pm10", out var pm10Value) ? pm10Value.GetProperty("v").GetDouble() : (double?)null;
        var no2 = data.GetProperty("iaqi").TryGetProperty("no2", out var no2Value) ? no2Value.GetProperty("v").GetDouble() : (double?)null;

        // Time data
        var lastUpdated = data.GetProperty("time").GetProperty("iso").GetString();

        // Attribution data
        var attributions = data.GetProperty("attributions")
            .EnumerateArray()
            .Select(a => new
            {
                name = a.GetProperty("name").GetString(),
                url = a.GetProperty("url").GetString()
            }).ToList();

        var result = new
        {
            aqi,
            location,
            lat,
            lon,
            dominantPollutant,
            pm25,
            pm10,
            no2,
            lastUpdated,
            attributions,
            color = GetAQIColor(aqi)
        };

        return Ok(result);
    }

    private string GetAQIColor(int aqi)
    {
        return aqi <= 50 ? "green" :
               aqi <= 100 ? "yellow" :
               aqi <= 150 ? "orange" :
               aqi <= 200 ? "red" :
               aqi <= 300 ? "purple" : "maroon";
    }
    
    [HttpGet("stations")]
public async Task<IActionResult> GetAqiStations()
{
    var stations = await _context.AqiStations
        .Select(s => new { s.Name, s.StationId })
        .ToListAsync();

    return Ok(stations);
}

}
