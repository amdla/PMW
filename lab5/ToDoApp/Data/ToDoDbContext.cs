using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;

namespace ToDoApp.Data
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<ToDoItemCategory> ToDoItemCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacja wiele do wielu: ToDoItem i Category przez ToDoItemCategory
            modelBuilder.Entity<ToDoItemCategory>()
                .HasKey(ic => new { ic.ToDoItemId, ic.CategoryId });

            modelBuilder.Entity<ToDoItemCategory>()
                .HasOne(ic => ic.ToDoItem)
                .WithMany(i => i.ToDoItemCategories)
                .HasForeignKey(ic => ic.ToDoItemId);

            modelBuilder.Entity<ToDoItemCategory>()
                .HasOne(ic => ic.Category)
                .WithMany(c => c.ToDoItemCategories)
                .HasForeignKey(ic => ic.CategoryId);

            // Relacja 1:* User -> ToDoItems
            modelBuilder.Entity<ToDoItem>()
                .HasOne(t => t.User)
                .WithMany(u => u.ToDoItems)
                .HasForeignKey(t => t.UserId)
                .IsRequired(false) // Klucz obcy może być null
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}