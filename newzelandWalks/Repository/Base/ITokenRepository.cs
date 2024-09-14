using Microsoft.AspNetCore.Identity;
using newzelandWalks.Models.Domain;

namespace newzelandWalks.Repository.Base
{
    public interface ITokenRepository
    {
       string CreateJwtToken(IdentityUser user, List<string> roles);
        RefreshToken GenerateRefreshToken();
        void SetRefreshToken(RefreshToken newRefreshToken);
    }
}
