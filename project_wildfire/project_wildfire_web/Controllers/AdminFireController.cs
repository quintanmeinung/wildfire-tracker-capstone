using Microsoft.AspNetCore.Mvc;
using project_wildfire_web.Models;
using project_wildfire_web.Models.DTO;
using project_wildfire_web.ExtensionsMethods;
using project_wildfire_web.Models;

namespace project_wildfire_web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminFireController : ControllerBase
    {
        private readonly FireDataDbContext _context;

        public AdminFireController(FireDataDbContext context)
        {
            _context = context;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] FireDTO fireDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid fire data");

            // Force IsAdminFire to true (just in case someone tries to spoof)
            fireDto.IsAdminFire = true;

            var fireEntity = fireDto.ToFire();

            _context.Fires.Add(fireEntity);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "🔥 Admin fire successfully saved.",
                fireId = fireEntity.FireId
            });
        }
    }
}

