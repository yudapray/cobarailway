using GuestbookApp.Models;

namespace GuestbookApp.Data;

public interface IGuestbookRepository
{
    Task<IReadOnlyList<GuestEntry>> GetAllEntriesAsync();
    Task AddEntryAsync(GuestEntry entry);
    Task<int> GetTotalCountAsync();
}

