using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ToDoAppServer.Models;
using System.Security.Claims;

namespace ToDoAppServer.Core
{
    public class JWTManager
    {
        private readonly IConfiguration configuration;

        public JWTManager(IConfiguration configuration) => this.configuration = configuration;

        public string GenerateToken(User user)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            byte[]? key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Nickname)
                }),
                Expires = DateTime.UtcNow.AddSeconds(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken? token = handler.CreateToken(descriptor);
            string tokenString = handler.WriteToken(token);
            return tokenString;
        }
    }
}
