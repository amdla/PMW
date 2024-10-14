using Microsoft.AspNetCore.Mvc;
using zad.Models;
using zad.Extensions; // Make sure to include this for the custom session extensions

namespace zad.Controllers
{
    public class ToDoAppController : Controller
    {
        private const string SessionKey = "ToDoList";

        [HttpGet]
        public IActionResult Index()
        {
            var model = new ToDoAppModel
            {
                Items = HttpContext.Session.Get<List<ToDoItem>>(SessionKey) ?? new List<ToDoItem>()
            };
            return View("ToDoApp", model);
        }

        [HttpPost]
        public IActionResult AddTask(ToDoAppModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.NewTask))
            {
                var items = HttpContext.Session.Get<List<ToDoItem>>(SessionKey) ?? new List<ToDoItem>();
                items.Add(new ToDoItem { Task = model.NewTask, IsCompleted = false });
                HttpContext.Session.Set(SessionKey, items);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult DeleteTask(int index)
        {
            var items = HttpContext.Session.Get<List<ToDoItem>>(SessionKey);
            if (items != null && index >= 0 && index < items.Count)
            {
                items.RemoveAt(index);
                HttpContext.Session.Set(SessionKey, items);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ToggleComplete(int index)
        {
            var items = HttpContext.Session.Get<List<ToDoItem>>(SessionKey);
            if (items != null && index >= 0 && index < items.Count)
            {
                items[index].IsCompleted = !items[index].IsCompleted;
                HttpContext.Session.Set(SessionKey, items);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}