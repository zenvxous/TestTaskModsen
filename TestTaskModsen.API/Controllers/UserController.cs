using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestTaskModsen.API.Contracts.User;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Interfaces.Services;


namespace TestTaskModsen.API.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor)
    {
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet("{userId::guid}")]
    public async Task<ActionResult<UserResponse>> GetUserAsync(Guid userId)
    {
        var user = await _userService.GetUserById(userId);
        
        var response = new UserResponse(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.Role.ToString(),
            user.Registrations);
        
        return Ok(response);
    }

    [HttpGet]
    [Authorize]
    public ActionResult<Guid> GetCurrentUserIdAsync()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            throw new Exception("HTTP context is not available.");
        
        var id = _userService.GetCurrentUserId(context);
        
        return Ok(id);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
    {
        await _userService.RegisterUser(request.Email, request.Password, request.FirstName, request.LastName);
        
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
    {
        var tokenResponse = await _userService.Login(request.Email, request.Password);
        
        var context = _httpContextAccessor.HttpContext;
        if (context == null)
            throw new Exception("HTTP context is not available.");
        
        context.Response.Cookies.Append("_at", tokenResponse.AccessToken);
        context.Response.Cookies.Append("_rt", tokenResponse.RefreshToken);
        
        return Ok();
    }

    [HttpPut("update-role/{userId:guid}/{roleNumber:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateRole(Guid userId, int roleNumber)
    {
        if (!Enum.IsDefined(typeof(UserRole), roleNumber))
            return BadRequest();

        var role = (UserRole)roleNumber;
        
        await _userService.UpdateUserRole(userId, role);
        
        return Ok();
    }
}