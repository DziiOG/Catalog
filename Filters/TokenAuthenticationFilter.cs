using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Catalog.Interfaces.TokenAuthorization;
using Catalog.Services.TokenAuthentication;
using System.Security.Claims;
using Catalog.Interfaces.Redis;

namespace Catalog.Filters
{
    public class TokenAuthenticationFilter : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool result = true;

            TokenManager? tokenManager = (TokenManager?)
                context.HttpContext.RequestServices.GetService(typeof(ITokenManager));

            bool hasAuthorization = context.HttpContext.Request.Headers.ContainsKey(
                "Authorization"
            );
            if (!hasAuthorization)
            {
                result = false;
            }
            string token = string.Empty;
            if (result)
            {
                var authorizationString = context.HttpContext.Request.Headers.First(
                    x => x.Key == "Authorization"
                );

                if (!string.IsNullOrEmpty(authorizationString.Value))
                {
                    String auth = authorizationString.Value;
                    String[] spearator = { " " };
                    Int32 count = 2;

                    // using the method
                    String[] authArrayItems = auth.Split(
                        spearator,
                        count,
                        StringSplitOptions.RemoveEmptyEntries
                    );

                    if (authArrayItems[0] == "Bearer")
                    {
                        ClaimsPrincipal? claimPrinciple;

                        if (tokenManager is not null)
                        {
                            try
                            {
                                token = authArrayItems[1];
                                claimPrinciple = tokenManager.VerifyToken(token);
                                IRedisResponseCache? cacheService =
                                    context.HttpContext.RequestServices.GetRequiredService<IRedisResponseCache>();
                                var cachedUserResponse =
                                    await cacheService.GetCachedUserResponseAsync(token);
                                Console.WriteLine("I arrived here");
                                if (!string.IsNullOrEmpty(cachedUserResponse))
                                {
                                    Console.WriteLine($"user: {cachedUserResponse}");
                                }
                                else
                                {
                                    result = false;
                                    context.ModelState.AddModelError("Unauthorized", "");
                                }
                            }
                            catch (Exception e)
                            {
                                result = false;
                                context.ModelState.AddModelError("Unauthorized", e.ToString());
                            }
                        }
                        else
                        {
                            result = false;
                            context.ModelState.AddModelError("Unauthorized", "");
                        }
                    }
                    else
                    {
                        result = false;
                        context.ModelState.AddModelError("Unauthorized", "");
                    }
                }
                else
                {
                    result = false;
                    context.ModelState.AddModelError("Unauthorized", "");
                }
            }

            if (!result)
            {
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }
        }
    }
}
