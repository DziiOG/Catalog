using System.IdentityModel.Tokens.Jwt;
using Catalog.Api.Interfaces.TokenAuthorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Catalog.Api.Settings;

namespace Catalog.Api.Services.TokenAuthentication
{
    public class TokenManager : ITokenManager
    {
        private JwtSecurityTokenHandler tokenHandler;
        private byte[] secretKey;

        private JwsSettings jwtSettings;

        public TokenManager(JwsSettings jwtSettings)
        {
            this.jwtSettings = jwtSettings;
            tokenHandler = new JwtSecurityTokenHandler();
            secretKey = Encoding.ASCII.GetBytes(jwtSettings.Secret);
        }

        public bool Authenticate(string userName, string password)
        {
            if (
                !string.IsNullOrWhiteSpace(userName)
                && !string.IsNullOrWhiteSpace(password)
                && userName.ToLower() == "admin"
                && password == "password"
            )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string NewToken()
        {
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(
                    new Claim[] { new Claim(ClaimTypes.Name, "Frank Liu") }
                ),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };
            Microsoft.IdentityModel.Tokens.SecurityToken token = tokenHandler.CreateToken(
                tokenDescriptor
            );
            string jwtString = tokenHandler.WriteToken(token);

            return jwtString;
        }

        public ClaimsPrincipal? VerifyToken(string token)
        {
            ClaimsPrincipal? claims = tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                },
                out SecurityToken validatedToken
            );
            return claims;
        }
    }
}
