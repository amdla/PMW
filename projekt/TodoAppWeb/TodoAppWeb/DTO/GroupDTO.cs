namespace TodoAppWeb.DTO

{
    public class GroupDto
    {
        public int GroupId { get; set; }
        public string Name { get; set; }
        public List<TaskDto> Tasks { get; set; } = new List<TaskDto>();
    }
}