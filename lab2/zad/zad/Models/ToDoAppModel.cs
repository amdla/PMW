namespace zad.Models
{
    public class ToDoItem
    {
        public string Task { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class ToDoAppModel
    {
        public List<ToDoItem> Items { get; set; }
        public string NewTask { get; set; }
    }
}