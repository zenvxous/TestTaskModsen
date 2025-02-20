using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken);
    Task<PagedResult<User>> GetByEventIdAsync(Guid eventId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task UpdateRoleAsync(Guid userId, UserRole role, CancellationToken cancellationToken);
    Task CreateAsync(User user, CancellationToken cancellationToken);
}