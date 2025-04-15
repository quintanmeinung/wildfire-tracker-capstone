using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_wildfire_web.Models;

namespace project_wildfire_web.Controllers
{
    // Change to a normal MVC controller with route for Razor Pages
    [Route("Identity/Account/Manage/[controller]")]
    public class UserLocationController : Controller
    {
        private readonly FireDataDbContext _context;

        public UserLocationController(FireDataDbContext context)
        {
            _context = context;
        }

        // GET: Identity/Account/Manage/UserLocation/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserLocations(string userId)
        {
            // Fetch the saved locations for a specific user
            var locations = await _context.UserLocations.Where(ul => ul.UserId == userId).ToListAsync();
            return View(locations); // Return a Razor view with the locations
        }

        // POST: Identity/Account/Manage/UserLocation
[HttpPost]
public async Task<IActionResult> PostUserLocation(UserLocation location)
{
    // Automatically use the logged-in user's ID
    var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

    if (string.IsNullOrEmpty(userId))
    {
        return BadRequest("User is not logged in.");
    }

    // Set the UserId and add the location
    location.UserId = userId;
    _context.UserLocations.Add(location);
    await _context.SaveChangesAsync();

    return RedirectToAction("SavedLocations", "UserLocation");
}



        // DELETE: Identity/Account/Manage/UserLocation/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserLocation(int id)
        {
            var location = await _context.UserLocations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _context.UserLocations.Remove(location);
            await _context.SaveChangesAsync();

            // After deletion, redirect back to the saved locations page
            return RedirectToAction("GetUserLocations", new { userId = location.UserId });
        }
    }
}
