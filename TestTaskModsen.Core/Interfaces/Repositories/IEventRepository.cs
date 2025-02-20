using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Repositories;

public interface IEventRepository
{
    Task<PagedResult<Event>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Event> GetByIdAsync(Guid eventId, CancellationToken cancellationToken);
    Task<Event> GetByTitleAsync(string title, CancellationToken cancellationToken);

    Task<PagedResult<Event>> GetByFiltersAsync(
        int pageNumber,
        int pageSize,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? location = null,
        EventCategory? category = null,
        CancellationToken cancellationToken = default);

    Task CreateAsync(Event @event, CancellationToken cancellationToken);
    Task UpdateAsync(Event @event, CancellationToken cancellationToken);
    Task UpdateImageDataAsync(Guid eventId, byte[] imageData, CancellationToken cancellationToken);
    Task DeleteAsync(Guid eventId, CancellationToken cancellationToken);
}