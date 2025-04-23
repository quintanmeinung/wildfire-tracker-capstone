using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using project_wildfire_web.Areas.Identity.Data;
using Microsoft.Extensions.Logging;

namespace project_wildfire_web.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        public LogoutModel(SignInManager<IdentityUser> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();

            // 🧹 Clear session data
            HttpContext.Session.Clear();

            _logger.LogInformation("User logged out and session cleared.");
            return LocalRedirect(returnUrl ?? Url.Content("~/"));
        }
    }
}
