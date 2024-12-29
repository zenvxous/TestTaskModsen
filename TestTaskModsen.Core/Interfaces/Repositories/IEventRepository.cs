using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Repositories;

public interface IEventRepository
{
    Task<PagedResult<Event>> GetAllAsync(int pageNumber, int pageSize);
    Task<Event> GetByIdAsync(Guid eventId);
    Task<Event> GetByTitleAsync(string title);

    Task<PagedResult<Event>> GetByFiltersAsync(
        int pageNumber,
        int pageSize,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? location = null,
        EventCategory? category = null);

    Task CreateAsync(Event @event);
    Task UpdateAsync(Event @event);
    Task UpdateImageDataAsync(Guid eventId, byte[] imageData);
    Task DeleteAsync(Guid eventId);
}