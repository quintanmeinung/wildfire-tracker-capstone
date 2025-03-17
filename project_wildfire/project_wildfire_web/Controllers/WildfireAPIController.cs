using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using project_wildfire_web.Models;
using project_wildfire_web.Models.DTO;
using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.ExtensionsMethods;
using CsvHelper;
using CsvHelper.Configuration;
using NetTopologySuite.Geometries;
using project_wildfire_web.Services;


namespace project_wildfire_web.Controllers;

    [ApiController]
    [Route("api/WildfireAPIController")]
    
    public class WildfireAPIController : ControllerBase
    {
        private readonly ILogger<WildfireAPIController> _logger;
        private readonly INasaService _nasaService;
        private readonly IConfiguration _configuration;
        //private readonly HttpClient _httpClient;
        private readonly IWildfireRepository _wildfireRepository;





        public WildfireAPIController(IWildfireRepository wildfirefireRepository, ILogger<WildfireAPIController> logger, INasaService nasaService)
        {
            _logger = logger;
            _nasaService = nasaService;
            _wildfireRepository = wildfirefireRepository;
        }

        [HttpGet("fetchWildfires")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Fire>))]

        public async Task<IActionResult> GetWildfiresAsync()
        {
            List<FireDTO> wildfires = await _nasaService.GetFiresAsync();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(wildfires);
        }

        [HttpPost ("postData")]
        public async Task<IActionResult> SaveDataToDB()
        {
            _logger.LogInformation("Post FireDTO to DB");
            var fetchResult = await GetWildfiresAsync();

            if (fetchResult is ObjectResult result && result.Value is List<FireDTO> wildfiresDto)
            {
                if (!wildfiresDto.Any())
                {
                    return BadRequest("Invalid or empty data.");
                }

                var wildfires = wildfiresDto.Select(dto => new Fire
                {
                    Longitude = dto.Longitude,
                    Latitude = dto.Latitude,
                    RadiativePower = dto.RadiativePower
                }).ToList();

                await _wildfireRepository.AddWildfiresAsync(wildfires);
                return Ok(new { message = "Wildfire data successfully saved to the database." });
            }

            return StatusCode(500, "Failed to fetch wildfires from NASA API.");
        }

        }

        //Fetch Wildfires
        /* [HttpGet("fetch")]
        public async Task<IActionResult> FetchWildfires()
        {
            _logger.LogInformation("Calling NASA Firms API");

            string apiKey = _configuration["NASA:FirmsApiKey"];
            if(string.IsNullOrEmpty(apiKey))
            {
                _logger.LogError("NASA FIRMS API key missing");
                return StatusCode(500, "Internal Server Error: Missing API Key");
            }

            //PNW coordinates for fires
            string endpoint = $"https://firms.modaps.eosdis.nasa.gov/api/area/csv/{apiKey}/VIIRS_SNPP_NRT/-130,40,-110,50/1/2025-03-02";

            HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
            string responseBody;
            if (response.IsSuccessStatusCode)
            {
                responseBody = await response.Content.ReadAsStringAsync();
            }
            else
            {
                _logger.LogError($"Failed to fetch nasa data- {response.StatusCode}\n{response.Content}"); 
                return null;
            }
             // var response = await _httpClient.GetStreamAsync(apiUrl);
            using var reader = new StringReader(responseBody);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null
            };
            
            using var csv = new CsvReader(reader, csvConfig);

            var wildfires = csv.GetRecords<FireDTO>().ToList();
            
            //var wildfireDataDTO = csv.GetRecords<FireDatumDTO>().ToList();
            
            //var wildfires = wildfireDataDTO.Select(dto => dto.ToFireDatum()).ToList();

            return Ok(wildfires);

        } */

        //Fetch All NASA API Wildfires
             
        /* [HttpGet("fetchAll")]
         public async Task<string> GetFireDataCsvAsync()
           {
              /*   string apiKey = _configuration["NASA:FirmsApiKey"];
                string endpoint = $"https://firms.modaps.eosdis.nasa.gov/api/area/csv/{apiKey}/VIIRS_SNPP_NRT/-130,40,-110,50/1/2025-03-02";

               // string url = "https://firms.modaps.eosdis.nasa.gov/api/path-to-your-csv";
                var response = await _httpClient.GetAsync(endpoint);
                
                response.EnsureSuccessStatusCode(); // Throws an error if the response is not successful

                return await response.Content.ReadAsStringAsync(); 

                var wildfires = await _nasaService.GetFiresAsync();

                if (wildfires == null || !wildfires.Any())
                {
                    return BadRequest("No wildfires found.");
                }

                return Ok(wildfires);
            }
        

       */


    




    
