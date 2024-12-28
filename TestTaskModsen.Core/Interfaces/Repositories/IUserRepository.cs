using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByEmailAsync(string email); 
    Task<List<User>> GetByEventIdAsync(Guid eventId);
    Task CreateAsync(User user);
}