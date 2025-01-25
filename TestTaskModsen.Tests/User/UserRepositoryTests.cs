using Microsoft.EntityFrameworkCore;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Persistence;
using TestTaskModsen.Persistence.Entities;
using TestTaskModsen.Persistence.Mappers;
using TestTaskModsen.Persistence.Repositories;

namespace TestTaskModsen.Tests.User;

public class UserRepositoryTests : IDisposable
{
    private const string CONNECTION_STRING = "User ID=postgres;Password=postgres;Host=127.0.0.1;Port=5433;Database=TestTaskModsen";
    
    private readonly AppDbContext _context;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(CONNECTION_STRING)
            .Options;
        
        _context = new AppDbContext(options);
        _context.Database.EnsureDeleted();
        _context.Database.Migrate();
            
        _repository = new UserRepository(_context, new UserMapper());
    }
    
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Fact]
    public async Task GetByIdAsync_UserExists_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userEntity = new UserEntity
        {
            Id = userId,
            FirstName = "Test",
            LastName = "User",
            Email = "test@user.com",
            PasswordHash = "password",
            Role = UserRole.User,
            Registrations = []
        };
        
        var eventEntity = new EventEntity
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            Description = "Test",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(1),
            Location = "Test",
            Category = EventCategory.Other,
            Capacity = 1,
            Registrations = [],
            ImageData = []
        };
        
        var registrationEntity = new RegistrationEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            User = userEntity,
            EventId = eventEntity.Id,
            Event = eventEntity,
            RegistrationDate = DateTime.UtcNow
        };
        
        userEntity.Registrations.Add(registrationEntity);
        eventEntity.Registrations.Add(registrationEntity);
        
        await _context.Users.AddAsync(userEntity);
        await _context.Events.AddAsync(eventEntity);
        await _context.Registrations.AddAsync(registrationEntity);
        await _context.SaveChangesAsync();
        
        // Act
        var user = await _repository.GetByIdAsync(userId);
        
        // Assert
        Assert.Equal(userId, user.Id);
        Assert.Equal(userEntity.FirstName, user.FirstName);
        Assert.Equal(userEntity.LastName, user.LastName);
        Assert.Equal(userEntity.Email, user.Email);
        Assert.Equal(userEntity.PasswordHash, user.PasswordHash);
        Assert.Equal(userEntity.Role, user.Role);
        Assert.NotEmpty(user.Registrations);
        Assert.Equal(userEntity.Registrations.Count, user.Registrations.Count);
        Assert.Equal(userEntity.Registrations[0].UserId, user.Id);
        Assert.Equal(userEntity.Registrations[0].EventId, eventEntity.Id);
    }

    [Fact]
    public async Task GetByIdAsync_UserDoesNotExist_ThrowException()
    {
        // Arrange
        var userId = Guid.NewGuid();
        
        // Act & Assert
        await Assert.ThrowsAsync<Exception>(async () =>  await _repository.GetByIdAsync(userId));
    }

    [Fact]
    public async Task UpdateRoleAsync_UserExists_UpdatesRole()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userEntity = new UserEntity
        {
            Id = userId,
            FirstName = "Test",
            LastName = "User",
            Email = "test@user.com",
            PasswordHash = "password",
            Role = UserRole.User,
            Registrations = []
        };
        
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
        
        var newRole = UserRole.Admin;
        
        // Act
        await _repository.UpdateRoleAsync(userId, newRole);
        var user = await _repository.GetByIdAsync(userId);
        
        // Assert
        Assert.Equal(userId, user.Id);
        Assert.Equal(userEntity.FirstName, user.FirstName);
        Assert.Equal(userEntity.LastName, user.LastName);
        Assert.Equal(userEntity.Email, user.Email);
        Assert.Equal(userEntity.PasswordHash, user.PasswordHash);
        Assert.Equal(newRole, user.Role);
    }
}