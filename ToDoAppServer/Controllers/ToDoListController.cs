using Microsoft.AspNetCore.Mvc;
using ToDoAppServer.Data;
using ToDoAppSharedModels.Common;

namespace ToDoAppServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoListController : Controller
    {
        private readonly ToDoListDbContext dbContext;
        private readonly ILogger<UserController> logger;

        public ToDoListController(ToDoListDbContext dbContext, ILogger<UserController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        [HttpGet]
        [Route("all")]
        public IEnumerable<ToDoList> GetToDoLists()
        {
            IEnumerable<ToDoList> result = dbContext.ToDoLists.ToList();
            return result;
        }
    }
}
