using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using project_wildfire_web.Models;
using project_wildfire_web.DAL.Abstract;
using CsvHelper;
using CsvHelper.Configuration;

namespace project_wildfire_web.Controllers;

    [ApiController]
    [Route("api/WildfireAPIController")]
    
    public class WildfireAPIController : ControllerBase
    {
        private readonly IWildfireRepository _wildfireRepository;
        private readonly ILogger<WildfireAPIController> _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;



        public WildfireAPIController(IConfiguration configuration, HttpClient httpClient, ILogger<WildfireAPIController> logger, IWildfireRepository wildfireRepository)
        {
            _httpClient = httpClient;
            _wildfireRepository = wildfireRepository;
            _configuration = configuration;
            _logger = logger;

        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FireDatum>))]

        public IActionResult GetWildfires()
        {
            var wildfires = _wildfireRepository.GetWildfires();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(wildfires);
        }

        [HttpGet("fetch")]
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

            var wildfireData = csv.GetRecords<FireDatumDTO>().ToList();
            return Ok(wildfireData);       

        }



    }




    
