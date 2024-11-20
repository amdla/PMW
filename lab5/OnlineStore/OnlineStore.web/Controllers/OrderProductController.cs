using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data;
using OnlineStore.Models;

namespace OnlineStore.Web.Controllers;

public class OrderProductController : Controller
{
    private readonly AppDbContext _context;

    public OrderProductController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var orderProducts = _context.OrderProducts.ToList();
        return View(orderProducts);
    }

    public IActionResult Create()
    {
        ViewBag.Orders = _context.Orders.ToList();
        ViewBag.Products = _context.Products.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult Create(OrderProduct orderProduct)
    {
        if (ModelState.IsValid)
        {
            _context.OrderProducts.Add(orderProduct);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Orders = _context.Orders.ToList();
        ViewBag.Products = _context.Products.ToList();
        return View(orderProduct);
    }
}