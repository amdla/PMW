using SQLite;

namespace ToDoApp.Models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }
        public string Name { get; set; }

        public List<ToDoItemCategory> ToDoItemCategories { get; set; } = new();
    }
}