using Microsoft.EntityFrameworkCore;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Entities;

namespace TestTaskModsen.Persistence.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly AppDbContext _context;

    public RefreshTokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(string Token, DateTime Expiration)> GetByUserIdAsync(Guid userId)
    {
        var tokenEntity = await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.UserId == userId);

        if (tokenEntity is null)
            throw new Exception("Token not found");
        
        return (tokenEntity.Token, tokenEntity.Expiration);
    }

    public async Task<(string Token, DateTime Expiration)> GetByTokenAsync(string token)
    {
        var tokenEntity = await _context.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == token);
        
        if (tokenEntity is null)
            throw new Exception("Token not found");
        
        return (tokenEntity.Token, tokenEntity.Expiration);
    }

    public async Task CreateAsync(User user, string token, DateTime expiration)
    {
        var userEntity = await _context.Users.FindAsync(user.Id);

        if (userEntity is null)
            throw new Exception("User not found");
        
        var existingTokens = _context.RefreshTokens.Where(t => t.UserId == userEntity.Id);
        _context.RefreshTokens.RemoveRange(existingTokens);
        
        var tokenEntity = new RefreshTokenEntity
        {
            UserId = userEntity.Id,
            User = userEntity,
            Token = token,
            Expiration = expiration
        };
        
        await _context.RefreshTokens.AddAsync(tokenEntity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByTokenAsync(string token)
    {
        var tokenEntity = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == token);

        if (tokenEntity is null)
            throw new Exception("Token not found");
        
        _context.RefreshTokens.Remove(tokenEntity);
        await _context.SaveChangesAsync();
    }
}