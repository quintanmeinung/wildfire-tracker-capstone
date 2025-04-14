using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using project_wildfire_web.DAL.Abstract;

public class ManageAccountModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IUserRepository _userRepository;

    public ManageAccountModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userRepository = userRepository;
    }

    [BindProperty]
    public string FullName { get; set; }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string PhoneNumber { get; set; }

    [BindProperty]
    public string Password { get; set; }

    [BindProperty]
    public string Address { get; set; }

    [BindProperty]
    public string FirstName { get; set; }

    [BindProperty]
    public string LastName { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var authUser = await _userManager.GetUserAsync(User);
        if (authUser == null)
        {
            return NotFound("Unable to retrieve identity user by ID.");
        }

        var primaryUser = await _userRepository.GetUserByIdAsync(authUser.Id);
        if (primaryUser == null)
        {
            return NotFound("Unable to retrieve primary user by ID.");
        }

        FirstName = primaryUser.FirstName;
        LastName = primaryUser.LastName;
        Email = authUser.Email;
        PhoneNumber = authUser.PhoneNumber;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound("Unable to load user.");
        }

        // Update email if changed
        if (user.Email != Email)
        {
            var setEmailResult = await _userManager.SetEmailAsync(user, Email);
            if (!setEmailResult.Succeeded)
            {
                foreach (var error in setEmailResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
        }

        // Update phone number if changed
        if (user.PhoneNumber != PhoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                foreach (var error in setPhoneResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }
        }

        // Update first name and last name in the primary user record
        var primaryUser = await _userRepository.GetUserByIdAsync(user.Id);
        if (primaryUser != null)
        {
            primaryUser.FirstName = FirstName;
            primaryUser.LastName = LastName;
            _userRepository.UpdateUser(primaryUser);
        }

        await _signInManager.RefreshSignInAsync(user);
        TempData["StatusMessage"] = "Your profile has been updated.";
        return RedirectToPage();
    }
}