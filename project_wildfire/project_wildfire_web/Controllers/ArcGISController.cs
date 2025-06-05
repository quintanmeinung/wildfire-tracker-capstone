

using Microsoft.AspNetCore.Mvc;
using project_wildfire_web.Models.ArcGis;
using project_wildfire_web.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Controllers
{
    [ApiController]
    [Route("api/wildfires")]
    public class ArcGISController : ControllerBase
    {
        private readonly IArcGisService _arcGisService;

        public ArcGISController(IArcGisService arcGisService)
        {
            _arcGisService = arcGisService;
        }

        /// <summary>
        /// GET /api/wildfires
        /// Returns all active wildfires.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FireEvent>>> GetAllWildfires()
        {
            var fires = await _arcGisService.GetUsaWildfiresAsync();
            return Ok(fires);
        }
    }
}