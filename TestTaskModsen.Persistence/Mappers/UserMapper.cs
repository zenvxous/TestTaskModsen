using TestTaskModsen.Core.Interfaces.Mappers;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Entities;

namespace TestTaskModsen.Persistence.Mappers;

public class UserMapper : IMapper<UserEntity, User>
{
    public User Map(UserEntity entity) =>
        new User
        (
            entity.Id,
            entity.FirstName,
            entity.LastName,
            entity.Email,
            entity.PasswordHash,
            entity.Role,
            entity.Registrations.Select(new RegistrationMapper().Map).ToList()
        );
    
    public List<User> Map(IEnumerable<UserEntity> entities) => entities.Select(Map).ToList();
    
    public PagedResult<User> Map(PagedResult<UserEntity> pagedResult) => new PagedResult<User>(
        pagedResult.Items.Select(Map),
        pagedResult.TotalItems,
        pagedResult.PageNumber,
        pagedResult.PageSize);
}