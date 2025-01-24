using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Services;

public interface IEventService
{
    Task<bool> IsEventExistsAsync(Guid eventId);
    Task<PagedResult<Event>> GetAllEvents(int pageNumber, int pageSize);
    Task<Event> GetEventById(Guid eventId);
    Task<Event> GetEventByTitle(string title);
    Task CreateEvent(Event @event);
    Task UpdateEvent(Event @event);
    Task DeleteEvent(Guid eventId);
    Task<PagedResult<Event>> GetEventsByFilter(
        int pageNumber,
        int pageSize,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? location = null,
        EventCategory? category = null);

    Task<byte[]> GetImageData(Guid eventId);
    Task UpdateImageData(Guid eventId, byte[] imageData);
}