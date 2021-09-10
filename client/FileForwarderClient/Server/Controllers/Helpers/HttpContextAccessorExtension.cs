using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FileForwarderClient.Server.Controllers.Helpers
{
    public static class HttpContextAccessorExtension
    {
        public static string GetUserId(this IHttpContextAccessor accessor)
        {
            return accessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
