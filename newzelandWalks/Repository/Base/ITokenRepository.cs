using Microsoft.AspNetCore.Identity;

namespace newzelandWalks.Repository.Base
{
    public interface ITokenRepository
    {
       string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
