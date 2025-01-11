using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TestTaskModsen.Application.Authentication;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Interfaces.Services;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Application.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IUserRepository _userRepository;
    private readonly JwtOptions _jwtSettings;


    public JwtTokenService(IOptions<JwtOptions> jwtSettings, IRefreshTokenRepository refreshTokenRepository, IUserRepository userRepository)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userRepository = userRepository;
        _jwtSettings = jwtSettings.Value;
    }
    
    public async Task<TokenResponse> GenerateTokens(User user)
    {
        var accessToken = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        var tokens = new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            Expiration = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        };
        
        await _refreshTokenRepository.CreateAsync(user, tokens.RefreshToken, tokens.Expiration);
        
        return tokens;
    }

    public async Task<TokenResponse> UpdateTokens(HttpContext context)
    {
        var accessToken = context.Request.Cookies.FirstOrDefault(x => x.Key == "_at").Value;
        var refreshToken = context.Request.Cookies.FirstOrDefault(x => x.Key == "_rt").Value;
        
        var token = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (token.Expiration < DateTime.UtcNow)
            throw new Exception("The refresh token has expired");
        
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(accessToken);
        
        var userId = Guid.Parse(jwtToken.Claims.FirstOrDefault(x => x.Type == "sub")!.Value);
        var user = await _userRepository.GetByIdAsync(userId);
        
        return await GenerateTokens(user);
    }
    
    private string GenerateAccessToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }
}