namespace zad.Controllers;

using Models;
using Microsoft.AspNetCore.Mvc;

public class CalculatorController : Controller
{
    [HttpGet]
    public IActionResult Calculator()
    {
        return View(new CalculatorModel());
    }

    [HttpPost]
    public IActionResult Calculator(CalculatorModel model)
    {
        if (ModelState.IsValid)
        {
            model.Result = model.Number1 + model.Number2;
        }

        return View(model);
    }
}