using TestTaskModsen.Core.Interfaces;
using TestTaskModsen.Core.Interfaces.Mappers;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Entities;

namespace TestTaskModsen.Persistence.Mappers;

public class RegistrationMapper : IMapper<RegistrationEntity, Registration>
{
    public Registration Map(RegistrationEntity entity) =>
        new Registration
        (
            entity.Id,
            new UserMapper().Map(entity.User),
            new EventMapper().Map(entity.Event),
            entity.RegistrationDate
        );
    
    public List<Registration> Map(IEnumerable<RegistrationEntity> entities) => entities.Select(Map).ToList();
}