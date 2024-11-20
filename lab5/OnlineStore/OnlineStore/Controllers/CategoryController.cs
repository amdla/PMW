using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data;
using OnlineStore.Models;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCategories()
    {
        var categories = context.Categories.ToList();
        return Ok(categories);
    }

    [HttpPost]
    public IActionResult CreateCategory(Category category)
    {
        context.Categories.Add(category);
        context.SaveChanges();
        return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
    }
}