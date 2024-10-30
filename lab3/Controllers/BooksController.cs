using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace lab3.Controllers;

public class BooksController : Controller
{
    private readonly string _jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "books.json");

    // Helper method to read JSON file
    private List<Book> ReadJsonFile()
    {
        var jsonData = System.IO.File.ReadAllText(_jsonFilePath);
        return JsonConvert.DeserializeObject<List<Book>>(jsonData) ?? new List<Book>();
    }

    // Helper method to write to JSON file
    private void WriteJsonFile(List<Book> books)
    {
        var jsonData = JsonConvert.SerializeObject(books, Formatting.Indented);
        System.IO.File.WriteAllText(_jsonFilePath, jsonData);
    }

    // Action to display all books
    public IActionResult Index(string searchQuery, string authorFilter, string genreFilter, int? yearFilter,
        string sortOrder, float priceFilter)
    {
        // Read books from JSON
        var books = ReadJsonFile();

        // Search functionality (by Name or ID)
        if (!string.IsNullOrEmpty(searchQuery))
        {
            books = books.Where(b => b.Name.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                                     b.Id.ToString() == searchQuery).ToList();
        }

        // Filter by author
        if (!string.IsNullOrEmpty(authorFilter))
        {
            books = books.Where(b => b.Author.Equals(authorFilter, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Filter by genre
        if (!string.IsNullOrEmpty(genreFilter))
        {
            books = books.Where(b => b.LiteraryGenre.Equals(genreFilter, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Filter by year
        if (yearFilter.HasValue)
        {
            books = books.Where(b => b.YearOfPublish == yearFilter.Value).ToList();
        }

        //filter by availability
        if (sortOrder == "availability_asc")
        {
            books = books.Where(b => b.IsAvailable).ToList();
        }
        else if (sortOrder == "availability_desc")
        {
            books = books.Where(b => !b.IsAvailable).ToList();
        }

        //filter by price
        if (priceFilter > 0)
        {
            books = books.Where(b => b.Price == priceFilter).ToList();
        }

        // Sorting
        books = sortOrder switch
        {
            "name_asc" => books.OrderBy(b => b.Name).ToList(),
            "name_desc" => books.OrderByDescending(b => b.Name).ToList(),
            "author_asc" => books.OrderBy(b => b.Author).ToList(),
            "author_desc" => books.OrderByDescending(b => b.Author).ToList(),
            "availability_asc" => books.OrderBy(b => b.IsAvailable).ToList(),
            "availability_desc" => books.OrderByDescending(b => b.IsAvailable).ToList(),
            "year_asc" => books.OrderBy(b => b.YearOfPublish).ToList(),
            "year_desc" => books.OrderByDescending(b => b.YearOfPublish).ToList(),
            "genre_asc" => books.OrderBy(b => b.LiteraryGenre).ToList(),
            "genre_desc" => books.OrderByDescending(b => b.LiteraryGenre).ToList(),
            "price_asc" => books.OrderBy(b => b.Price).ToList(),
            "price_desc" => books.OrderByDescending(b => b.Price).ToList(),
            _ => books.ToList()
        };

        return View(books);
    }

    // Action for viewing the create form
    public IActionResult Create()
    {
        return View();
    }

    // Action to create a new book
    [HttpPost]
    public IActionResult Create(Book book)
    {
        if (ModelState.IsValid)
        {
            var books = ReadJsonFile();
            book.Id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
            books.Add(book);
            WriteJsonFile(books);
            return RedirectToAction(nameof(Index));
        }

        return View(book);
    }

    // Action for viewing the edit form
    public IActionResult Edit(int id)
    {
        var books = ReadJsonFile();
        var book = books.FirstOrDefault(b => b.Id == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // Action to edit an existing book
    [HttpPost]
    public IActionResult Edit(Book book)
    {
        if (ModelState.IsValid)
        {
            var books = ReadJsonFile();
            var bookToUpdate = books.FirstOrDefault(b => b.Id == book.Id);
            if (bookToUpdate == null)
            {
                return NotFound();
            }

            // Update book details
            bookToUpdate.Name = book.Name;
            bookToUpdate.Author = book.Author;
            bookToUpdate.IsAvailable = book.IsAvailable;
            bookToUpdate.YearOfPublish = book.YearOfPublish;
            bookToUpdate.LiteraryGenre = book.LiteraryGenre;
            bookToUpdate.Price = book.Price;

            WriteJsonFile(books);
            return RedirectToAction(nameof(Index));
        }

        return View(book);
    }

    // Action to delete a book
    public IActionResult Delete(int id)
    {
        var books = ReadJsonFile();
        var bookToDelete = books.FirstOrDefault(b => b.Id == id);
        if (bookToDelete != null)
        {
            books.Remove(bookToDelete);
            WriteJsonFile(books);
        }

        return RedirectToAction(nameof(Index));
    }
}