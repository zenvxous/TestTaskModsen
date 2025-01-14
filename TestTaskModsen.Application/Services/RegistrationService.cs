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

    private async Task<bool> IsUserRegisteredToEvent(Guid userId, Guid eventId)
    {
        try
        {
            await _registrationRepository.GetByUserAndEventIdAsync(userId, eventId);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> IsEventAvailable(Guid eventId)
    {
        var @event = await _eventService.GetEventById(eventId);

        if (@event.Capacity <= @event.Registrations.Count) 
            return false;
        
        return true;
    }
    
    public async Task RegisterUserToEvent(HttpContext context, Guid eventId)
    {
        var userId = _userService.GetCurrentUserId(context);
        
        if(!await _eventService.IsEventExistsAsync(eventId))
            throw new Exception($"Event with id {eventId} not found");
        
        if (await IsUserRegisteredToEvent(userId, eventId))
            throw new Exception("User already registered");
        
        await _registrationRepository.RegisterUserToEventAsync(userId, eventId);
    }

    public async Task UnregisterUserToEvent(HttpContext context, Guid eventId)
    {
        var userId = _userService.GetCurrentUserId(context);
        
        if(!await _eventService.IsEventExistsAsync(eventId))
            throw new Exception($"Event with id {eventId} not found");
        
        if (!await IsUserRegisteredToEvent(userId, eventId))
            throw new Exception("User doesn't registered");
        
        await _registrationRepository.UnregisterUserFromEventAsync(userId, eventId);
    }
}