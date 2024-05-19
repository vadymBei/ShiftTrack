using Kernel.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Kernel.Extensions
{
    public static class ErrorHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseKernelExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
