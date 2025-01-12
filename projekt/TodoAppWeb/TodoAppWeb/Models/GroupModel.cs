namespace TodoAppWeb.Models;

public class GroupModel
{
    public int GroupId { get; set; }
    public string Name { get; set; }
    public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
}