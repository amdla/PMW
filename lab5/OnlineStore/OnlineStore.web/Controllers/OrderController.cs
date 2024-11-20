using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.web.Controllers;

public class OrderController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}