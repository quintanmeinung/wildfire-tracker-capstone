using System.Collections.Generic;
using System.Linq;
using project_wildfire_web.Models;

namespace project_wildfire_web.Services
{
    public class AqiStationService
    {
        private readonly FireDataDbContext _context;

        public AqiStationService(FireDataDbContext context)
        {
            _context = context;
        }

        public List<AqiStation> GetAll()
        {
            return _context.AqiStations.ToList();
        }
    }
}
