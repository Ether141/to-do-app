using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoAppServer.Core;
using ToDoAppServer.Models;

namespace ToDoAppServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly AccountsManager accountsManager;
        private readonly ILogger<UserController> logger;

        public UserController(AccountsManager accountsManager, ILogger<UserController> logger)
        {
            this.accountsManager = accountsManager;
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
        public ActionResult Login([FromForm] UserLoginDTO user)
        {
            logger.LogInformation("Attempt to login: {a}", user.Nickname);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            LoginResult result = accountsManager.Login(user, out string token);

            if (result != LoginResult.Success && result != LoginResult.SuccessShouldUpdatePassword)
            {
                logger.LogInformation("Failed to login: {a} | {b}", user.Nickname, result);
                return CreateLoginResultObject(result);
            }

            return Ok(new Dictionary<string, string>() 
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
