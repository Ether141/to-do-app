using ToDoAppServer.Middleware;
using ToDoAppServer.Models;

namespace ToDoAppServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddTransient<ApiKeyMiddleware>();

            WebApplication? app = builder.Build();
            app.UseAuthorization();
            app.MapControllers();
            app.UseMiddleware<ApiKeyMiddleware>();
            app.Run();
        }
    }
}