namespace TestTaskModsen.Core.Interfaces.Services;

public interface IEventService
{
    Task<bool> IsEventExistsAsync(Guid eventId);
}