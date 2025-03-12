using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using project_wildfire_web;
using project_wildfire_web.Models;
using project_wildfire_web.DAL.Abstract;

public class UserPreferencesRepository : IUserPreferencesRepository
{
    private readonly FireDataDbContext _context;

    public UserPreferencesRepository(FireDataDbContext context)
    {
        _context = context;
    }

<<<<<<< HEAD
    // ✅ Add new user preferences
    public async Task AddUserPreferenceAsync(UserPreferences preferences)
    {
        await _context.UserPreferences.AddAsync(preferences);
        await _context.SaveChangesAsync();
    }

    // ✅ Retrieve user preferences
    public async Task<UserPreferences> GetUserPreferencesAsync(string userId)
    {
        return await _context.UserPreferences
            .FirstOrDefaultAsync(up => up.UserId == userId);
    }

    // ✅ Update user preferences
    public async Task UpdateUserPreference(UserPreferences preferences)
    {
        _context.UserPreferences.Update(preferences);
        await _context.SaveChangesAsync();
    }

    // ✅ Save database changes
    public async Task SaveUserPreference()
    {
        await _context.SaveChangesAsync();
    }

    // ✅ Optional: Delete user preferences
    public async Task DeleteUserPreference(string userId)
    {
        var preferences = await _context.UserPreferences.FirstOrDefaultAsync(up => up.UserId == userId);
        if (preferences != null)
        {
            _context.UserPreferences.Remove(preferences);
            await _context.SaveChangesAsync();
        }
    }
}

=======
} */
>>>>>>> 67b689b388a552f38b16d056b249c81201829597
