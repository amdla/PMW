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

            // Make sure you set IsEditMode from session so the Layout sees the correct mode
            bool isEditMode = HttpContext.Session.GetString("IsEditMode") == "true";
            ViewBag.IsEditMode = isEditMode;

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

        // GET: Group/EditGroup/123
        public async Task<IActionResult> EditGroup(int id)
        {
            // 1. Get all the groups from the service or DB.
            var groups = await _apiService.GetGroupsAsync();

            // 2. Find the specific group by its ID.
            var group = groups.FirstOrDefault(g => g.GroupId == id);
            if (group == null)
            {
                return NotFound();
            }

            // (Optional) Set or read IsEditMode from Session if you need to show/hide UI.
            bool isEditMode = HttpContext.Session.GetString("IsEditMode") == "true";
            ViewBag.IsEditMode = isEditMode;

            // 3. Return the view for editing. Pass the "group" model to it.
            return View(group);
        }

        // POST: Group/EditGroup
        [HttpPost]
        public async Task<IActionResult> EditGroup(Group updatedGroup)
        {
            // 1. Validate
            if (!ModelState.IsValid)
            {
                // If validation fails, re-display the form with the user’s input
                return View(updatedGroup);
            }

            // 2. Call your API or database to update the group
            //    You need an UpdateGroupAsync or similar in your service:
            //    e.g.: await _apiService.UpdateGroupAsync(updatedGroup);
            await _apiService.UpdateGroupAsync(updatedGroup);

            // 3. Once updated, redirect to the “Index” or “DetailsGroup” as you prefer
            return RedirectToAction("Index", "Home");
        }
    }
}