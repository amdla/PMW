using Microsoft.AspNetCore.Mvc;
using TodoAppWeb.Models;
using TodoAppWeb.Services;

namespace TodoAppWeb.Controllers
{
    public class GroupController : Controller
    {
        private readonly TodoApiService _apiService;

        public GroupController(TodoApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Group/Create
        public IActionResult CreateGroup()
        {
            return View();
        }

        // POST: Group/CreateGroup
        [HttpPost]
        public async Task<IActionResult> CreateGroup(Group group)
        {
            if (string.IsNullOrWhiteSpace(group.Name))
            {
                ModelState.AddModelError("Name", "Group name cannot be empty.");
                return View(group);
            }

            await _apiService.AddGroupAsync(group);
            return RedirectToAction("Index", "Home"); // Redirect to the main page
        }


        public async Task<IActionResult> DetailsGroup(int id)
        {
            var groups = await _apiService.GetGroupsAsync();
            var group = groups.FirstOrDefault(g => g.GroupId == id);

            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }


        // GET: Group/Edit/5
        public async Task<IActionResult> EditGroup(int id)
        {
            var groups = await _apiService.GetGroupsAsync();
            var group = groups.FirstOrDefault(g => g.GroupId == id);

            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }

        // POST: Group/Edit/5
        [HttpPost]
        public async Task<IActionResult> EditGroup(Group group)
        {
            if (ModelState.IsValid)
            {
                await _apiService.UpdateGroupAsync(group);
                return RedirectToAction("DetailsGroup", new { id = group.GroupId });
            }

            return View(group);
        }

        public async Task<IActionResult> DeleteTask(int taskId, int groupId)
        {
            await _apiService.DeleteTaskAsync(taskId);

            var groups = await _apiService.GetGroupsAsync();
            var group = groups.FirstOrDefault(g => g.GroupId == groupId);

            if (group?.Tasks.Count == 0)
            {
                await _apiService.DeleteGroupAsync(groupId);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("DetailsGroup", new { id = groupId });
        }
    }
}