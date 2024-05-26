using Microsoft.AspNetCore.Identity;

namespace ShiftTrack.Authentication.Models
{
    public class User : IdentityUser
    {
        public string Login { get; set; }

        public List<string> Roles { get; set; }
    }
}
