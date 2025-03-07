using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using project_wildfire_web.Models;
using project_wildfire_web.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using project_wildfire_web.DAL.Concrete;
using Microsoft.AspNetCore.Identity;

namespace project_wildfire_web.DAL.Concrete
{
    public class LocationRepository : Repository<UserLocation>, ILocationRepository
    {
        private readonly FireDataDbContext context;

        public LocationRepository(FireDataDbContext context) : base(context)
        {
            this.context = context;
        }

        public ICollection<UserLocation> GetUserLocations(string userId)
        {
            /* var locations = context.SavedLocations
                .Where(ul => ul.UserId == userId)
                .Select(ul => ul.Location)
                .ToList(); */
            throw new NotImplementedException();
        }

        public UserLocation GetLocationById(int locationId)
        {
            throw new NotImplementedException();
        }
        
        public Task AddLocationAsync(UserLocation location)
        {
            // Save user-saved location with the user's ID
            throw new NotImplementedException();
        }

        public Task DeleteLocationAsync(int locationId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLocationAsync(UserLocation location)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }

}