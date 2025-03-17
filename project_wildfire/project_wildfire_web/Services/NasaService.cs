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
    public async Task<List<FireDTO>> GetFiresAsync()
    {
       
        _logger.LogInformation("Calling NASA Firms API");

        string apiKey = _configuration["NASA:FirmsApiKey"];
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError("NASA FIRMS API key missing");
            return new List<FireDTO>(); // Return empty list instead of null
        }

        string endpoint = $"https://firms.modaps.eosdis.nasa.gov/api/area/csv/{apiKey}/VIIRS_SNPP_NRT/-130,40,-110,50/1/2025-03-02";

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




}