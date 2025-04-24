using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using project_wildfire_web.Models;

namespace project_wildfire_web.Controllers
{
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
            return View(locations);
        }

        [HttpPost]
        public async Task<IActionResult> PostUserLocation(UserLocation location)
        {
            // Automatically use the logged-in user's ID
            var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User is not logged in.");
            }
            location.Latitude = Math.Round(location.Latitude, 6);
            location.Longitude = Math.Round(location.Longitude, 6);

            // Set the UserId and add the location
            location.UserId = userId;
            _context.UserLocations.Add(location);
            await _context.SaveChangesAsync();

            return Redirect("/Identity/Account/Manage/SavedLocations");
        }




        [HttpPost("delete/{id}")]
        public async Task<IActionResult> DeleteUserLocation(int id)
        {
            var location = await _context.UserLocations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _context.UserLocations.Remove(location);
            await _context.SaveChangesAsync();

            // Redirect back to Razor page after deletion
            return Redirect("~/Identity/Account/Manage/SavedLocations");
        }

    }
}
