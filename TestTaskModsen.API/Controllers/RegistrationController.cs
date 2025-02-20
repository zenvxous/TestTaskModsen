using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestTaskModsen.Core.Interfaces.Services;

namespace TestTaskModsen.API.Controllers;

[ApiController]
[Route("api/registrations")]
public class RegistrationController : ControllerBase
{
    private readonly IRegistrationService _registrationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public RegistrationController(IRegistrationService registrationService, IHttpContextAccessor httpContextAccessor)
    {
        _registrationService = registrationService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost("{eventId:guid}")]
    [Authorize]
    public async Task<IActionResult> RegisterUserToEvent(Guid eventId, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            throw new InvalidOperationException("HTTP context is not available.");
        
        await _registrationService.RegisterUserToEvent(context, eventId, cancellationToken);
        
        return Ok();
    }

    [HttpDelete("{eventId:guid}")]
    [Authorize]
    public async Task<IActionResult> UnregisterUserToEvent(Guid eventId, CancellationToken cancellationToken)
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            throw new InvalidOperationException("HTTP context is not available.");
        
        await _registrationService.UnregisterUserToEvent(context, eventId, cancellationToken);
        
        return Ok();
    }
}