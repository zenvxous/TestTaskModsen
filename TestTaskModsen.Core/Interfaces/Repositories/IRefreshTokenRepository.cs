using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<(string Token, DateTime Expiration)> GetByUserIdAsync(Guid userId);
    Task<(string Token, DateTime Expiration)> GetByTokenAsync(string token);
    Task CreateAsync(User user, string token, DateTime expiration);
    Task DeleteByTokenAsync(string token);
}