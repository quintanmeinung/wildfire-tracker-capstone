
using System.ComponentModel.DataAnnotations;
namespace project_wildfire_web.Models;

public partial class AqiStation
{
    [Key] //This was added for my nunit test F14
    public string? Name { get; set; }

    public string? StationId { get; set; }
}
