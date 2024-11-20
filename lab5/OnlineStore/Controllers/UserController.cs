using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = context.Users.ToList();
        return Ok(users);
    }

    [HttpPost]
    public IActionResult CreateUser(User user)
    {
        context.Users.Add(user);
        context.SaveChanges();
        return CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
    }
}