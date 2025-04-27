using project_wildfire_web.Models;
using CsvHelper;
using CsvHelper.Configuration;
using NetTopologySuite.Geometries; 
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using project_wildfire_web.Models.DTO;

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
    //Orignal Code
    
    public async Task<List<FireDTO>> GetFiresAsync()
    {
       
        _logger.LogInformation("Calling NASA Firms API");

        string apiKey = _configuration["NASA:FirmsApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError("NASA FIRMS API key missing");
            return new List<FireDTO>(); // Return empty list instead of null
        }

        //Code for modifying the year/month/day of wildfire occurences
        //Needs to be updated for filtering
        //string endpoint = $"https://firms.modaps.eosdis.nasa.gov/api/area/csv/{apiKey}/VIIRS_SNPP_NRT/-130,40,-110,50/1/2025-03-02";
        //string endpoint = $"https://firms.modaps.eosdis.nasa.gov/api/country/csv/{apiKey}/VIIRS_SNPP_NRT/USA/1/2025-03-15";
        string currentDate = DateTime.UtcNow.ToString("2025-04-21");
        string endpoint = $"https://firms.modaps.eosdis.nasa.gov/api/country/csv/{apiKey}/VIIRS_SNPP_NRT/USA/1/{currentDate}";


        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Failed to fetch NASA data - {response.StatusCode}");
            return new List<FireDTO>();
        }

        string responseBody = await response.Content.ReadAsStringAsync();


        using var reader = new StringReader(responseBody);
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture) { HeaderValidated = null };

        List<FireDTO> wildFires = new List<FireDTO> ();
        try{
             var csv = new CsvReader(reader, csvConfig);
             wildFires = csv.GetRecords<FireDTO>().ToList();
        }
        catch (Exception ex)
            {
                _logger.LogError($"Error parsing CSV: {ex.Message}");
                //return StatusCode(500, "Error processing wildfire data");
            }
        //using var csv = new CsvReader(reader, csvConfig);
        
       // return csv.GetRecords<Fire>().ToList();
       return wildFires;
    }

    //Modified Code to accept Fires by date for Controller
    public async Task<List<FireDTO>> GetFiresByDateAsync(DateTime date)
    {
        _logger.LogInformation($"Calling NASA FIRMS API for date: {date:yyyy-MM-dd}");

        string apiKey = _configuration["NASA:FirmsApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError("NASA FIRMS API key missing");
            return new List<FireDTO>();
        }

        string formattedDate = date.ToString("yyyy-MM-dd");
        string endpoint = $"https://firms.modaps.eosdis.nasa.gov/api/country/csv/{apiKey}/VIIRS_SNPP_NRT/USA/1/{formattedDate}";

        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            _logger.LogError($"Failed to fetch NASA data - {response.StatusCode}");
            return new List<FireDTO>();
        }

        string responseBody = await response.Content.ReadAsStringAsync();

        using var reader = new StringReader(responseBody);
        var csvConfig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture) { HeaderValidated = null };

        try
        {
            var csv = new CsvReader(reader, csvConfig);
            return csv.GetRecords<FireDTO>().ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error parsing CSV: {ex.Message}");
            return new List<FireDTO>();
        }
    }
}