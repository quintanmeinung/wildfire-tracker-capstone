using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
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

    return Ok(response);
}
}
