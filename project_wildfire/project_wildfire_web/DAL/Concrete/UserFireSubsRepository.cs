using project_wildfire_web.DAL;
using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.Models;
using Microsoft.EntityFrameworkCore;

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
    
    
    public async Task SubscribeAsync(string userID, int fireId)
        {
            if (!await IsSubscribedAsync(userID, fireId))
            {
                 var user = await _context.Users.FindAsync(userID);
                 var fire = await _context.Fires.FindAsync(fireId);
                var subscription = new UserFireSubscription
                {
                    UserId = userID,
                    FireId = fireId,
                    User = user,
                    Fire = fire
                };
    
                _context.UserFireSubscriptions.Add(subscription);
                await _context.SaveChangesAsync();

            }
        }

    public async Task<bool> IsSubscribedAsync(string userID, int FireID)
    {
        return await _context.UserFireSubscriptions
            .AnyAsync(s => s.UserId == userID && s.FireId == FireID);
    }
    public async Task UnsubscribeAsync(string userID, int FireID)
    {
        var subscription = await _context.UserFireSubscriptions
            .FirstOrDefaultAsync(s => s.UserId == userID && s.FireId == FireID);

        if (subscription != null)
        {
            _context.UserFireSubscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
        }
    }


    public async Task<IEnumerable<Fire>> GetFiresSubsAsync(string userID)
    {
        return await _context.UserFireSubscriptions
            .Where(s => s.UserId == userID)
            .Include(s => s.Fire)
            .Select(s => s.Fire)
            .ToListAsync();
    }
    

    
}