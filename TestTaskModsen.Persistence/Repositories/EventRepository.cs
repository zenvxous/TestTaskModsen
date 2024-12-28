using Microsoft.EntityFrameworkCore;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Interfaces.Mappers;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Entities;

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

    public async Task<List<Event>> GetAllAsync()
    {
        var eventEntities = await _context.Events
            .Include(e => e.Registrations)
            .AsNoTracking()
            .ToListAsync();
        
        return _mapper.Map(eventEntities);
    }

    public async Task<Event> GetByIdAsync(Guid eventId)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Registrations)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == eventId);
        
        return _mapper.Map(eventEntity);
    }

    public async Task<Event> GetByTitleAsync(string title)
    {
        var eventEntity = await _context.Events
            .Include(e => e.Registrations)
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Title == title);
        
        return _mapper.Map(eventEntity);
    }

    public async Task<List<Event>> GetByFiltersAsync(
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? location = null,
        EventCategory? category = null)
    {
        var eventEntities = await _context.Events
            .Include(e => e.Registrations)
            .AsNoTracking()
            .Where(e => 
                (!startDate.HasValue || e.StartDate >= startDate.Value) &&
                (!endDate.HasValue || e.EndDate <= endDate.Value) &&
                (string.IsNullOrEmpty(location) || e.Location.Contains(location)) &&
                (!category.HasValue || e.Category == category))
            .ToListAsync();
        
        return _mapper.Map(eventEntities);
    }
    
    public async Task CreateAsync(Event @event)
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
        
        await _context.Events.AddAsync(eventEntity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Event @event)
    {
        await _context.Events
            .Where(e => e.Id == @event.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(e => e.Title, @event.Title)
                .SetProperty(e => e.Description, @event.Description)
                .SetProperty(e => e.StartDate, @event.StartDate)
                .SetProperty(e => e.EndDate, @event.EndDate)
                .SetProperty(e => e.Location, @event.Location)
                .SetProperty(e => e.Category, @event.Category)
                .SetProperty(e => e.Capacity, @event.Capacity));
    }

    public async Task UpdateImageDataAsync(Guid eventId, byte[] imageData)
    {
        await _context.Events
            .Where(e => e.Id == eventId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(e => e.ImageData, imageData));
    }

    public async Task DeleteAsync(Guid eventId)
    {
        await _context.Events
            .Where(e => e.Id == eventId)
            .ExecuteDeleteAsync();
    }
}