using System.Diagnostics;
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

    public IActionResult Index()
    {
        // ProfileViewModel populates the partial profile modal
        var profileViewModel = new ProfileViewModel
        {
            Email = string.Empty,
            SavedLocations = [],
            FireSubscriptions = []
        };
        
        // If the user is authenticated, populate the model with their data
        if (User.Identity.IsAuthenticated)
        {
            // Get the current user
            var user = _userManager.GetUserAsync(User).Result;
            var primaryUser = _userRepository.GetUserByIdAsync(user.Id.ToString()).Result;
            
            if (user != null)
            {
                // Populate the model with user data
                profileViewModel.FirstName = primaryUser.FirstName;
                profileViewModel.LastName = primaryUser.LastName;
                profileViewModel.Email = user.Email;
                profileViewModel.PhoneNumber = user.PhoneNumber;
                
                // profileViewModel.SavedLocations = _locationRepository.GetUserLocations(user.Id);
                profileViewModel.SavedLocations = [];
                profileViewModel.FireSubscriptions = [];
            }
        }
        
        // Pass the model to the view
        return View(profileViewModel);
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
