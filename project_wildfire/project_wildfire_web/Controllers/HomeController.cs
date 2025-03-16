using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using project_wildfire_web.Areas.Identity.Data;
using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.Models;

namespace project_wildfire_web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILocationRepository _locationRepository;
    private readonly IUserRepository _userRepository;

    public HomeController(
        ILogger<HomeController> logger,
        UserManager<IdentityUser> userManager,
        ILocationRepository repository,
        IUserRepository userRepository)
    {
        _logger = logger;
        _userManager = userManager;
        _locationRepository = repository;
        _userRepository = userRepository;
    }

    public async Task<IActionResult> Index()
    {
        // If the user is authenticated, populate the model with their data
        if (User?.Identity?.IsAuthenticated == true)
        {
            // Find the identity user record
            var authUser = await _userManager.GetUserAsync(User);
            if (authUser == null)
            {
                return NotFound("Unable to retreive identity user by ID.");
            }
            // Find the primary user record
            var primaryUser = await _userRepository.GetUserByIdAsync(authUser.Id);
            if (primaryUser == null)
            {
                return NotFound("Unable to retreive primary user by ID.");
            }

            var savedLocations = _locationRepository.GetUserLocations(primaryUser.UserId);

            // Generate view model for the profile page
            var profileViewModel = new ProfileViewModel
            {
                Id = primaryUser.UserId,
                FirstName = primaryUser.FirstName,
                LastName = primaryUser.LastName,
                Email = authUser.Email,
                PhoneNumber = authUser.PhoneNumber,
                // SavedLocations needs to be populated 
                FireSubscriptions = primaryUser.Fires
            };
            Console.WriteLine("ProfileViewModel: " + profileViewModel);

            // Pass the model to the view
            return View(profileViewModel);
        }
        // If user is not authenticated, no model is sent
        return View();
        
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Resources()
    {
        return View();
    }

}
