using project_wildfire_web.Models;
using CsvHelper;
using CsvHelper.Configuration;
using NetTopologySuite.Geometries; 
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace project_wildfire_web.Services;


public class NasaService : INasaService
{
    readonly HttpClient _httpClient;
    readonly ILogger <NasaService> _logger;
    readonly IConfiguration _configuration;

    public NasaService(HttpClient httpClient, ILogger<NasaService> logger, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _logger = logger;
        _configuration = configuration;
    }

    //fetches fires from built-in coordinates
    public async Task<List<Fire>> GetFiresAsync()
    {
       
        _logger.LogInformation("Calling NASA Firms API");

        string apiKey = _configuration["NASA:FirmsApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError("NASA FIRMS API key missing");
            return new List<Fire>(); // Return empty list instead of null
        }

        string endpoint = $"https://firms.modaps.eosdis.nasa.gov/api/area/csv/{apiKey}/VIIRS_SNPP_NRT/-130,40,-110,50/1/2025-03-02";

        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Failed to fetch NASA data - {response.StatusCode}");
            return new List<Fire>();
        }

        string responseBody = await response.Content.ReadAsStringAsync();
        using var reader = new StringReader(responseBody);
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) { HeaderValidated = null };
        using var csv = new CsvReader(reader, csvConfig);
        
        return csv.GetRecords<Fire>().ToList();
    }



}