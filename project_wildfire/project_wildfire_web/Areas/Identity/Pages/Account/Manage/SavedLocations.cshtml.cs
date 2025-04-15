using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project_wildfire_web.Models;
using System.Security.Claims;

public class SavedLocationModel : PageModel
{
    private readonly FireDataDbContext _context;

    public SavedLocationModel(FireDataDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public UserLocation NewLocation { get; set; }

    public IList<UserLocation> UserLocations { get; set; } = new List<UserLocation>();

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return RedirectToPage("/Account/Login");
        }

        UserLocations = await _context.UserLocations
            .Where(u => u.UserId == userId)
            .Include(u => u.User) 
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostAddAsync()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null || !ModelState.IsValid)
        {
            return RedirectToPage("/Account/Login");
        }

        NewLocation.UserId = userId;
        _context.UserLocations.Add(NewLocation);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int locationId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return RedirectToPage("/Account/Login");
        }

        var location = await _context.UserLocations
            .FirstOrDefaultAsync(l => l.UserId == userId && l.Id == locationId); // Make sure `Id` exists in your model

        if (location != null)
        {
            _context.UserLocations.Remove(location);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage();
    }
}
