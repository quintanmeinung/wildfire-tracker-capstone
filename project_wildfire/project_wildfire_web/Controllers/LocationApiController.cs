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
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace project_wildfire_web.Controllers;

[ApiController]
[Route("api/Location")]
public class LocationApiController : ControllerBase
{
    private readonly ILogger<WildfireAPIController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly ILocationRepository _locationRepository;

    public LocationApiController(
        ILogger<WildfireAPIController> logger, 
        UserManager<IdentityUser> userManager,
        IUserRepository userRepository,
        ILocationRepository locationRepository
        )
    {
        _logger = logger;
        _userManager = userManager;
        _userRepository = userRepository;
        _locationRepository = locationRepository;
    }

    [HttpPost]
    [Route("api/Location/SaveLocation")]
    public async Task<IActionResult> SaveLocation([FromBody] UserLocationDTO userLocationDTO)
    {
        Console.WriteLine("Received request: {userLocationDTO}");
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for UserLocationDTO: {ModelState}", ModelState);
            return BadRequest(ModelState);
        }
        // POSTs from dynamic user markers will not have userId
        var authUser = await _userManager.GetUserAsync(User);

        // Add location to user's saved locations
        UserLocation userLocation = userLocationDTO.ToUserLocation();
        if (authUser != null)
        {
            userLocation.UserId = authUser.Id;
        }

        // Save changes
        try{
            await _locationRepository.AddLocationAsync(userLocation);

        } catch (Exception ex) {
            _logger.LogError(ex, "Error saving user location: {Message}", ex.Message);
            return StatusCode(500, "Internal server error while saving location.");
        }
        
        return Ok();
    }

}

    