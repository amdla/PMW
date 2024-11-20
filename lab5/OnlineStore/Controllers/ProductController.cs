using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult GetProducts()
    {
        var products = context.Products.ToList();
        return Ok(products);
    }

    [HttpPost]
    public IActionResult CreateProduct(Product product)
    {
        context.Products.Add(product);
        context.SaveChanges();
        return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
    }

}
