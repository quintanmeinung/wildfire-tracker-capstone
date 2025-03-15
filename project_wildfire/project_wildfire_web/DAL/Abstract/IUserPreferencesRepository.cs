using System;
using System.Collections.Generic;
using project_wildfire_web.Models;

namespace project_wildfire_web.DAL.Abstract
{
    public interface IUserPreferencesRepository
    {
        Task<UserPreferences> GetUserPreferencesAsync(string userId);
        Task AddUserPreferenceAsync(UserPreferences preferences);
        Task UpdateUserPreference(UserPreferences preferences);
        Task SaveUserPreference();
        Task DeleteUserPreference(string userId);
    }
}