using project_wildfire_web.Models;

namespace project_wildfire_web.Services;

public interface INasaService
{ 
    Task<List<Fire>> GetFiresAsync ();
}