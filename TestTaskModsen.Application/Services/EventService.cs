using FluentValidation;
using TestTaskModsen.Application.Validators;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Interfaces.Services;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _repository;

    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> IsEventExistsAsync(Guid eventId)
    {
        try
        {
            await _repository.GetByIdAsync(eventId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<PagedResult<Event>> GetAllEvents(int pageNumber, int pageSize)
    {
        return await _repository.GetAllAsync(pageNumber, pageSize);
    }

    public async Task<Event> GetEventById(Guid eventId)
    {
        return await _repository.GetByIdAsync(eventId);
    }

    public async Task<Event> GetEventByTitle(string title)
    {
        return await _repository.GetByTitleAsync(title);
    }

    public async Task CreateEvent(Event @event)
    {
        var eventValidator = new EventValidator();
        await eventValidator.ValidateAndThrowAsync(@event);
        
        await _repository.CreateAsync(@event);
    }

    public async Task UpdateEvent(Event @event)
    {
        var eventValidator = new EventValidator();
        await eventValidator.ValidateAndThrowAsync(@event);
        
        await _repository.UpdateAsync(@event);
    }

    public async Task DeleteEvent(Guid eventId)
    {
        await _repository.DeleteAsync(eventId);
    }

    public async Task<PagedResult<Event>> GetEventsByFilter(
        int pageNumber,
        int pageSize,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? location = null,
        EventCategory? category = null)
    {
        return await _repository.GetByFiltersAsync(pageNumber, pageSize, startDate, endDate, location, category);
    }

    public async Task UpdateImageData(Guid eventId, byte[] imageData)
    {
        await _repository.UpdateImageDataAsync(eventId, imageData);
    }
}