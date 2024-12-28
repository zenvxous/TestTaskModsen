using TestTaskModsen.Core.Interfaces;
using TestTaskModsen.Core.Interfaces.Mappers;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Entities;

namespace TestTaskModsen.Persistence.Mappers;

public class EventMapper : IMapper<EventEntity, Event>
{
    public Event Map(EventEntity entity) =>
        new Event
        (
            entity.Id,
            entity.Title,
            entity.Description,
            entity.StartDate,
            entity.EndDate,
            entity.Location,
            entity.Category,
            entity.Capacity,
            entity.Registrations.Select(new RegistrationMapper().Map).ToList(),
            entity.ImageData
        );

    public List<Event> Map(IEnumerable<EventEntity> entities) => entities.Select(Map).ToList();
}