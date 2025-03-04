using System;
using System.Collections.Generic;
using project_wildfire_web.Models;

namespace project_wildfire_web.DAL.Abstract
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetUsers();
        Task<User> GetUserByIdAsync(string userId);
        Task AddUserAsync(User user);
        void DeleteUser(string userId);
        void UpdateUser(User user);
        void Save();
    }
}