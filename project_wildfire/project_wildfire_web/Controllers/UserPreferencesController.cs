using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using project_wildfire_web.Services;
using project_wildfire_web.Models;

[Route("api/user/preferences")]
[ApiController]
[Authorize] // Ensure only logged-in users can access this
public class UserPreferencesController : ControllerBase
{
    private readonly UserPreferencesService _preferencesService;
    private readonly ILogger<UserPreferencesController> _logger;

    public UserPreferencesController(UserPreferencesService preferencesService, ILogger<UserPreferencesController> logger)
    {
        _preferencesService = preferencesService;
        _logger = logger;
    }

    // GET: api/user/preferences
    [HttpGet]
    public async Task<IActionResult> GetPreferences()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get logged-in user ID
        if (userId == null) return Unauthorized();

        var preferences = await _preferencesService.GetPreferences(userId);
        return Ok(preferences);
    }

    [HttpPost]
    public async Task<IActionResult> SavePreferences([FromBody] UserPreferences preferences)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogError("❌ User ID not found. Unauthorized request.");
                return Unauthorized("User ID not found.");
            }

            _logger.LogInformation($"🔄 Saving preferences for user: {userId}");
            _logger.LogInformation($"📝 Received Preferences - FontSize: {preferences.FontSize}, ContrastMode: {preferences.ContrastMode}, TextToSpeech: {preferences.TextToSpeech}");

            var existingPreferences = await _preferencesService.GetPreferences(userId);

            if (existingPreferences == null)
            {
                _logger.LogInformation("🆕 No existing preferences found. Inserting new preferences...");
                var newPreferences = new UserPreferences
                {
                    UserId = userId,
                    FontSize = preferences.FontSize,
                    ContrastMode = preferences.ContrastMode,
                    TextToSpeech = preferences.TextToSpeech
                };

                await _preferencesService.SavePreferences(newPreferences);
            }
            else
            {
                _logger.LogInformation("🔄 Updating existing preferences...");
                existingPreferences.FontSize = preferences.FontSize;
                existingPreferences.ContrastMode = preferences.ContrastMode;
                existingPreferences.TextToSpeech = preferences.TextToSpeech;
                await _preferencesService.SavePreferences(existingPreferences);
            }

            return Ok(new { message = "Preferences saved successfully!" });
        }
        catch (Exception ex)
        {
            _logger.LogError($"❌ Exception in SavePreferences: {ex}");
            return StatusCode(500, $"Internal Server Error: {ex.Message}");
        }
    }

}

