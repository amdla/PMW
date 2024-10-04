using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace RazorApp.Pages;

public class Form : PageModel
{
    [BindProperty] public string Name { get; set; }

    [BindProperty] public string Surname { get; set; }

    [TempData] public string SuccessMessage { get; set; }

    public void OnGet()
    {
    }

    // Przekazanie danych z logiki aplikacji do widoku poprzed 'TempData'
    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            SuccessMessage = "Form submitted successfully!";
            return Page();
        }

        return Page();
    }
}