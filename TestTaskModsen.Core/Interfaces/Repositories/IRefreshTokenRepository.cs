using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    Task<(string Token, DateTime Expiration)> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken);
    Task<(string Token, DateTime Expiration)> GetByTokenAsync(string token, CancellationToken cancellationToken);
    Task CreateAsync(User user, string token, DateTime expiration, CancellationToken cancellationToken);
    Task DeleteByTokenAsync(string token, CancellationToken cancellationToken);
}