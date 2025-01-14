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
            var groups = await _apiService.GetGroupsAsync();
            return View(groups);
        }
    }
}