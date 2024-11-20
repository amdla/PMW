using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.web.Controllers;

public class CategoryController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}