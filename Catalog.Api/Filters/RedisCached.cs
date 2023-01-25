using System.Text;
using Catalog.Api.Interfaces.Redis;
using Catalog.Api.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalog.Api.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class RedisCached : Attribute, IAsyncActionFilter
    {
        private readonly int duration;
        private readonly bool Enabled;

        public RedisCached(int duration, bool Enabled = true)
        {
            this.duration = duration;
            this.Enabled = Enabled;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next
        )
        {
            RedisCacheSettings? cacheSettings =
                context.HttpContext.RequestServices.GetRequiredService<RedisCacheSettings>();
            if (!Enabled || !cacheSettings.Enabled)
            {
                await next();
                return;
            }

            IRedisResponseCache? cacheService =
                context.HttpContext.RequestServices.GetRequiredService<IRedisResponseCache>();

            string cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);

            // var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);

            // if (!string.IsNullOrEmpty(cachedResponse))
            // {
            //     var contentResult = new ContentResult()
            //     {
            //         Content = cachedResponse,
            //         ContentType = "application/json",
            //         StatusCode = 200
            //     };
            //     context.Result = contentResult;
            //     return;
            // }
            ActionExecutedContext? excutedContext = await next();

            if (excutedContext.Result is OkObjectResult okObjectResult)
            {
                // await cacheService.CacheResponseAsync(
                //     cacheKey,
                //     okObjectResult.Value,
                //     duration: TimeSpan.FromSeconds(duration)
                // );
            }
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
