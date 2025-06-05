
using System.ComponentModel.DataAnnotations;
namespace project_wildfire_web.Models;

public partial class AqiStation
{
    [Key] 
    public string? Name { get; set; }

    public string? StationId { get; set; }
}
