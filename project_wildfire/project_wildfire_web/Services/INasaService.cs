using project_wildfire_web.Models;
using Microsoft.AspNetCore.Mvc;
using project_wildfire_web.Models.DTO;


namespace project_wildfire_web.Services;

public interface INasaService
{ 
    Task<List<FireDTO>> GetFiresAsync ();


}