using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace session_storage_to_redis.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        public string role { get; set; } = string.Empty;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (string.IsNullOrEmpty(context.HttpContext.Session.GetString("id")))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401
                };

                return;
            }

            else if (context.HttpContext.Session.GetString("role") != role)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 403
                };

                return;
            }

            await next();
        }
    }
}