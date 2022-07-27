using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
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
                byte[]? key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);

                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        }
    }
}