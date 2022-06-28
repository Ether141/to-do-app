using Microsoft.Extensions.Primitives;

namespace ToDoAppServer.Middleware
{
    public class ApiKeyMiddleware : IMiddleware
    {
        public const string ApiKeyName = "todoapp-apikey";
        public const string ApiKeyHeader = "x-api-key";

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out StringValues extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api key was not provided ");
                return;
            }

            IConfiguration? appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
            string? apiKey = appSettings.GetValue<string>(ApiKeyName);

            if (!apiKey.Equals(extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client");
                return;
            }

            await next(context);
        }
    }
}
