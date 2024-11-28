using SQLite;

namespace ToDoApp.Models
{
    public class ToDoItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        public int UserId { get; set; } // Relacja 1:1 z User (jeden użytkownik ma jedno zadanie)

        // Relacja: Zadanie może mieć wiele kategorii przez ToDoItemCategory
        public List<ToDoItemCategory> Categories { get; set; }

    }
}