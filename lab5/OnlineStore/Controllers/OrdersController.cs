using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult GetOrders()
    {
        var orders = context.Orders.ToList();
        return Ok(orders);
    }

    [HttpPost]
    public IActionResult CreateOrder(Order order)
    {
        context.Orders.Add(order);
        context.SaveChanges();
        return CreatedAtAction(nameof(GetOrders), new { id = order.Id }, order);
    }
}