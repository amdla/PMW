using SQLite;

namespace ToDoApp.Models
{
    public class ToDoItemCategory
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }

        public int ToDoItemId { get; set; }
        public ToDoItem ToDoItem { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

}