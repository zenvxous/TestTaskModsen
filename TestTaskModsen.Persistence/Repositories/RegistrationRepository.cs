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

    public async Task RegisterUserToEventAsync(Guid userId, Guid eventId, CancellationToken cancellationToken)
    {
        var userEntity = await _context.Users.FindAsync(new object[] { userId }, cancellationToken);
        var eventEntity = await _context.Events.FindAsync(new object[] { eventId }, cancellationToken);
        
        if (userEntity is null || eventEntity is null)
            throw new KeyNotFoundException("User or event not found");
        
        var registration = new RegistrationEntity
        {
            Id = Guid.NewGuid(),
            UserId = userEntity.Id,
            User = userEntity,
            EventId = eventEntity.Id,
            Event = eventEntity,
            RegistrationDate = DateTime.UtcNow,
        };
        
        await _context.Registrations.AddAsync(registration, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UnregisterUserFromEventAsync(Guid userId, Guid eventId, CancellationToken cancellationToken)
    {
        var registrationEntity = await _context.Registrations
            .FirstOrDefaultAsync(r => r.UserId == userId && r.EventId == eventId, cancellationToken);
        
        if (registrationEntity is null)
            throw new KeyNotFoundException("Registration not found");
        
        _context.Registrations.Remove(registrationEntity);
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Registration> GetByUserAndEventIdAsync(Guid userId, Guid eventId, CancellationToken cancellationToken)
    {
        var registrationEntity = await _context.Registrations
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.UserId == userId && r.EventId == eventId, cancellationToken);

        if (registrationEntity is null)
            throw new KeyNotFoundException("Registration not found");
        
        return _mapper.Map(registrationEntity);
    }
}
