using System.ComponentModel.DataAnnotations;

public class Book
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Author { get; set; }

    public bool IsAvailable { get; set; }

    [Required]
    [Range(1500, 2100, ErrorMessage = "Year of publish must be between 1500 and 2100")]
    public int YearOfPublish { get; set; }

    [Required]
    public string LiteraryGenre { get; set; }
}
