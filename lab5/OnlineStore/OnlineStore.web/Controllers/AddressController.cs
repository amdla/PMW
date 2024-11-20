using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.web.Controllers;

public class AddressController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}