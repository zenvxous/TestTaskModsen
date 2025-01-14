using System.IdentityModel.Tokens.Jwt;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using TestTaskModsen.Application.Validators;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Interfaces.Services;
using TestTaskModsen.Core.Interfaces.Authentication;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(
        IUserRepository userRepository, 
        IJwtTokenService jwtTokenService,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
        _passwordHasher = passwordHasher;
    }

    public async Task RegisterUser(string email, string password, string firstName, string lastName)
    {
        var passwordHash = _passwordHasher.HashPassword(password);
        
        var user = new User(Guid.NewGuid(), firstName, lastName, email, passwordHash, UserRole.User, []);

        var userValidator = new UserValidator();
        await userValidator.ValidateAndThrowAsync(user);

        try
        {
            await _userRepository.GetByEmailAsync(user.Email);
        }
        catch
        {
            await _userRepository.CreateAsync(user);
        }
    }

    public async Task<TokenResponse> Login(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (!_passwordHasher.VerifyPasswordHash(password, user.PasswordHash))
            throw new UnauthorizedAccessException("Incorrect password.");

        return await _jwtTokenService.GenerateTokens(user);
    }

    public Guid GetCurrentUserId(HttpContext context)
    {
        var accessToken = context.Request.Cookies.FirstOrDefault(x => x.Key == "_at").Value;
        
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(accessToken);
        
        return Guid.Parse(jwtToken.Claims.FirstOrDefault(x => x.Type == "sub")!.Value);
    }

    public async Task<User> GetUserById(Guid userId)
    {
        return await _userRepository.GetByIdAsync(userId);
    }

    public async Task<PagedResult<User>> GetUsersByEventId(Guid eventId, int pagenumber, int pageSize)
    {
        return await _userRepository.GetByEventIdAsync(eventId, pagenumber, pageSize);
    }

    public async Task UpdateUserRole(Guid userId, UserRole role)
    {
        await _userRepository.UpdateRoleAsync(userId, role);
    }
}