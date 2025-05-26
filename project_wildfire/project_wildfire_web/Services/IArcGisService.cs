using System.Collections.Generic;
using System.Threading.Tasks;
using project_wildfire_web.Models.ArcGis;

namespace project_wildfire_web.Services
{
    /// <summary>
    /// Abstraction for querying wildfire data from the ArcGIS Feature Service.
    /// </summary>
    public interface IArcGisService
    {
        /// <summary>
        /// Retrieves all active wildfire events.
        /// </summary>
        //Task<List<FireEvent>> GetActiveWildfireEventsAsync();

        /// <summary>
        /// Retrieves the details for a single wildfire by its OBJECTID.
        /// </summary>
       // Task<FireEvent?> GetEventDetailsAsync(string objectId);
        Task<List<FireEvent>> GetUsaWildfiresAsync();
    }
}