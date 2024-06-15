using Microsoft.AspNetCore.Builder;
using ShiftTrack.Kernel.Middleware;

namespace ShiftTrack.Kernel.Extensions
{
    public static class ErrorHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseKernelExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
