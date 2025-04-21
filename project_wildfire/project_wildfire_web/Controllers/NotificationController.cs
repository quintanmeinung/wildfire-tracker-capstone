using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

[Route("[controller]/[action]")]
public class NotificationController : Controller
{
    private readonly NotificationService _notificationService;
    private readonly UserManager<IdentityUser> _userManager;

    public NotificationController(NotificationService notificationService, UserManager<IdentityUser> userManager)
    {
        _notificationService = notificationService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> CheckFireProximity()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized("User not logged in.");
        }

        // Get the user and their phone number
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        var phoneNumber = user.PhoneNumber;
        if (string.IsNullOrEmpty(phoneNumber))
        {
            return BadRequest("Phone number is missing.");
        }

        await _notificationService.CheckFiresNearUserLocationsAsync(userId, phoneNumber);

        return Content("✅ Fire proximity check completed. Check the console logs.");
    }
}
