using Microsoft.AspNetCore.Http;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Interfaces.Services;

namespace TestTaskModsen.Application.Services;

public class RegistrationService : IRegistrationService
{
    private readonly IRegistrationRepository _registrationRepository;
    private readonly IUserService _userService;
    private readonly IEventService _eventService;

    public RegistrationService(
        IRegistrationRepository registrationRepository,
        IUserService userService,
        IEventService eventService)
    {
        _registrationRepository = registrationRepository;
        _userService = userService;
        _eventService = eventService;
    }

    private async Task<bool> IsUserRegisteredToEvent(Guid userId, Guid eventId, CancellationToken cancellationToken)
    {
        try
        {
            await _registrationRepository.GetByUserAndEventIdAsync(userId, eventId, cancellationToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> IsEventAvailable(Guid eventId, CancellationToken cancellationToken)
    {
        var @event = await _eventService.GetEventById(eventId, cancellationToken);

        if (@event.Capacity <= @event.Registrations.Count)
            return false;

        return true;
    }
    
    public async Task RegisterUserToEvent(HttpContext context, Guid eventId, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId(context);
        
        if(!await _eventService.IsEventExistsAsync(eventId, cancellationToken))
            throw new KeyNotFoundException($"Event with id {eventId} not found");
        
        if (await IsUserRegisteredToEvent(userId, eventId, cancellationToken))
            throw new InvalidDataException("User already registered");
        
        if(!await IsEventAvailable(eventId, cancellationToken))
            throw new ArgumentException($"Event with id {eventId} not available");
        
        await _registrationRepository.RegisterUserToEventAsync(userId, eventId, cancellationToken);
    }

    public async Task UnregisterUserToEvent(HttpContext context, Guid eventId, CancellationToken cancellationToken)
    {
        var userId = _userService.GetCurrentUserId(context);
        
        if(!await _eventService.IsEventExistsAsync(eventId, cancellationToken))
            throw new KeyNotFoundException($"Event with id {eventId} not found");
        
        if (!await IsUserRegisteredToEvent(userId, eventId, cancellationToken))
            throw new InvalidDataException("User doesn't registered");
        
        await _registrationRepository.UnregisterUserFromEventAsync(userId, eventId, cancellationToken);
    }
}
