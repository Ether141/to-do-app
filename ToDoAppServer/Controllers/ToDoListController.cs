using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToDoAppServer.Core;
using ToDoAppServer.Data;
using ToDoAppServer.Models;
using ToDoAppSharedModels.Common;
using ToDoAppSharedModels.Requests;

namespace ToDoAppServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDoListController : Controller
    {
        private readonly ToDoListDbContext dbContext;
        private readonly ILogger<UserController> logger;
        private readonly JWTManager jwtManager;
        private readonly AccountsManager accountsManager;

        public ToDoListController(ToDoListDbContext dbContext, ILogger<UserController> logger, AccountsManager accountsManager, JWTManager jwtManager)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.accountsManager = accountsManager;
            this.jwtManager = jwtManager;
        }

        [HttpGet]
        [Authorize]
        [Route("all")]
        public ActionResult<IEnumerable<ToDoList>> GetToDoLists()
        {
            string token = JWTManager.GetTokenFromRequest(Request);
            User? user = accountsManager.GetUser(jwtManager.GetPrincipalFromToken(token, false)!.Identity!.Name!);

            if (user == null)
                return BadRequest();

            logger.LogInformation("user: {a} requested all his todo lists", user.Nickname);
            IEnumerable<ToDoList> result = dbContext.ToDoLists.Where(l => l.UserId == user.Id).ToList();
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("add")]
        public ActionResult<ToDoList> AddToDoList([FromBody] AddToDoListDTO dto)
        {
            logger.LogInformation("attempt to add a new todo list");

            User? user = GetUserFromToken(JWTManager.GetTokenFromRequest(Request));

            if (!ModelState.IsValid || user == null)
                return BadRequest();

            ToDoList list = new ToDoList
            {
                Name = dto.ListName,
                UserId = user.Id
            };

            list = dbContext.ToDoLists.Add(list).Entity;
            dbContext.SaveChanges();

            logger.LogInformation("added a new list with an id {a} and name {b} for user {c}", list.Id, list.Name, user.Nickname);

            return Ok(list);
        }

        [HttpPost]
        [Authorize]
        [Route("remove")]
        public ActionResult RemoveList([FromBody] RemoveListDTO dto)
        {
            User? user = GetUserFromToken(JWTManager.GetTokenFromRequest(Request));

            if (!ModelState.IsValid || user == null)
                return BadRequest();

            ToDoList? list = dbContext.ToDoLists.FirstOrDefault(l => l.Id == dto.Id && l.UserId == user.Id);

            if (list == null)
                return BadRequest($"unable to find list with given id: {dto.Id}");

            logger.LogInformation("removed list with id: {a}", dto.Id);
            dbContext.ToDoLists.Remove(list);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("rename")]
        public ActionResult RenameList([FromBody] RenameListDTO dto)
        {
            User? user = GetUserFromToken(JWTManager.GetTokenFromRequest(Request));

            if (!ModelState.IsValid || user == null)
                return BadRequest();

            ToDoList? list = dbContext.ToDoLists.FirstOrDefault(l => l.Id == dto.Id && l.UserId == user.Id);

            if (list == null)
                return BadRequest($"unable to find list with given id: {dto.Id}");

            logger.LogInformation("renamed list with id {a}. new name: {b}", dto.Id, dto.NewName);
            list.Name = dto.NewName;
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("addentry")]
        public ActionResult<ToDoEntry> AddToDoEntry([FromBody] AddListEntryDTO dto)
        {
            logger.LogInformation("attempt to add new todo entry to list with id {a}", dto.ListId);

            User? user = GetUserFromToken(JWTManager.GetTokenFromRequest(Request));

            if (!ModelState.IsValid || user == null)
                return BadRequest();

            ToDoList? todoList = dbContext.ToDoLists.FirstOrDefault(l => l.Id == dto.ListId && l.UserId == user.Id);

            if (todoList == null)
                return BadRequest("todo list with given id does not exist, or does not belong to this user");

            ToDoEntry entry = new ToDoEntry
            {
                Text = dto.EntryName,
                IsDone = false,
                ToDoListId = todoList.Id
            };

            entry = dbContext.ToDoEntries.Add(entry).Entity;
            dbContext.SaveChanges();

            logger.LogInformation("added a new entry {a} | to the list with an id: {b}", entry.Text, todoList.Id);

            return Ok(entry);
        }

        [HttpPost]
        [Authorize]
        [Route("removeentry")]
        public ActionResult RemoveToDoEntry([FromBody] RemoveListEntryDTO dto)
        {
            User? user = GetUserFromToken(JWTManager.GetTokenFromRequest(Request));

            if (!ModelState.IsValid || user == null)
                return BadRequest();

            ToDoEntry? entry = dbContext.ToDoEntries.FirstOrDefault(e => e.Id == dto.Id);

            if (entry == null)
                return BadRequest("todo list entry with given id does not exist");

            logger.LogInformation("removed todo entry with id: {a}", dto.Id);
            dbContext.ToDoEntries.Remove(entry);
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("renameentry")]
        public ActionResult RenameToDoEntry([FromBody] RenameListEntryDTO dto)
        {
            User? user = GetUserFromToken(JWTManager.GetTokenFromRequest(Request));

            if (!ModelState.IsValid || user == null)
                return BadRequest();

            ToDoEntry? entry = dbContext.ToDoEntries.FirstOrDefault(e => e.Id == dto.Id);

            if (entry == null)
                return BadRequest("todo list entry with given id does not exist");

            logger.LogInformation("renamed todo list entry with id: {a}. new name: {b}", dto.Id, dto.NewName);

            entry.Text = dto.NewName;
            dbContext.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("changeentriesstate")]
        public ActionResult ChangeToDoEntriesState([FromBody] ChangeToDoEntryStateDTO dto)
        {
            User? user = GetUserFromToken(JWTManager.GetTokenFromRequest(Request));

            if (!ModelState.IsValid || user == null)
                return BadRequest();

            IEnumerable<ToDoEntry> todoEntries = dbContext.ToDoEntries.Where(e => dto.EntriesIdsAndStates.Keys.Contains(e.Id));

            if (todoEntries == null)
                return BadRequest("unable to find any todo entry");

            foreach (ToDoEntry entry in todoEntries)
                entry.IsDone = dto.EntriesIdsAndStates.GetValueOrDefault(entry.Id);

            logger.LogInformation("changed todo list entries states");

            dbContext.SaveChanges();
            return Ok();
        }

        private User? GetUserFromToken(string token) => accountsManager.GetUser(jwtManager.GetPrincipalFromToken(token, false)!.Identity!.Name!);
    }
}
