using SQLite;

namespace ToDoApp.Models
{
    public class ToDoItemCategory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int ToDoItemId { get; set; }   // Klucz obcy do ToDoItem
        public int CategoryId { get; set; }    // Klucz obcy do Category
    }
}