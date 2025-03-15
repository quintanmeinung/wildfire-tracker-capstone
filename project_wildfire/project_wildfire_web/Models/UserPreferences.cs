using System;
using System.Collections.Generic;

namespace project_wildfire_web.Models;

public partial class UserPreferences
{
    public string UserId { get; set; }
    public string FontSize { get; set; } = "medium";
    public bool ContrastMode { get; set; } = false;
    public bool TextToSpeech { get; set; } = false;
    
    public virtual User User { get; set; } // Navigation property
}
