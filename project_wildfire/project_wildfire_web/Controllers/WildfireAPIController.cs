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



        public WildfireAPIController(
            ILogger<WildfireAPIController> logger, 
            INasaService nasaService
            )
        {
            _logger = logger;
            _nasaService = nasaService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Fire>))]

        public async Task<IActionResult> GetWildfiresAsync()
        {
            List<Fire> wildfires = await _nasaService.GetFiresAsync();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(wildfires);
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

        /* [HttpPost("save")]
        public async Task<IActionResult> SaveWildfireData()
        {
            try
            {
                var response = await FetchWildfires();

                if (response is OkObjectResult okResult)
                {
                    var wildfireDTOs = okResult.Value as List<FireDatumDTO>;
                    if (wildfireDTOs == null || !wildfireDTOs.Any())
                    {
                        _logger.LogWarning("No wildfire data fetched.");
                        return BadRequest("No wildfire data received.");
                    }

                     var wildfires = wildfireDTOs.Select(dto => new FireDatum
                        {
                            Location = new Point(dto.Longitude, dto.Latitude) { SRID = 4326 }, // 
                            RadiativePower = dto.RadiativePower
                        }).ToList();
                    
                    await _wildfireRepository.AddWildfiresAsync(wildfires);

                    return CreatedAtAction(nameof(GetWildfires), wildfires);
                }
                else
                {
                    {
                        _logger.LogError("Failed to fetch wildfire data from NASA.");
                        return StatusCode(500, "Failed to fetch wildfire data.");
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error saving wildfire data: {ex.Message}");
                return StatusCode(500, "Internal server error.");
            }
            
        } */





    }




    
