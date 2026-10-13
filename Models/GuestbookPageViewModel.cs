namespace GuestbookApp.Models;

public class GuestbookPageViewModel
{
    public GuestEntry Input { get; set; } = new();

    public IReadOnlyList<GuestEntry> Entries { get; set; } = [];
}