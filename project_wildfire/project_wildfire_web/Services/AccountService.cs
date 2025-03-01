using Microsoft.Extensions.Logging;
using project_wildfire_web.Models;
using project_wildfire_web.Areas.Identity.Pages.Account;
using System;
using System.Threading.Tasks;

namespace project_wildfire_web.Services;

public class AccountService
{
    private readonly WildfireDbContext _wildfireDbContext;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        WildfireDbContext wildfireDbContext,
        ILogger<AccountService> logger)
    {
        _wildfireDbContext = wildfireDbContext;
        _logger = logger;
    }

    /// <summary>
    /// Generates a user record in the wildfire database.
    /// </summary>
    /// <param name="userId">The user's Identity GUID.</param>
    /// <param name="input">The user's registration form inputs.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task GeneratePrimaryUserAsync(string userId, RegisterModel.InputModel input)
    {
        try
        {
            // Validate input
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.");

            if (input == null)
                throw new ArgumentNullException(nameof(input), "Input model cannot be null.");

            // Create the user object
            var visitor = new User
            {
                UserId = userId,
                FirstName = input.FirstName,
                LastName = input.LastName,
            };

            // Add the user to the database
            _wildfireDbContext.Add(visitor);
            await _wildfireDbContext.SaveChangesAsync();

            _logger.LogInformation("User created successfully: {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while generating the primary user.");
            throw; // Re-throw the exception to propagate it
        }
    }

}