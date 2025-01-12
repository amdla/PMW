namespace TodoAppWeb.Controllers;

using Microsoft.AspNetCore.Mvc;
using TodoAppWeb.Models;

public class TasksController : Controller
{
    private readonly ApiService _apiService;

    public TasksController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var tasks = await _apiService.GetTasksAsync();
        return View(tasks);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskModel task)
    {
        if (ModelState.IsValid)
        {
            await _apiService.AddTaskAsync(task);
            return RedirectToAction(nameof(Index));
        }

        return View(task);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var tasks = await _apiService.GetTasksAsync();
        var task = tasks.FirstOrDefault(t => t.TaskId == id);
        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TaskModel task)
    {
        if (ModelState.IsValid)
        {
            await _apiService.UpdateTaskAsync(task);
            return RedirectToAction(nameof(Index));
        }

        return View(task);
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _apiService.DeleteTaskAsync(id);
        return RedirectToAction(nameof(Index));
    }


}