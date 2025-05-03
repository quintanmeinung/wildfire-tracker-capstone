using Microsoft.AspNetCore.Mvc;
using project_wildfire_web.Models;
using project_wildfire_web.Models.DTO;
using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.ExtensionsMethods;
using Microsoft.AspNetCore.Identity;


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

    [HttpPost("SaveLocation")]
    public async Task<IActionResult> SaveLocation([FromBody] UserLocationDTO userLocationDTO)
    {
        _logger.LogDebug("Request received at api/Location/SaveLocation(POST)");
        _logger.LogDebug("DTO received: {UserLocationDTO}", userLocationDTO);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for UserLocationDTO: {ModelState}", ModelState);
            return BadRequest(ModelState);
        }
        // Add location to user's saved locations
        UserLocation userLocation = userLocationDTO.ToUserLocation();

        try{
            await _locationRepository.AddLocationAsync(userLocation);

        } catch (Exception ex) {
            _logger.LogError(ex, "Error saving user location: {Message}", ex.Message);
            return StatusCode(500, "Internal server error while saving location.");
        }
        
        _logger.LogDebug("User location saved successfully");
        return Ok("Location saved successfully");
    }

    [HttpPost("UpdateLocation")]
    public async Task<IActionResult> UpdateLocation([FromBody] UserLocationDTO userLocationDTO)
    {
        _logger.LogDebug("Request received at api/Location/UpdateLocation(POST)");
        _logger.LogDebug("DTO received: {UserLocationDTO}", userLocationDTO);
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid model state for UserLocationDTO: {ModelState}", ModelState);
            return BadRequest(ModelState);
        }
        // Update location in user's saved locations
        UserLocation userLocation = userLocationDTO.ToUserLocation();

        try{
            await _locationRepository.UpdateLocationAsync(userLocation);

        } catch (Exception ex) {
            _logger.LogError(ex, "Error updating user location: {Message}", ex.Message);
            return StatusCode(500, "Internal server error while updating location.");
        }
        
        _logger.LogDebug("User location updated successfully");
        return Ok("Location updated successfully");
    }

}

    