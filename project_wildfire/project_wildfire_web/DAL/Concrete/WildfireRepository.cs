using project_wildfire_web.Models;

namespace project_wildfire_web.DAL.Concrete
{
    public class WildfireRepository : IWildfireRepository
    {
        private readonly WildfireDbContext _context;

        public WildfireRepository(WildfireDbContext context)
        {
            _context = context;
        }
        public ICollection<FireDatum> GetWildfires()
        {
            return _context.FireData.ToList();
        }

        public async Task AddWildfiresAsync(List<FireDatum> wildfires)
        {
            await _context.FireData.AddRangeAsync(wildfires);
            await _context.SaveChangesAsync();
        }
    }
}