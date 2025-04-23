using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using project_wildfire_web.Models;
using project_wildfire_web.DAL.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using project_wildfire_web.Models.DTO;


namespace project_wildfire_web.Controllers;

[ApiController]
[Route("api/User")]
public class UserApiController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<WildfireAPIController> _logger;
    private readonly UserManager<IdentityUser> _userManager;

    public UserApiController(
        ILogger<WildfireAPIController> logger, 
        IUserRepository userRepository,
        UserManager<IdentityUser> userManager
        )
    {
        _userRepository = userRepository;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> SaveModalEdits([FromBody]ProfileViewModel profileViewModel)
    {
        // Find auth and primary user records for the given ID
        var user = await _userRepository.GetUserByIdAsync(profileViewModel.Id);
        var authUser = await _userManager.FindByIdAsync(profileViewModel.Id);

        // Null check
        if (user == null || authUser == null)
        {
            return NotFound("User record not found.");
        }

        // Update primary user record
        user.FirstName = profileViewModel.FirstName;
        user.LastName = profileViewModel.LastName;

        //update custom preferences record
        user.FontSize = profileViewModel.FontSize ?? "medium"; //Added the .FontSize class Attribute 
        user.ContrastMode = profileViewModel.ContrastMode;
        user.TextToSpeech = profileViewModel.TextToSpeech;

        // Update auth user record
        authUser.Email = profileViewModel.Email;
        authUser.PhoneNumber = profileViewModel.PhoneNumber;

        // Save changes
        _userRepository.UpdateUser(user);
        await _userManager.UpdateAsync(authUser);

        // Return updated user
        return Ok();
    }

    [HttpPost("UpdateEmail")]
    public async Task<IActionResult> UpdateEmail([FromBody] string newEmail)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var setEmailResult = await _userManager.SetEmailAsync(user, newEmail);
        if (!setEmailResult.Succeeded)
        {
            return BadRequest(setEmailResult.Errors);
        }

        var setUserNameResult = await _userManager.SetUserNameAsync(user, newEmail);
        if (!setUserNameResult.Succeeded)
        {
            return BadRequest(setUserNameResult.Errors);
        }

        return Ok("Email updated successfully.");
    }

    //Saves Preferences 
    [HttpPost("preferences")]
    public async Task<IActionResult> SavePreferences([FromBody] PreferencesDTO dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _logger.LogInformation($"🔍 Incoming savePreferences call for user: {userId}");
        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("⚠️ No user ID found in claims.");
            return Unauthorized("User not authenticated");
        }
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
        {
            _logger.LogWarning($"⚠️ No matching user found in DB for userId: {userId}");
            return NotFound("User not found");
        }

        _logger.LogInformation($"💾 Saving preferences: FontSize={dto.FontSize}, ContrastMode={dto.ContrastMode}, TextToSpeech={dto.TextToSpeech}");
        user.FontSize = dto.FontSize ?? "medium";
        user.ContrastMode = dto.ContrastMode;
        user.TextToSpeech = dto.TextToSpeech;

        _userRepository.UpdateUser(user);
        _logger.LogInformation("✅ Preferences updated and saved to DB");
        return Ok();
    }

}





