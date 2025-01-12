namespace TodoApi.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class Task
    {
        public int TaskId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string? Description { get; set; }

        public bool IsCompleted { get; set; } = false;

        public int GroupId { get; set; } // Klucz obcy
        public Group Group { get; set; } // Nawigacja do grupy
    }

}
