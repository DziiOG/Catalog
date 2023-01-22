using System.Security.Claims;
using Catalog.TokenAuthentication;

namespace Catalog.Interfaces.TokenAuthorization
{
    public interface ITokenManager
    {
        bool Authenticate(string userName, string password);

        string NewToken();
        ClaimsPrincipal? VerifyToken(string token);
    }
}
