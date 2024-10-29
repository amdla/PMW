using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

    // Action to list all books
    public IActionResult Index()
    {
        var books = ReadJsonFile();
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
