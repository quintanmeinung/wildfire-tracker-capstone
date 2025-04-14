using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using project_wildfire_web.DAL.Abstract;

public class ManageAccountModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserRepository _userRepository;

    public ManageAccountModel(UserManager<IdentityUser> userManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
    }

    [BindProperty]
    public string FullName { get; set; }

    [BindProperty]
    public string Email { get; set; }

    [BindProperty]
    public string PhoneNumber { get; set; }

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
}