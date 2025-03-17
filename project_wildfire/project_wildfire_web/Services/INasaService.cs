using project_wildfire_web.Models;
using Microsoft.AspNetCore.Mvc;


namespace project_wildfire_web.Services;

public interface INasaService
{ 
    Task<List<Fire>> GetFiresAsync ();

}