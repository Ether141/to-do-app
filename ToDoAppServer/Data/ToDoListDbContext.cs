using Microsoft.EntityFrameworkCore;
using ToDoAppSharedModels.Common;

namespace ToDoAppServer.Data
{
    public class ToDoListDbContext : DbContext
    {
        public DbSet<ToDoList> ToDoLists { get; private set; }
        public DbSet<ToDoEntry> ToDoEntries { get; private set; }

        public ToDoListDbContext(DbContextOptions options) : base(options) { }
    }
}
