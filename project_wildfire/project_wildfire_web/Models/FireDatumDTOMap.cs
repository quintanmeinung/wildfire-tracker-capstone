using CsvHelper.Configuration;
using project_wildfire_web.Models.DTO;


namespace project_wildfire_web.Models;

public class FireDTOMap : ClassMap<FireDTO>
{
    public FireDTOMap()
    {
        Map(m => m.Latitude).Name("latitude");
        Map(m => m.Longitude).Name("longitude");
        Map(m => m.RadiativePower).Name("frp"); // NASA's Fire Radiative Power column
    }
}