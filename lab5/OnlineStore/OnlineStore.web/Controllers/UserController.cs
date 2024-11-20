using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.web.Controllers;

public class UserController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}