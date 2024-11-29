using SQLite;
using System.Collections.Generic;

namespace ToDoApp.Models
{
    public class ToDoItem
    {
        [PrimaryKey, AutoIncrement] public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }

        // Relacja 1:1 z User (klucz obcy)
        public int? UserId { get; set; } // Klucz obcy może być null
        public User User { get; set; }

        // Relacja wiele do wielu z Category
        public List<ToDoItemCategory> ToDoItemCategories { get; set; } = new();
    }
}