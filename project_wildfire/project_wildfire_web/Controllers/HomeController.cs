using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using project_wildfire_web.Areas.Identity.Data;
using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.ExtensionsMethods;
using project_wildfire_web.Models;
using project_wildfire_web.Models.DTO;

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

            var profileViewModel = new ProfileViewModel
            {
                Id = primaryUser.UserId,
                FirstName = primaryUser.FirstName,
                LastName = primaryUser.LastName,
                Email = authUser.Email,
                PhoneNumber = authUser.PhoneNumber,
                SavedLocations = savedLocations,
                FireSubscriptions = primaryUser.Fires
            };

            // Setup location model for dynamic markers
            var userLocation = new UserLocation
            {
                UserId = primaryUser.UserId
                // Only the UserId is specified
            };

            // Generate IndexViewModel for Index
            var indexViewModel = new IndexViewModel(
                profileViewModel.ToProfileViewModelDTO(),
                userLocation.ToUserLocationDTO(),
                savedLocations.Select(ul => ul.ToUserLocationDTO()).ToList()
            );

            // Pass the model to the view
            _logger.LogDebug("User is authenticated, model sent to view: {IndexViewModel}", JsonSerializer.Serialize(indexViewModel));
            return View(indexViewModel);
        }
        // If user is not authenticated, no model is sent
        _logger.LogDebug("User is not authenticated, no model sent to view.");
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
