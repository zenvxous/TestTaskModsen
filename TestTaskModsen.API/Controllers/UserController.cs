using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestTaskModsen.API.Contracts.Registration;
using TestTaskModsen.API.Contracts.User;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Interfaces.Services;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("{id::guid}")]
    public async Task<ActionResult<UserResponse>> GetUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserById(id, cancellationToken);
        
        var response = new UserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role.ToString(),
            user.Registrations
                .Select(r => new RegistrationResponse(
                    r.Id,
                    r.UserId,
                    r.EventId,
                    r.RegistrationDate))
                .ToList());
        
        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    public ActionResult<Guid> GetCurrentUserId()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            throw new InvalidOperationException("HTTP context is not available.");
        
        var id = _userService.GetCurrentUserId(context);
        
        return Ok(id);
    }

    [HttpGet("events/{eventId::guid}")]
    public async Task<ActionResult<PagedResult<UserResponse>>> GetUsersByEventId(
        Guid eventId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var users = await _userService.GetUsersByEventId(eventId, pageNumber, pageSize, cancellationToken);

        var response = new PagedResult<UserResponse>(
            users.Items
                .Select(u => new UserResponse(
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.Email,
                        u.Role.ToString(),
                        u.Registrations
                            .Select(r => new RegistrationResponse(
                                r.Id,
                                r.UserId,
                                r.EventId,
                                r.RegistrationDate))
                            .ToList())),
                    users.TotalItems,
                    users.PageNumber,
                    users.PageSize);
        
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request, CancellationToken cancellationToken)
    {
        await _userService.RegisterUser(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            cancellationToken);
        
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request, CancellationToken cancellationToken)
    {
        var tokenResponse = await _userService.Login(request.Email, request.Password, cancellationToken);
        
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            throw new InvalidOperationException("HTTP context is not available.");
        
        context.Response.Cookies.Append("_at", tokenResponse.AccessToken);
        context.Response.Cookies.Append("_rt", tokenResponse.RefreshToken);
        
        return Ok();
    }

    [HttpPut("{id::guid}/role")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateRole(Guid id, [FromQuery] int roleNumber, CancellationToken cancellationToken)
    {
        if (!Enum.IsDefined(typeof(UserRole), roleNumber))
            return BadRequest();

        var role = (UserRole)roleNumber;
        
        await _userService.UpdateUserRole(id, role, cancellationToken);
        
        return Ok();
    }
}