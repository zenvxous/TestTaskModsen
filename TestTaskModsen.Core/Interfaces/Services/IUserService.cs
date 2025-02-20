using Microsoft.AspNetCore.Http;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Services;

public interface IUserService
{
    Task RegisterUser(string email, string password, string firstName, string lastName, CancellationToken cancellationToken);
    Task<TokenResponse> Login(string email, string password, CancellationToken cancellationToken);
    Guid GetCurrentUserId(HttpContext context);
    Task<User> GetUserById(Guid userId, CancellationToken cancellationToken);
    Task<PagedResult<User>> GetUsersByEventId(Guid eventId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task UpdateUserRole(Guid userId, UserRole role, CancellationToken cancellationToken);
}