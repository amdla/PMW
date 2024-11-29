using SQLite;
using System.Collections.Generic;

namespace ToDoApp.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Relacja 1:* (jeden użytkownik ma wiele zadań)
        public List<ToDoItem> ToDoItems { get; set; } = new();
    }
}