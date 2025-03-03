using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using project_wildfire_web.Models;
using project_wildfire_web.DAL.Abstract;

namespace project_wildfire_web.Controllers;

    [ApiController]
    [Route("api/WildfireAPIController")]
    
    public class WildfireAPIController : ControllerBase
    {
        private readonly IWildfireRepository _wildfireRepository;
        private readonly ILogger<WildfireAPIController> _logger;

        public WildfireAPIController(IWildfireRepository wildfireRepository)
        {
            _wildfireRepository = wildfireRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FireDatum>))]

        public IActionResult GetWildfires()
        {
            var wildfires = _wildfireRepository.GetWildfires();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(wildfires);
        }



    }




    
