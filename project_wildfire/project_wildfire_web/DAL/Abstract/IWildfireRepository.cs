using project_wildfire_web.DAL.Abstract;
using project_wildfire_web.Models;

namespace project_wildfire_web.DAL.Abstract
{
    public interface IWildfireRepository
    {
        ICollection<FireDatum> GetWildfires();

        Task AddWildfiresAsync(List<FireDatum> wildfires);

    }
}