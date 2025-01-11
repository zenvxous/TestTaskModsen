using Microsoft.AspNetCore.Http;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Core.Interfaces.Services;

public interface IUserService
{
    Task RegisterUser(string email, string password, string firstName, string lastName);
    Task<TokenResponse> Login(string email, string password);
    Guid GetCurrentUserId(HttpContext context);
    Task<User> GetUserById(Guid userId);
    Task UpdateUserRole(Guid userId, UserRole role);
}