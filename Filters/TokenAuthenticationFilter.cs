using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Catalog.Interfaces.TokenAuthorization;
using Catalog.Services.TokenAuthentication;
using System.Security.Claims;
using Catalog.Interfaces.Redis;
using Newtonsoft.Json;
using Catalog.Contracts;

namespace Catalog.Filters
{
    public class TokenAuthenticationFilter : Attribute, IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool result = await isAuthorized(context);

            if (!result)
            {
                context.ModelState.AddModelError("Unauthorized", "Unauthorized access");
                context.Result = new UnauthorizedObjectResult(context.ModelState);
            }
        }

        private static async Task<bool> isAuthorized(AuthorizationFilterContext context)
        {
            TokenManager? tokenManager = (TokenManager?)
                context.HttpContext.RequestServices.GetService(typeof(ITokenManager));

            bool hasAuthorization = context.HttpContext.Request.Headers.ContainsKey(
                "Authorization"
            );
            if (!hasAuthorization)
            {
                return false;
            }
            string token = string.Empty;
            var authorizationString = context.HttpContext.Request.Headers.First(
                x => x.Key == "Authorization"
            );

            if (!string.IsNullOrEmpty(authorizationString.Value))
            {
                String auth = authorizationString.Value;
                String[] spearator = { " " };
                Int32 count = 2;
                String[] authArrayItems = auth.Split(
                    spearator,
                    count,
                    StringSplitOptions.RemoveEmptyEntries
                );
                if (authArrayItems[0] == "Bearer")
                {
                    ClaimsPrincipal? claimPrinciple;
                    try
                    {
                        token = authArrayItems[1];
                        claimPrinciple = tokenManager.VerifyToken(token);
                        IRedisResponseCache? cacheService =
                            context.HttpContext.RequestServices.GetRequiredService<IRedisResponseCache>();
                        var cachedUserResponse = await cacheService.GetCachedResponseAsync(token);
                        if (!string.IsNullOrEmpty(cachedUserResponse))
                        {
                            var userInfo = JsonConvert.DeserializeObject<UserInfo>(
                                cachedUserResponse
                            );
                            context.HttpContext.Items.Add("LoggedInUser", userInfo);
                            return true;
                        }
                        return false;
                    }
                    catch (Exception e)
                    {
                        return false;
                    }
                }
                else if (authArrayItems[0] == "x-bot-auth") { }
                return false;
            }
            return false;
        }
    }
}

//
