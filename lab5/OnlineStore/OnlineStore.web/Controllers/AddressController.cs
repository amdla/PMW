using Microsoft.AspNetCore.Mvc;
using OnlineStore.Data;
using OnlineStore.Models;

namespace OnlineStore.Web.Controllers;

public class AddressController : Controller
{
    private readonly AppDbContext _context;

    public AddressController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var addresses = _context.Addresses.ToList();
        return View(addresses);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Address address)
    {
        if (ModelState.IsValid)
        {
            _context.Addresses.Add(address);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(address);
    }
}