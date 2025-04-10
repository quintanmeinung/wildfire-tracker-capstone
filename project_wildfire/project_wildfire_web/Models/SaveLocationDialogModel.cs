using Microsoft.AspNetCore.Identity;

namespace project_wildfire_web.Models;
public class SaveLocationDialogModel
{
    public required string Title { get; set; }
    public string? Address { get; set; }
    public string? Lat { get; set; }
    public string? Lng { get; set; }
}