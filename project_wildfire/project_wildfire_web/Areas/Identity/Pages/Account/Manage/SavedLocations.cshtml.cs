using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project_wildfire_web.Models;

public class SavedLocationModel : PageModel
{
    private readonly FireDataDbContext _context;

    public SavedLocationModel(FireDataDbContext context)
    {
        _context = context;
    }

    public IList<UserLocation> UserLocations { get; set; } = new List<UserLocation>();

    public async Task OnGetAsync()
    {
        // Optional: If you have Identity set up and want to show only this user's saved locations,
        // uncomment the following lines:
        //
        // var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // if (userId != null)
        // {
        //     UserLocations = await _context.UserLocations
        //         .Where(u => u.UserId == userId)
        //         .Include(u => u.User) // only if needed
        //         .ToListAsync();
        //     return;
        // }

        // Otherwise, show all saved locations
        UserLocations = await _context.UserLocations
            .Include(u => u.User) // optional
            .ToListAsync();
    }
}
