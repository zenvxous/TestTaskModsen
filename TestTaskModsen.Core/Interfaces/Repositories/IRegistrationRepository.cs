using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Repositories;

public interface IRegistrationRepository
{
    Task RegisterUserToEventAsync(Guid userId, Guid eventId, CancellationToken cancellationToken);
    Task UnregisterUserFromEventAsync(Guid userId, Guid eventId, CancellationToken cancellationToken);
    Task<Registration> GetByUserAndEventIdAsync(Guid userId, Guid eventId, CancellationToken cancellationToken);
}