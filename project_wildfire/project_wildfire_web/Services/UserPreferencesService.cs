using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using project_wildfire_web.Models;

namespace project_wildfire_web.Services
{
    public class UserPreferencesService
    {
        private readonly FireDataDbContext _context;

        public UserPreferencesService(FireDataDbContext context)
        {
            _context = context;
        }

        // Get user preferences
        public async Task<UserPreferences> GetPreferences(string userId)
        {
            return await _context.UserPreferences
                .FirstOrDefaultAsync(up => up.UserId == userId);
        }

        // Save or update preferences
        public async Task SavePreferences(UserPreferences preferences)
        {
            var existingPreferences = await _context.UserPreferences.FindAsync(preferences.UserId);

            if (existingPreferences == null)
            {
                _context.UserPreferences.Add(preferences);
            }
            else
            {
                existingPreferences.FontSize = preferences.FontSize;
                existingPreferences.ContrastMode = preferences.ContrastMode;
                existingPreferences.TextToSpeech = preferences.TextToSpeech;
            }

            await _context.SaveChangesAsync();
        }
    }
}

