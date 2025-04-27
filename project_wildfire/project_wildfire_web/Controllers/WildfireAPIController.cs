using Microsoft.AspNetCore.Mvc;
using project_wildfire_web.Models;
using project_wildfire_web.Models.DTO;
using project_wildfire_web.DAL.Abstract;
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
            if (wildfires == null)
            {
                _logger.LogError("NASA Service returned null.");
                return StatusCode(500, "NASA Service is unavailable or returned no data.");
            }
            return Ok(wildfires);
        }

        //Code to fetch Wildfire Markers by given date from user
        //Updated by Quintan
        [HttpGet("fetchWildfiresByDate")]
        public async Task<IActionResult> GetWildfiresByDate([FromQuery] string date)
        {
            if (!DateTime.TryParse(date, out var parsedDate))
                return BadRequest("Invalid date format");

            var fires = await _nasaService.GetFiresByDateAsync(parsedDate);
            return Ok(fires);
        }


        [HttpPost ("postData")]
        public async Task<IActionResult> SaveDataToDB()
        {
            _logger.LogInformation("Post FireDTO to DB");

            //Clear fires from db
            await _wildfireRepository.ClearWildfiresAsync();

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
        /////Functiont to pull fire fatabase


        

        }



    




    
