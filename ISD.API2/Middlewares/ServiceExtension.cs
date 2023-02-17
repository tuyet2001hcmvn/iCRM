using Microsoft.AspNetCore.Builder;

namespace ISD.API2.Middlewares
{
    public static class ServiceExtension
    {
        public static void UseExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
        public static void UseLoggingRequestMiddleware(this IApplicationBuilder app)
        {    
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
