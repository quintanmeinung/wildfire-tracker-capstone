using System;
using System.Collections.Generic;

namespace project_wildfire_web.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public virtual ICollection<Fire> Fires { get; set; } = new List<Fire>();
}
