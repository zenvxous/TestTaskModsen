using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Services;

public interface IEventService
{
    Task<bool> IsEventExistsAsync(Guid eventId, CancellationToken cancellationToken);
    Task<PagedResult<Event>> GetAllEvents(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Event> GetEventById(Guid eventId, CancellationToken cancellationToken);
    Task<Event> GetEventByTitle(string title, CancellationToken cancellationToken);

    Task CreateEvent(
        Guid id,
        string title,
        string description,
        DateTime startDate,
        DateTime endDate,
        string location,
        EventCategory category,
        int capacity,
        CancellationToken cancellationToken);

    Task UpdateEvent(
        Guid id,
        string title,
        string description,
        DateTime startDate,
        DateTime endDate,
        string location,
        EventCategory category,
        int capacity,
        CancellationToken cancellationToken);
    Task DeleteEvent(Guid id, CancellationToken cancellationToken);
    Task<PagedResult<Event>> GetEventsByFilter(
        int pageNumber,
        int pageSize,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? location = null,
        EventCategory? category = null,
        CancellationToken cancellationToken = default);

    Task<byte[]> GetImageData(Guid eventId, CancellationToken cancellationToken);
    Task UpdateImageData(Guid eventId, byte[] imageData, CancellationToken cancellationToken);
}