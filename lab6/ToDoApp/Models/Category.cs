using SQLite;

namespace ToDoApp.Models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        // Relacja: Kategoria ma wiele zadań
        public List<ToDoItemCategory> ToDoItems { get; set; } = new List<ToDoItemCategory>();
    }
}