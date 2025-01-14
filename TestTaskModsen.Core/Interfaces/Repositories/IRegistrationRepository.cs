using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Repositories;

public interface IRegistrationRepository
{
    Task RegisterUserToEventAsync(Guid userId, Guid eventId);
    Task UnregisterUserFromEventAsync(Guid userId, Guid eventId);
    Task<Registration> GetByUserAndEventIdAsync(Guid userId, Guid eventId);
}