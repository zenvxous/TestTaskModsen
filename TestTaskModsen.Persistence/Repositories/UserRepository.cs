using Microsoft.EntityFrameworkCore;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Interfaces.Mappers;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Entities;
using TestTaskModsen.Persistence.Extensions;

namespace TestTaskModsen.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper<UserEntity, User> _mapper;

    public UserRepository(AppDbContext context, IMapper<UserEntity, User> mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var userEntity = await _context.Users
            .Include(u => u.Registrations)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        
        if (userEntity is null)
            throw new KeyNotFoundException("User not found");
        
        return _mapper.Map(userEntity);
    }

    public async Task<User> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var userEntity = await _context.Users
            .Include(u => u.Registrations)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        
        if (userEntity is null)
            throw new KeyNotFoundException("User not found");
        
        return _mapper.Map(userEntity);
    }

    public async Task<PagedResult<User>> GetByEventIdAsync(Guid eventId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var userEntities = await _context.Registrations
            .AsNoTracking()
            .Where(r => r.EventId == eventId)
            .Select(r => r.User)
            .ToPagedResultAsync(pageNumber, pageSize, cancellationToken);
        
        return _mapper.Map(userEntities);
    }

    public async Task UpdateRoleAsync(Guid userId, UserRole role, CancellationToken cancellationToken)
    {
        var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
        if (userEntity is null)
            throw new KeyNotFoundException("User not found");
        
        userEntity.Role = role;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateAsync(User user, CancellationToken cancellationToken)
    {
        var userEntity = new UserEntity
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            Role = user.Role,
            Registrations = []
        };
        
        await _context.Users.AddAsync(userEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}