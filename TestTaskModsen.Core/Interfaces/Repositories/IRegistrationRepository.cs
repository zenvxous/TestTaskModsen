namespace TestTaskModsen.Core.Interfaces.Repositories;

public interface IRegistrationRepository
{
    Task RegisterUserToEventAsync(Guid userId, Guid eventId);
    Task UnregisterUserFromEventAsync(Guid userId, Guid eventId);
}