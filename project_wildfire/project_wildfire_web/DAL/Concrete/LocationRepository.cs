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
        private readonly FireDataDbContext _context;

        public LocationRepository(FireDataDbContext context) : base(context)
        {
            _context = context;
        }

        public ICollection<UserLocation> GetUserLocations(string userId)
        {
            var locations = _context.UserLocations
                .Include(ul => ul.User)
                .Where(ul => ul.UserId == userId)
                .ToList();

            return locations;
        }

        public UserLocation GetLocationById(int locationId)
        {
            throw new NotImplementedException();
        }
        
        public Task AddLocationAsync(UserLocation location)
        {
            if (location == null)
            {
                throw new ArgumentNullException(nameof(location), "UserLocation cannot be null.");
            }

            _context.UserLocations.Add(location);
            return _context.SaveChangesAsync();
        }

        public Task DeleteLocationAsync(int locationId, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateLocationAsync(UserLocation location)
        {
            ArgumentNullException.ThrowIfNull(location);

            try
            {
                var existingEntity = await _context.UserLocations
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == location.Id);
                
                if (existingEntity == null)
                {
                    throw new InvalidOperationException($"UserLocation with ID {location.Id} not found");
                }

                _context.UserLocations.Update(location);
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(
                    "Concurrency conflict occurred while updating location", ex);
            }
            
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(
                    "Database error occurred while updating location", ex);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }

}