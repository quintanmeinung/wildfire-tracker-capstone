using project_wildfire_web.Models;
using project_wildfire_web.Models.DTO;

namespace project_wildfire_web.DAL.Abstract
{
    public interface ILocationRepository : IRepository<UserLocation>
    {
        Task<ICollection<UserLocation>> GetUserLocationsAsync(string userId);
        UserLocation GetLocationById(int locationId);
        Task AddLocationAsync(UserLocation location);
        Task DeleteLocationAsync(string locationId);
        Task UpdateLocationAsync(UserLocation location);
        void Save();

    }
}