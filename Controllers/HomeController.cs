using System.Collections.Concurrent;
using GuestbookApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace GuestbookApp.Controllers;

public class HomeController : Controller
{
    private static readonly ConcurrentQueue<GuestEntry> Entries = new();

    [HttpGet]
    public IActionResult Index()
    {
        return View(CreatePageModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(GuestEntry input)
    {
        if (!ModelState.IsValid)
        {
            var page = CreatePageModel();
            page.Input = input;
            return View(page);
        }

        input.Name = input.Name.Trim();
        input.Address = input.Address.Trim();
        input.CreatedAt = DateTimeOffset.Now;
        Entries.Enqueue(input);

        TempData["SuccessMessage"] = "Terima kasih, kunjungan Anda sudah tercatat.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }

    private static GuestbookPageViewModel CreatePageModel()
    {
        return new GuestbookPageViewModel
        {
            Entries = Entries.ToArray().Reverse().ToArray()
        };
    }
}