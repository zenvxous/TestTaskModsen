using Microsoft.EntityFrameworkCore;
using TestTaskModsen.Core.Interfaces.Mappers;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Models;
using TestTaskModsen.Persistence.Entities;

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

    public async Task<User> GetByIdAsync(Guid id)
    {
        var userEntity = await _context.Users
            .Include(u => u.Registrations)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
        
        return _mapper.Map(userEntity);
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var userEntity = await _context.Users
            .Include(u => u.Registrations)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
        
        return _mapper.Map(userEntity);
    }

    public async Task<List<User>> GetByEventIdAsync(Guid eventId)
    {
        var userEntities = await _context.Registrations
            .AsNoTracking()
            .Where(r => r.EventId == eventId)
            .Select(r => r.User)
            .ToListAsync();
        
        return _mapper.Map(userEntities);
    }

    public async Task CreateAsync(User user)
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
        
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }
}