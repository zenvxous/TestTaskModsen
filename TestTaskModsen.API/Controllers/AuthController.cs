using Microsoft.AspNetCore.Mvc;
using TestTaskModsen.Core.Interfaces.Services;

namespace TestTaskModsen.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(IHttpContextAccessor httpContextAccessor, IJwtTokenService jwtTokenService)
    {
        _httpContextAccessor = httpContextAccessor;
        _jwtTokenService = jwtTokenService;
    }

    [HttpGet("update")]
    public async Task<IActionResult> UpdateTokens()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            throw new Exception("HTTP context is not available.");
        
        var tokens = await _jwtTokenService.UpdateTokens(context);
        
        context.Response.Cookies.Append("_at", tokens.AccessToken);
        context.Response.Cookies.Append("_rt", tokens.RefreshToken);

        return Ok();
    }
}