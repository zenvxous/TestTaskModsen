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
            entity.UserId,
            entity.EventId,
            entity.RegistrationDate
        );
    
    public List<Registration> Map(IEnumerable<RegistrationEntity> entities) => entities.Select(Map).ToList();
    
    public PagedResult<Registration> Map(PagedResult<RegistrationEntity> pagedResult) => new PagedResult<Registration>(
        pagedResult.Items.Select(Map),
        pagedResult.TotalItems,
        pagedResult.PageNumber,
        pagedResult.PageSize);
}