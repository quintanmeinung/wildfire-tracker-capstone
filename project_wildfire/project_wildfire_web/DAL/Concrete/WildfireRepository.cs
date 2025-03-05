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
    }
}