using project_wildfire_web.Models;

namespace project_wildfire_web.DAL.Abstract
{
    public interface ILocationRepository : IRepository<UserLocation>
    {
        ICollection<UserLocation> GetUserLocations(string userId);
        UserLocation GetLocationById(int locationId);
        Task AddLocationAsync(UserLocation location);
        Task DeleteLocationAsync(int locationId, string userId);
        Task UpdateLocationAsync(UserLocation location);
        void Save();

    }
}