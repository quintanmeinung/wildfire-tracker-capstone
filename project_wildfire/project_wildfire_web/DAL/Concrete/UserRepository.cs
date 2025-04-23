using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using project_wildfire_web.Models;
using project_wildfire_web.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using project_wildfire_web.DAL.Concrete;

namespace project_wildfire_web.DAL.Concrete
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly FireDataDbContext context;

        public UserRepository(FireDataDbContext context) : base(context)
        {
            this.context = context;
        }

        public IEnumerable<User> GetUsers()
        {
            return context.Users.ToList();
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            return user;
        }

        public async Task AddUserAsync(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public void DeleteUser(string userId)
        {
            User? user = context.Users.SingleOrDefault(u => u.UserId == userId);
            if (user == null)
            {   
                Console.WriteLine("UserRepository.DeleteUser: User not found");
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }
            context.Users.Remove(user);
        }

        public void UpdateUser(User user)
        {
            context.Entry(user).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Save()
        {
            context.SaveChanges();
        }

    }
}