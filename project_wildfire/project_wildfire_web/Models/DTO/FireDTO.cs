using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
using project_wildfire_web.Models;
using NetTopologySuite.Geometries;



namespace project_wildfire_web.Models.DTO
{
public partial class FireDTO
{
    [Required]
    [Name("latitude")]
    public double Latitude { get; set; }

    [Required]
    [Name("longitude")]
    public double Longitude { get; set; }

    //[Required]
    //public Geometry Location { get; set; } = null!;


    [Required]
    [Name("frp")]
    public decimal RadiativePower { get; set; }

}

}

namespace project_wildfire_web.ExtensionsMethods
    {
    
        public static class FireExtensions
        {
    public static Fire ToFire (this Models.DTO.FireDTO fire)
    {   
        return new Fire
        {
        Latitude = (decimal)fire.Latitude,
        Longitude = (decimal)fire.Longitude,
        RadiativePower = fire.RadiativePower
        };

    }
        }

    }