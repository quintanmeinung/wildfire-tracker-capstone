using project_wildfire_web.Models;

namespace project_wildfire_web.DAL.Abstract
{
    public interface ILocationRepository : IRepository<SavedLocation>
    {
        ICollection<SavedLocation> GetUserLocations(string userId);
        SavedLocation GetLocationById(int locationId);
        Task AddLocationAsync(SavedLocation location);
        Task DeleteLocationAsync(int locationId, string userId);
        Task UpdateLocationAsync(SavedLocation location);
        void Save();

    }
}