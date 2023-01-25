using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Catalog.Api.Interfaces.TokenAuthorization;
using Catalog.Api.Services.TokenAuthentication;
using System.Security.Claims;
using Catalog.Api.Interfaces.Redis;
using Newtonsoft.Json;
using Catalog.Api.Contracts;
using System.Text;
using Catalog.Api.Settings;
using Microsoft.Extensions.Primitives;

namespace Catalog.Api.Filters
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
                string[] authArrayItems = GetSplittedAuthorizationString(authorizationString);
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
                else if (authArrayItems[0] == "x-bot-auth")
                {
                    try
                    {
                        ResquestBotSettings? botSettings = (ResquestBotSettings?)
                            context.HttpContext.RequestServices.GetService(
                                typeof(ResquestBotSettings)
                            );
                        byte[]? stringBytes = System.Convert.FromBase64String(authArrayItems[1]);
                        string? decodedBytes = Encoding.Default.GetString(stringBytes);
                        String[] spearator = { ":" };
                        Int32 count = 2;
                        String[] splittedByteCode = decodedBytes.Split(
                            spearator,
                            count,
                            StringSplitOptions.RemoveEmptyEntries
                        );
                        if (
                            botSettings != null
                            && splittedByteCode[0] == botSettings.BotEmail
                            && splittedByteCode[1] == botSettings.BotPassword
                        )
                        {
                            Bot bot = new Bot()
                            {
                                BotEmail = botSettings.BotEmail,
                                BotPassword = botSettings.BotPassword
                            };
                            context.HttpContext.Items.Add("BotAccess", bot);
                            return true;
                        }
                        return false;
                    }
                    catch (System.Exception)
                    {
                        return false;
                    }
                }
                return false;
            }
            return false;
        }

        private static string[] GetSplittedAuthorizationString(
            KeyValuePair<string, StringValues> authorizationString
        )
        {
            String auth = authorizationString.Value;
            String[] spearator = { " " };
            Int32 count = 2;
            var authArrayItems = auth.Split(
                spearator,
                count,
                StringSplitOptions.RemoveEmptyEntries
            );
            return authArrayItems;
        }
    }
}

//
