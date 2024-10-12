namespace zad.Controllers;

using Models;
using Microsoft.AspNetCore.Mvc;

public class ToDoAppController : Controller
{
    [HttpGet]
    public IActionResult ToDoApp()
    {
        return View(new ToDoAppModel());
    }

    [HttpPost]
    public IActionResult ToDoApp(ToDoAppModel model)
    {
        if (ModelState.IsValid)
        {
        }

        return View(model);
    }
}