namespace TodoAppWeb.Models
{
    public class Group
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TodoTask> Tasks { get; set; } = new HashSet<TodoTask>();
    }
}