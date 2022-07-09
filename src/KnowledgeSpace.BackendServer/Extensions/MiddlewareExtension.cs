using KnowledgeSpace.BackendServer.Helpers;
using Microsoft.AspNetCore.Builder;

namespace KnowledgeSpace.BackendServer.Extensions
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UserErrorWrapping(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorWrappingMiddleware>();
        }
    }
}
