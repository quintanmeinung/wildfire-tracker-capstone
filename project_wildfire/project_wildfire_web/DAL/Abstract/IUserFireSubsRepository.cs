using project_wildfire_web.DAL.Concrete;
using project_wildfire_web.Models;

namespace project_wildfire_web.DAL.Abstract;

public interface IUserFireSubRepository
{
    //Method to get subscribed fires
    Task<IEnumerable<Fire>> GetFiresSubsAsync(string userID);
    Task<bool> IsSubscribedAsync(string userID, int FireId);
    Task SubscribeAsync(string userID,int FireId);
    Task UnsubscribeAsync(string userID,int FireId);
}