using TodoAppWeb.Services;

namespace TodoAppWeb.Controllers;

using Microsoft.AspNetCore.Mvc;
using Models;

public class TaskController : Controller
{
    private readonly TodoApiService _apiService;

    public TaskController(TodoApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> IndexTask()
    {
        var tasks = await _apiService.GetTasksAsync();
        return View(tasks);
    }

    public IActionResult CreateTask(int groupId)
    {
        var task = new TodoTask { GroupId = groupId };
        return View(task);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(TodoTask task)
    {
        if (ModelState.IsValid)
        {
            await _apiService.AddTaskAsync(task);
            return RedirectToAction(nameof(IndexTask));
        }

        return View(task);
    }

    public async Task<IActionResult> EditTask(int id)
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
    public async Task<IActionResult> EditTask(TodoTask task)
    {
        if (ModelState.IsValid)
        {
            await _apiService.UpdateTaskAsync(task);
            return RedirectToAction(nameof(IndexTask));
        }

        return View(task);
    }

    public async Task<IActionResult> DeleteTask(int id)
    {
        await _apiService.DeleteTaskAsync(id);
        return RedirectToAction(nameof(IndexTask));
    }
}