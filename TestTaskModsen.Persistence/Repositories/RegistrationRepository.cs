using Microsoft.EntityFrameworkCore;
using TestTaskModsen.Core.Interfaces.Mappers;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Entities;

namespace TestTaskModsen.Persistence.Repositories;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper<RegistrationEntity, Registration> _mapper;

    public RegistrationRepository(AppDbContext context, IMapper<RegistrationEntity, Registration> mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task RegisterUserToEventAsync(Guid userId, Guid eventId)
    {
        var userEntity = await _context.Users.FindAsync(userId);
        var eventEntity = await _context.Events.FindAsync(eventId);
        
        var registration = new RegistrationEntity
        {
            Id = Guid.NewGuid(),
            UserId = userEntity.Id,
            User = userEntity,
            EventId = eventEntity.Id,
            Event = eventEntity,
            RegistrationDate = DateTime.UtcNow,
        };
        
        await _context.Registrations.AddAsync(registration);
        await _context.SaveChangesAsync();
    }

    public async Task UnregisterUserFromEventAsync(Guid userId, Guid eventId)
    {
        var registrationEntity = await _context.Registrations
            .FirstOrDefaultAsync(r => r.UserId == userId && r.EventId == eventId);
        
        _context.Registrations.Remove(registrationEntity);
        
        await _context.SaveChangesAsync();
    }
}