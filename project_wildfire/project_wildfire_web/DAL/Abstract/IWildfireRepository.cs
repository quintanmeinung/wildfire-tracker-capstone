using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.Models;

namespace project_wildfire_web.DAL.Abstract
{
    public interface IWildfireRepository
    {
        ICollection<Fire> GetWildfires();

        Task AddWildfiresAsync(List<Fire> wildfires);

    }
}