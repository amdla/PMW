namespace TodoApi.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Group
    {
        public int GroupId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }

}
