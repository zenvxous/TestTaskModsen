using TestTaskModsen.Core.Models;
using Microsoft.AspNetCore.Http;

namespace TestTaskModsen.Core.Interfaces.Services;

public interface IJwtTokenService
{
    Task<TokenResponse> GenerateTokens(User user);
    Task<TokenResponse> UpdateTokens(HttpContext context);
}
