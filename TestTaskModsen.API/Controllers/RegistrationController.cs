using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestTaskModsen.Core.Interfaces.Services;

namespace TestTaskModsen.API.Controllers;

[ApiController]
[Route("registration")]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RegistrationController(IRegistrationService registrationService, IHttpContextAccessor httpContextAccessor)
    {
        _registrationService = registrationService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("register/{eventId::guid}")]
    [Authorize]
    public async Task<IActionResult> RegisterUserToEvent(Guid eventId)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            throw new Exception("HTTP context is not available.");
        
        await _registrationService.RegisterUserToEvent(context, eventId);
        
        return Ok();
    }

    [HttpPost("unregister/{eventId::guid}")]
    [Authorize]
    public async Task<IActionResult> UnregisterUserToEvent(Guid eventId)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            throw new Exception("HTTP context is not available.");
        
        await _registrationService.UnregisterUserToEvent(context, eventId);
        
        return Ok();
    }
}