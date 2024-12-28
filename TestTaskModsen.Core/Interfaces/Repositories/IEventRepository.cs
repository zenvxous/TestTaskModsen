using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Repositories;

public interface IEventRepository
{
    Task<List<Event>> GetAllAsync();
    Task<Event> GetByIdAsync(Guid eventId);
    Task<Event> GetByTitleAsync(string title);

    Task<List<Event>> GetByFiltersAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? location = null,
        EventCategory? category = null);

    Task CreateAsync(Event @event);
    Task UpdateAsync(Event @event);
    Task UpdateImageDataAsync(Guid eventId, byte[] imageData);
    Task DeleteAsync(Guid eventId);
}