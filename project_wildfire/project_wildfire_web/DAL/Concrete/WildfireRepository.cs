using Microsoft.EntityFrameworkCore;
using project_wildfire_web.Models;
using project_wildfire_web.DAL.Abstract;

namespace project_wildfire_web.DAL.Concrete
{
    public class WildfireRepository : IWildfireRepository
    {
        private readonly FireDataDbContext _context;

        public WildfireRepository(FireDataDbContext context)
        {
            _context = context;
        }
        public ICollection<Fire> GetWildfires()
        {
            return _context.Fires.ToList();
        }

        public async Task AddWildfiresAsync(List<Fire> wildfires)
        {
            await _context.Fires.AddRangeAsync(wildfires);
            await _context.SaveChangesAsync();
        }

        public async Task ClearWildfiresAsync()
        {
            _context.Fires.RemoveRange(_context.Fires); //this should remove all Fire records from the context db
            await _context.SaveChangesAsync();  // commit the changes to the db 
            await _context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Fires', RESEED, 0)");
        }
    }
}