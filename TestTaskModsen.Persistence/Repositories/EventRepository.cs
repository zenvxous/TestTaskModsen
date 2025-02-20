using Microsoft.EntityFrameworkCore;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Interfaces.Mappers;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Entities;
using TestTaskModsen.Persistence.Extensions;

namespace TestTaskModsen.Persistence.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper<EventEntity, Event> _mapper;

    public EventRepository(AppDbContext context, IMapper<EventEntity, Event> mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<Event>> GetAllAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var eventEntities = await _context.Events
            .Include(e => e.Registrations)
            .AsNoTracking()
            .ToPagedResultAsync(pageNumber, pageSize, cancellationToken);
        
        return _mapper.Map(eventEntities);
    }

    public async Task<Event> GetByIdAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Registrations)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

        if (eventEntity is null)
            throw new KeyNotFoundException("Event not found");
        
        return _mapper.Map(eventEntity);
    }

    public async Task<Event> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Registrations)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Title == title, cancellationToken);
        
        if (eventEntity is null)
            throw new KeyNotFoundException("Event not found");
        
        return _mapper.Map(eventEntity);
    }

    public async Task<PagedResult<Event>> GetByFiltersAsync(
        int pageNumber,
        int pageSize,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? location = null,
        EventCategory? category = null,
        CancellationToken cancellationToken = default)
    {
        var eventEntities = await _context.Events
            .Include(e => e.Registrations)
            .AsNoTracking()
            .Where(e => 
                (!startDate.HasValue || e.StartDate >= startDate.Value) &&
                (!endDate.HasValue || e.EndDate <= endDate.Value) &&
                (string.IsNullOrEmpty(location) || e.Location.Contains(location)) &&
                (!category.HasValue || e.Category == category))
            .ToPagedResultAsync(pageNumber, pageSize, cancellationToken);
        
        return _mapper.Map(eventEntities);
    }
    
    public async Task CreateAsync(Event @event, CancellationToken cancellationToken)
    {
        var eventEntity = new EventEntity
        {
            Id = @event.Id,
            Title = @event.Title,
            Description = @event.Description,
            StartDate = @event.StartDate,
            EndDate = @event.EndDate,
            Location = @event.Location,
            Category = @event.Category,
            Capacity = @event.Capacity,
            Registrations = [],
            ImageData = @event.ImageData
        };
        
        await _context.Events.AddAsync(eventEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Event @event, CancellationToken cancellationToken)
    {
        var eventEntity = await _context.Events.FindAsync([@event.Id], cancellationToken);
        if (eventEntity == null)
            throw new KeyNotFoundException("Event not found");
        
        eventEntity.Title = @event.Title;
        eventEntity.Description = @event.Description;
        eventEntity.StartDate = @event.StartDate;
        eventEntity.EndDate = @event.EndDate;
        eventEntity.Location = @event.Location;
        eventEntity.Category = @event.Category;
        eventEntity.Capacity = @event.Capacity;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateImageDataAsync(Guid eventId, byte[] imageData, CancellationToken cancellationToken)
    {
        await _context.Events
            .Where(e => e.Id == eventId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(e => e.ImageData, imageData),
                cancellationToken);
    }

    public async Task DeleteAsync(Guid eventId, CancellationToken cancellationToken)
    {
        var eventEntity = await _context.Events.FindAsync([eventId], cancellationToken);
        if (eventEntity == null)
            throw new KeyNotFoundException("Event not found");
        
        _context.Events.Remove(eventEntity);
        await _context.SaveChangesAsync(cancellationToken);
    }
}