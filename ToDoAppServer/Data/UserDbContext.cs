using Microsoft.EntityFrameworkCore;
using ToDoAppServer.Models;

namespace ToDoAppServer.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; private set; }

        public UserDbContext(DbContextOptions options) : base(options) { }
    }
}
