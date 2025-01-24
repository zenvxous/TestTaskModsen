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

    public async Task<byte[]> GetImageData(Guid eventId)
    {
        var cacheKey = $"ImageData-{eventId}";
        
        if (_cache.TryGetValue(cacheKey, out byte[]? imageData))
            return imageData!;
        
        var @event =  await _repository.GetByIdAsync(eventId);
        
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(30));
        _cache.Set(cacheKey, @event.ImageData, cacheEntryOptions);
        
        return @event.ImageData;
    }

    public async Task UpdateImageData(Guid eventId, byte[] imageData)
    {
        await _repository.UpdateImageDataAsync(eventId, imageData);
        _cache.Remove($"ImageData-{eventId}");
    }
}