
namespace project_wildfire_web.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    // User preference properties
    public string FontSize { get; set; } = "medium";
    public bool ContrastMode { get; set; } = false;
    public bool TextToSpeech { get; set; } = false;

    public virtual ICollection<Fire> Fires { get; set; } = new List<Fire>();
    public virtual ICollection<UserFireSubscription> FireSubscriptions { get; set; }
    
}
