using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using project_wildfire_web.Areas.Identity.Data;
using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.Models;

namespace project_wildfire_web.Controllers;

public class AccountController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILocationRepository _locationRepository;
    private readonly IUserRepository _userRepository;

    public AccountController(
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

   /*  public async Task<IActionResult> Login(LoginViewModel model)
    {
        if(ModelState.IsValid)
        {
          var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,lockoutOnFailure: false);

        }
    }
 */
}