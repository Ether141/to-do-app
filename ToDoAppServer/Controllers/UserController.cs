using Microsoft.AspNetCore.Mvc;
using ToDoAppServer.Models;

namespace ToDoAppServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        [HttpGet]
        [Route("{id}")]
        public ActionResult<string> UserName(int id)
        {
            string userName = "example_user_name_123";

            if (id == 0)
                return userName;
            else
                return NotFound();
        }

        [HttpGet]
        [Route("all")]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            List<User> allUsers = new List<User>
            {
                new User(0, "ether141", "email@gmail.com"),
                new User(1, "kox3", "email@gmail.com"),
                new User(2, "eman91_", "email@gmail.com")
            };
            return Ok(allUsers);
        }

        [HttpPost]
        [Route("add")]
        public ActionResult AddUser([FromForm]User? user)
        {
            if (user == null)
                return BadRequest();

            user.Id = 0;
            return Ok();
        }
    }
}
