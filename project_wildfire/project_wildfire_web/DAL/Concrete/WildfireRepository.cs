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
    }
}