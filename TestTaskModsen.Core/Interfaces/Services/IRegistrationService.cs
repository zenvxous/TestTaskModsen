using Microsoft.AspNetCore.Http;

namespace TestTaskModsen.Core.Interfaces.Services;

public interface IRegistrationService
{
    Task RegisterUserToEvent(HttpContext context, Guid eventId);
    Task UnregisterUserToEvent(HttpContext context, Guid eventId);
}