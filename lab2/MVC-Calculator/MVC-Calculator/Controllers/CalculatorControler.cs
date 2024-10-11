namespace MVC_Calculator.Controllers;

using Models;
using Microsoft.AspNetCore.Mvc;

public class CalculatorController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new CalculatorModel());
    }

    [HttpPost]
    public IActionResult Index(CalculatorModel model)
    {
        if (ModelState.IsValid)
        {
            model.Result = model.Number1 + model.Number2;
        }

        return View(model);
    }
}