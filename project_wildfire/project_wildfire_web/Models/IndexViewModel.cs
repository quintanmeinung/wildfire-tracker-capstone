namespace project_wildfire_web.Models.DTO;

public class IndexViewModel
{
    public ProfileViewModelDTO Profile { get; set; }
    public UserLocationDTO Location { get; set; }

    public IndexViewModel(ProfileViewModelDTO pvm, UserLocationDTO ul)
    {
        Profile = pvm;
        Location = ul;
    }
}