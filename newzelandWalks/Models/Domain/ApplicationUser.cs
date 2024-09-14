using Microsoft.AspNetCore.Identity;

namespace newzelandWalks.Models.Domain
{
    public class ApplicationUser:IdentityUser
    {
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
    }
}
