using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderProductsController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult GetOrderProducts()
    {
        var orderProducts = context.OrderProducts.ToList();
        return Ok(orderProducts);
    }

    [HttpPost]
    public IActionResult CreateOrderProduct(OrderProduct orderProduct)
    {
        context.OrderProducts.Add(orderProduct);
        context.SaveChanges();
        return CreatedAtAction(nameof(GetOrderProducts),
            new { orderId = orderProduct.OrderId, productId = orderProduct.ProductId }, orderProduct);
    }
}