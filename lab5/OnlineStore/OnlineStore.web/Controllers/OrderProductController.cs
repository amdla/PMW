using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.web.Controllers;

public class OrderProductController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}