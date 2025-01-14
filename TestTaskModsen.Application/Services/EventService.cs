using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Interfaces.Services;

namespace TestTaskModsen.Application.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _repository;

    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> IsEventExistsAsync(Guid eventId)
    {
        try
        {
            await _repository.GetByIdAsync(eventId);
            return true;
        }
        catch
        {
            return false;
        }
    }
}