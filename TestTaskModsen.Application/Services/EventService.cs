using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using TestTaskModsen.Application.Validators;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Interfaces.Services;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _repository;
    private readonly IMemoryCache _cache;

    public EventService(IEventRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<bool> IsEventExistsAsync(Guid eventId, CancellationToken cancellationToken)
    {
        try
        {
            await _repository.GetByIdAsync(eventId, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<PagedResult<Event>> GetAllEvents(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(pageNumber, pageSize, cancellationToken);
    }

    public async Task<Event> GetEventById(Guid eventId, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(eventId, cancellationToken);
    }

    public async Task<Event> GetEventByTitle(string title, CancellationToken cancellationToken)
    {
        return await _repository.GetByTitleAsync(title, cancellationToken);
    }

    public async Task CreateEvent(
        Guid id,
        string title,
        string description,
        DateTime startDate,
        DateTime endDate,
        string location,
        EventCategory category,
        int capacity,
        CancellationToken cancellationToken)
    {
        var @event = new Event(
            id,
            title,
            description,
            startDate,
            endDate,
            location,
            category,
            capacity,
            [],
            []);
        
        var eventValidator = new EventValidator();
        await eventValidator.ValidateAndThrowAsync(@event, cancellationToken);
        
        await _repository.CreateAsync(@event, cancellationToken);
    }

    public async Task UpdateEvent(
        Guid id,
        string title,
        string description,
        DateTime startDate,
        DateTime endDate,
        string location,
        EventCategory category,
        int capacity,
        CancellationToken cancellationToken)
    {
        if (!await IsEventExistsAsync(id, cancellationToken))
            throw new KeyNotFoundException("Event not found");
        
        var @event = new Event(
            id,
            title,
            description,
            startDate,
            endDate,
            location,
            category,
            capacity,
            [],
            []);
        
        var eventValidator = new EventValidator();
        await eventValidator.ValidateAndThrowAsync(@event, cancellationToken);
        
        await _repository.UpdateAsync(@event, cancellationToken);
    }

    public async Task DeleteEvent(Guid id, CancellationToken cancellationToken)
    { 
        if (!await IsEventExistsAsync(id, cancellationToken))
            throw new KeyNotFoundException("Event not found");
        
        await _repository.DeleteAsync(id, cancellationToken);
    }

    public async Task<PagedResult<Event>> GetEventsByFilter(
        int pageNumber,
        int pageSize,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? location = null,
        EventCategory? category = null,
        CancellationToken cancellationToken = default)
    {
        return await _repository.GetByFiltersAsync(pageNumber, pageSize, startDate, endDate, location, category, cancellationToken);
    }

    public async Task<byte[]> GetImageData(Guid eventId, CancellationToken cancellationToken)
    {
        var cacheKey = $"ImageData-{eventId}";
        
        if (_cache.TryGetValue(cacheKey, out byte[]? imageData))
            return imageData!;
        
        var @event = await _repository.GetByIdAsync(eventId, cancellationToken);
        
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(30));
        _cache.Set(cacheKey, @event.ImageData, cacheEntryOptions);
        
        return @event.ImageData;
    }

    public async Task UpdateImageData(Guid eventId, byte[] imageData, CancellationToken cancellationToken)
    {
        await _repository.UpdateImageDataAsync(eventId, imageData, cancellationToken);
        _cache.Remove($"ImageData-{eventId}");
    }
}
