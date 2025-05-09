using project_wildfire_web.DAL;
using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.Models;

namespace project_wildfire_web.DAL.Concrete;

public class UserFireSubsRepository : IUserFireSubRepository
{
    private readonly FireDataDbContext _context;
    private readonly IWildfireRepository _wildfireRepo;

    public  UserFireSubsRepository(FireDataDbContext context, IWildfireRepository wildfireRepo)
    {
        _context = context;
        _wildfireRepo = wildfireRepo;
    }

    public async Task<IEnumerable<Fire>> GetFiresAsync (string userID)
    { 
        return await _context.UserFireSubscriptions
        .Where(s => s.UserId == userID)
        .Include(s => s.Fire)
        .Select(s => s.Fire)
        .ToListAsync();

    }
}