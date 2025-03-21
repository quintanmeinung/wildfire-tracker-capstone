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
        //This code is commented out to work with my test case, ignore for now until PK is created for UserPreferences Table
        /*
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
        }*/

        //Test Function for Saving Preferences with specific parameter
        //Delete when no longer needed
        public async Task SavePreferences(string userId, string fontSize, bool contrastMode, bool textToSpeech)
        {
            var preferences = await _context.UserPreferences.FindAsync(userId);

            if (preferences == null)
            {
                preferences = new UserPreferences
                {
                    UserId = userId,
                    FontSize = fontSize,
                    ContrastMode = contrastMode,
                    TextToSpeech = textToSpeech
                };
                _context.UserPreferences.Add(preferences);
            }
            else
            {
                preferences.FontSize = fontSize;
                preferences.ContrastMode = contrastMode;
                preferences.TextToSpeech = textToSpeech;
            }

            await _context.SaveChangesAsync();
        }
    }
}

