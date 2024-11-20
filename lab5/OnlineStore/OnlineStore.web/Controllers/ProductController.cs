using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data;
using OnlineStore.Models;

namespace OnlineStore.Web.Controllers;

public class ProductController(AppDbContext context) : Controller
{
    public IActionResult Index()
    {
        var products = context.Products.ToList();
        return View(products);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            context.Products.Add(product);
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(product);
    }
}