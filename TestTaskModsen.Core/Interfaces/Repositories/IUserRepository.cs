using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByEmailAsync(string email);
    Task<PagedResult<User>> GetByEventIdAsync(Guid eventId, int pageNumber, int pageSize);
    Task CreateAsync(User user);
}