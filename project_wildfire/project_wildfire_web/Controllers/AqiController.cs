using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AqiController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AqiController(IConfiguration configuration)
    {
        _configuration = configuration;
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

        var result = new
        {
            aqi,
            location,
            lat,
            lon,
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
}

