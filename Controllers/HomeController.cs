using GuestbookApp.Data;
using GuestbookApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace GuestbookApp.Controllers;

public class HomeController : Controller
{
    private readonly IGuestbookRepository _guestbookRepository;

    public HomeController(IGuestbookRepository guestbookRepository)
    {
        _guestbookRepository = guestbookRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var entries = await _guestbookRepository.GetAllEntriesAsync();
        return View(new GuestbookPageViewModel
        {
            Entries = entries
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(GuestEntry input)
    {
        if (!ModelState.IsValid)
        {
            var entries = await _guestbookRepository.GetAllEntriesAsync();
            var page = new GuestbookPageViewModel
            {
                Input = input,
                Entries = entries
            };
            return View(page);
        }

        input.Name = input.Name.Trim();
        input.Address = input.Address.Trim();
        input.Message = string.IsNullOrWhiteSpace(input.Message) ? null : input.Message.Trim();
        input.CreatedAt = DateTimeOffset.Now;

        await _guestbookRepository.AddEntryAsync(input);

        TempData["SuccessMessage"] = "Terima kasih, catatan kunjungan Anda berhasil disimpan ke database SQLite!";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Error()
    {
        return View();
    }
}