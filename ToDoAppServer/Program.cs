using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ToDoAppServer.Core;
using ToDoAppServer.Data;
using ToDoAppServer.Middleware;

namespace ToDoAppServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
            ConfigureJWT(builder);
            builder.Services.AddControllers();
            builder.Services.AddTransient<ApiKeyMiddleware>();
            builder.Services.AddDbContextPool<UserDbContext>(options => options.UseMySQL(builder.Configuration.GetConnectionString("ConnectionString")));
            builder.Services.AddSingleton<JWTManager>();
            builder.Services.AddScoped<AccountsManager>();
            builder.Services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddFilter(level => level >= LogLevel.Information);
            });

            WebApplication? app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseMiddleware<ApiKeyMiddleware>();
            app.Run();
        }

        private static void ConfigureJWT(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = JWTManager.CreateTokenValidationParameters(builder.Configuration);
            });
        }
    }
}