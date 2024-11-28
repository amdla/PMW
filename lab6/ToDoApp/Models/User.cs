using SQLite;

namespace ToDoApp.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; } // Jeśli chcesz przechowywać hasło

        // Relacja 1:1: Jeden użytkownik ma jedno zadanie
        public ToDoItem? ToDoItem { get; set; }
    }
}