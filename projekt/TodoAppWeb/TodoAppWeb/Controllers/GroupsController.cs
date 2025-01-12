namespace TodoAppWeb.Controllers;

using Microsoft.AspNetCore.Mvc;
using TodoAppWeb.Models;

public class GroupsController : Controller
{
    private readonly ApiService _apiService;

    public GroupsController(ApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var groups = await _apiService.GetGroupsAsync();
        return View(groups);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(GroupModel group)
    {
        if (ModelState.IsValid)
        {
            await _apiService.AddGroupAsync(group);
            return RedirectToAction(nameof(Index));
        }

        return View(group);
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _apiService.DeleteGroupAsync(id);
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Tasks(int id)
    {
        return RedirectToAction("Index", "Tasks", new { groupId = id });
    }
}