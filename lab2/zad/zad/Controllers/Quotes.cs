
using Microsoft.AspNetCore.Mvc;

namespace zad.Controllers
{
    public class QuotesController : Controller
    {
        [HttpGet]
        public IActionResult Quotes()
        {
            // No session handling, just serving the view
            return View("Quotes");
        }
    }
}
