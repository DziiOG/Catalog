using System.Security.Claims;

namespace Catalog.Api.Interfaces.TokenAuthorization
{
    public interface ITokenManager
    {
        bool Authenticate(string userName, string password);

        string NewToken();
        ClaimsPrincipal? VerifyToken(string token);
    }
}
