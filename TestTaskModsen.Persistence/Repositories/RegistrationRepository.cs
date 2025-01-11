using Microsoft.EntityFrameworkCore;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Persistence.Entities;

namespace TestTaskModsen.Persistence.Repositories;

public class RegistrationRepository : IRegistrationRepository
{
    private readonly AppDbContext _context;

    public RegistrationRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task RegisterUserToEventAsync(Guid userId, Guid eventId)
    {
        var userEntity = await _context.Users.FindAsync(userId);
        var eventEntity = await _context.Events.FindAsync(eventId);
        
        if (userEntity is null || eventEntity is null)
            throw new Exception("User or event not found");
        
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
        
        if (registrationEntity is null)
            throw new Exception("Registration not found");
        
        _context.Registrations.Remove(registrationEntity);
        
        await _context.SaveChangesAsync();
    }
}
