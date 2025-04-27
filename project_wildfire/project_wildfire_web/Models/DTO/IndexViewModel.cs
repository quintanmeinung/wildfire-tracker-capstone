namespace project_wildfire_web.Models.DTO;

public class IndexViewModel
{
    public ProfileViewModelDTO Profile { get; set; }
    public UserLocationDTO Location { get; set; }
    public ICollection<UserLocationDTO> SavedLocations { get; set; }

    public IndexViewModel(ProfileViewModelDTO pvm, UserLocationDTO ul, ICollection<UserLocationDTO> sl)
    {
        Profile = pvm;
        Location = ul;
        SavedLocations = sl;
    }
}