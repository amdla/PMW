using Microsoft.AspNetCore.Mvc;
using TodoAppWeb.Services;

namespace TodoAppWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly TodoApiService _apiService;

        public HomeController(TodoApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Home/Index
        public async Task<IActionResult> Index()
        {
            // Get current edit mode from session
            bool isEditMode = HttpContext.Session.GetString("IsEditMode") == "true";
            ViewBag.IsEditMode = isEditMode;

            // Retrieve the list of groups from your API or data store
            var groups = await _apiService.GetGroupsAsync();
            return View(groups);
        }

        public IActionResult ToggleEdit(string? returnUrl)
        {
            // Read current state from session
            bool isEditMode = HttpContext.Session.GetString("IsEditMode") == "true";

            // Flip the state
            isEditMode = !isEditMode;

            // Write it back to session
            HttpContext.Session.SetString("IsEditMode", isEditMode ? "true" : "false");

            // If we have a returnUrl, use it. Otherwise, default to Index.
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index");
        }
    }
}