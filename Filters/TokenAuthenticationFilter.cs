using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Interfaces.TokenAuthorization;
using Catalog.Services.TokenAuthentication;
using System.Security.Claims;

namespace Catalog.Filters
{
    public class TokenAuthenticationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool result = true;

            TokenManager? tokenManager = (TokenManager?)
                context.HttpContext.RequestServices.GetService(typeof(ITokenManager));

            bool hasAuthorization = context.HttpContext.Request.Headers.ContainsKey(
                "Authorization"
            );
            Console.WriteLine(hasAuthorization);
            if (!hasAuthorization)
            {
                result = false;
            }
            string token = string.Empty;
            if (result)
            {
                token = context.HttpContext.Request.Headers
                    .First(x => x.Key == "Authorization")
                    .Value;
                ClaimsPrincipal? claimPrinciple;

                if (tokenManager is not null)
                {
                    try
                    {
                        Console.WriteLine("Came here");
                        claimPrinciple = tokenManager.VerifyToken(token);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Came here was here");
                        result = false;
                        context.ModelState.AddModelError("Unauthorized", e.ToString());
                    }
                }
                else
                {
                    result = false;
                }
            }

            if (!result)
            {
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }
        }
    }
}
