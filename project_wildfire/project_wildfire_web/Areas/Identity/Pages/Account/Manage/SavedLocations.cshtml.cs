using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project_wildfire_web.Models;
using System.Security.Claims;

public class SavedLocationsModel : PageModel
{
    private readonly FireDataDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public SavedLocationsModel(FireDataDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [BindProperty]
    public SavedLocation? NewLocation { get; set; }

    public List<SavedLocation>? UserLocations { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var userId = _userManager.GetUserId(User);
        UserLocations = await _context.SavedLocations
                                      .Where(l => l.UserId == userId)
                                      .ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var userId = _userManager.GetUserId(User);
        NewLocation.UserId = userId;

        _context.SavedLocations.Add(NewLocation);
        await _context.SaveChangesAsync();

        return RedirectToPage();
    }
}
