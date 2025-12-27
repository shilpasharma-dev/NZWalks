using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repostitories
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles
            );
    }
}
