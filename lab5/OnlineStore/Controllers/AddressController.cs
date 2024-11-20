using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AddressesController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAddresses()
    {
        var addresses = context.Addresses.ToList();
        return Ok(addresses);
    }

    [HttpPost]
    public IActionResult CreateAddress(Address address)
    {
        context.Addresses.Add(address);
        context.SaveChanges();
        return CreatedAtAction(nameof(GetAddresses), new { id = address.Id }, address);
    }
}