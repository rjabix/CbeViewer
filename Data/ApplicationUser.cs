using Microsoft.AspNetCore.Identity;

namespace CbeViewer.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public ulong? SecondsWatched { get; set; }
}