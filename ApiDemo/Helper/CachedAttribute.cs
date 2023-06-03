using Core.Entities.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace ApiDemo.Helper
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveInSeconds;
        public CachedAttribute( int timeToLiveInSeconds)
        {
            _timeToLiveInSeconds= timeToLiveInSeconds;

        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheServices = context.HttpContext.RequestServices.GetRequiredService<IResponseCasheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cacheResponse=await cacheServices.GetCashedResponse(cacheKey);
            if(!string.IsNullOrEmpty(cacheResponse) )
            {
                var ContentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = ContentResult;
                return;
            }
            var executedContext = await next();
            if (executedContext.Result is OkObjectResult okObjectResult)
                await cacheServices.CasheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveInSeconds));
        }
        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keybulider = new StringBuilder();
            keybulider.Append($"{request.Path}");
            foreach (var (key,value) in request.Query.OrderBy(k=>k.Key))
           
                keybulider.Append($"|{key}-{value}");

            return keybulider.ToString(); 
        }
    }
}
