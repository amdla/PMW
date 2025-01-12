using Microsoft.EntityFrameworkCore;

namespace TodoApi
{

    using TodoApi.Models;

    public class TodoContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Group> Groups { get; set; }

        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=TodoApi.db");
            }
        }
    }


}
