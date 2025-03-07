using project_wildfire_web.Models; 

namespace project_wildfire_web.Services;

public class NasaService : INasaService
{
    readonly HttpClient _httpClient;
    readonly ILogger <NasaService> _logger;

    public NasaService(HttpClient httpClient, ILogger<NasaService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<Fire>> GetFiresAsync()
    {
        throw new NotImplementedException();   
    }
}