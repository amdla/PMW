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

    public async Task<IActionResult> CreateTask(int groupId)
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
            return RedirectToAction("DetailsGroup", "Group", new { id = task.GroupId });
        }

        return View(task);
    }

    [HttpGet]
    public async Task<IActionResult> EditTask(int groupId, int taskId)
    {
        var tasks = await _apiService.GetTasksAsync();
        var task = tasks.FirstOrDefault(t => t.TaskId == taskId && t.GroupId == groupId);
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
            return RedirectToAction("DetailsGroup", "Group", new { id = task.GroupId });
        }

        return View(task);
    }

}