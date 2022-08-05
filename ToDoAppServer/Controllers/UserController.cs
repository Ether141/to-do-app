﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoAppServer.Core;
using ToDoAppServer.Models;

namespace ToDoAppServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly AccountsManager accountsManager;
        private readonly JWTManager jwtManager;
        private readonly ILogger<UserController> logger;

        public UserController(AccountsManager accountsManager, JWTManager jwtManager, ILogger<UserController> logger)
        {
            this.accountsManager = accountsManager;
            this.jwtManager = jwtManager;
            this.logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public ActionResult Register([FromForm] UserRegisterDTO user)
        {
            logger.LogInformation("Attempt to register user");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            RegisterResult result = accountsManager.Register(user, out int newUserId);

            if (result == RegisterResult.Success)
            {
                logger.LogInformation("Created new user with id: {a}", newUserId);
                return Ok(Json(new Dictionary<string, int>() { { "id", newUserId } }));
            }
            else
            {
                logger.LogInformation("Unable to create new user: {a}", result.ToString());
                return CreateRegisterResultObject(result);
            }
        }

        [HttpPost]
        [Route("login")]
        public ActionResult Login([FromForm] UserLoginDTO dto)
        {
            logger.LogInformation("Attempt to login: {a}", dto.Nickname);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            LoginResult result = accountsManager.Login(dto, out TokenResult token);

            if (result != LoginResult.Success && result != LoginResult.SuccessShouldUpdatePassword)
            {
                logger.LogInformation("Failed to login: {a} | {b}", dto.Nickname, result);
                return CreateLoginResultObject(result);
            }

            return Ok(new Dictionary<string, object>() 
            {
                { "code", ((int)result).ToString() },
                { "message", result.ToString() },
                { "token", token }
            });
        }

        [HttpGet]
        [Authorize]
        [Route("all")]
        public IEnumerable<User> GetAllUsers() => accountsManager.AllUsers;

        [HttpPost]
        [Route("refresh")]
        public ActionResult RefreshToken([FromBody] TokenRefreshRequestDTO dto)
        {
            if (!ModelState.IsValid || dto.Token == null || dto.RefreshToken == null)
                return BadRequest(ModelState);

            ClaimsPrincipal principal = jwtManager.GetPrincipalFromExpiredToken(dto.Token);

            if (principal.Identity == null || principal.Identity.Name == null)
                return BadRequest();

            string nickname = principal.Identity.Name;
            User? user = accountsManager.GetUser(nickname);

            if (user == null || user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.Now)
                return BadRequest();

            TokenResult newToken = jwtManager.CreateTokenResult(user);

            user.RefreshToken = newToken.RefreshToken;
            user.RefreshTokenExpiryDate = DateTime.Now.AddDays(1);

            return Ok(newToken);
        }

        private BadRequestObjectResult CreateRegisterResultObject(RegisterResult result)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>()
            {
                { "code", ((int)result).ToString() },
                { "message", result.ToString() }
            };
            return BadRequest(Json(errors));
        }

        private BadRequestObjectResult CreateLoginResultObject(LoginResult result)
        {
            Dictionary<string, string> errors = new Dictionary<string, string>()
            {
                { "code", ((int)result).ToString() },
                { "message", result.ToString() }
            };
            return BadRequest(Json(errors));
        }
    }
}
