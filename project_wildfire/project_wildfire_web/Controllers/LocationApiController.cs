using Microsoft.AspNetCore.Mvc;
using project_wildfire_web.Models;
using project_wildfire_web.Models.DTO;
using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.ExtensionsMethods;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace project_wildfire_web.Controllers;

[ApiController]
[Route("api/Location")]
public class LocationApiController : ControllerBase
{
    private readonly ILogger<LocationApiController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;
    private readonly ILocationRepository _locationRepository;

    public LocationApiController(
        ILogger<LocationApiController> logger, 
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
        string userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("User ID is null or empty");
            return BadRequest(new { Error = "User ID cannot be null or empty" });
        }
        userLocation.UserId = userId;

        // Truncate lng & lat to 5 decimal places
        userLocation.Latitude = Math.Round(userLocation.Latitude, 5);
        userLocation.Longitude = Math.Round(userLocation.Longitude, 5);

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
        if (userLocationDTO == null)
        {
            _logger.LogWarning("Request body is null");
            return BadRequest(new { Error = "Request body cannot be null" });
        }

        _logger.LogDebug("Request received at api/Location/UpdateLocation(POST)");
        _logger.LogDebug("DTO received: {@UserLocationDTO}", userLocationDTO);

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            _logger.LogWarning("Validation errors: {@Errors}", errors);
            return BadRequest(new { Errors = errors });
        }

        // Validate user exists
        var user = await _userManager.FindByIdAsync(userLocationDTO.UserId);
        if (user == null)
        {
            _logger.LogWarning("User not found: {UserId}", userLocationDTO.UserId);
            return NotFound(new { Error = "User not found" });
        }

        UserLocation userLocation = userLocationDTO.ToUserLocation();

        try
        {
            await _locationRepository.UpdateLocationAsync(userLocation);
            _logger.LogInformation("Location updated for user {UserId}", userLocationDTO.UserId);
            return Ok(new { Success = true, Message = "Location updated", Data = userLocation });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error updating location");
            return StatusCode(500, new { Error = "Database error updating location" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating location");
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }

   [HttpDelete("DeleteLocation")]
    public async Task<IActionResult> DeleteLocation([FromBody] string locationId)
    {
        _logger.LogDebug("DeleteLocation request received for Location ID: {LocationId}", locationId);
        
        if (string.IsNullOrEmpty(locationId))
        {
            _logger.LogWarning("Invalid request - null or empty LocationId");
            return BadRequest("LocationId is required");
        }

        try
        {
            await _locationRepository.DeleteLocationAsync(locationId);
            return Ok(new { Success = true, Message = "Location deleted" });
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database error deleting location {LocationId}", locationId);
            return StatusCode(500, new { Error = "Database error deleting location" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error deleting location {LocationId}", locationId);
            return StatusCode(500, new { Error = "Internal server error" });
        }
    }
}

    