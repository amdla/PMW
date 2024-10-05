using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace RazorApp.Pages;

public class AjaxAxiosForm : PageModel
{
    [BindProperty] public string Name { get; set; }

    [BindProperty] public string Surname { get; set; }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            // if everything works correctly, send JSON response for Ajax request
            return new JsonResult(new
            {
                success = true,
                message = "Form submitted successfully!",
                name = Name,
                surname = Surname,
            });
        }

        return new JsonResult(new { success = false });
    }
}