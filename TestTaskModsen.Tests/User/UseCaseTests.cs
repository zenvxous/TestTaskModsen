using Microsoft.AspNetCore.Http;
using Moq;
using TestTaskModsen.Application.Services;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Interfaces.Repositories;
using TestTaskModsen.Core.Interfaces.Services;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Tests.User;

public class UseCaseTests
{
    private readonly Mock<IRegistrationRepository> _registrationRepositoryMock;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IEventService> _eventServiceMock;
    private readonly RegistrationService _registrationService;
    private readonly HttpContext _context;
    private readonly Guid _eventId;
    private readonly Guid _userId;
    
    public UseCaseTests()
    {
        _registrationRepositoryMock = new Mock<IRegistrationRepository>();
        _userServiceMock = new Mock<IUserService>();
        _eventServiceMock = new Mock<IEventService>();
        _registrationService = new RegistrationService(
            _registrationRepositoryMock.Object,
            _userServiceMock.Object,
            _eventServiceMock.Object);
        
        _context = new DefaultHttpContext();
        _eventId = Guid.NewGuid();
        _userId = Guid.NewGuid();
    }

    [Fact]
    public async Task RegisterUserToEvent_ShouldRegisterSuccessfully_WhenAllConditionsAreMet()
    {
        // Arrange
        _userServiceMock.Setup(s => s.GetCurrentUserId(_context)).Returns(_userId);
        _eventServiceMock.Setup(s => s.IsEventExistsAsync(_eventId)).ReturnsAsync(true);
        _eventServiceMock.Setup(s => s.GetEventById(_eventId))
            .ReturnsAsync(new Event(_eventId, "test", "test", DateTime.UtcNow, DateTime.UtcNow.AddHours(1),"test", EventCategory.Other, 100, [], []));
        _registrationRepositoryMock.Setup(r => r.GetByUserAndEventIdAsync(_userId, _eventId)).ThrowsAsync(new Exception());
        
        // Act
        await _registrationService.RegisterUserToEvent(_context, _eventId);
        
        // Assert
        _registrationRepositoryMock.Verify(r => r.RegisterUserToEventAsync(_userId, _eventId), Times.Once);
    }

    [Fact]
    public async Task RegisterUserToEvent_ShouldThrowException_WhenEventDoesNotExist()
    {
        // Arrange
        _userServiceMock.Setup(s => s.GetCurrentUserId(_context)).Returns(_userId);
        _eventServiceMock.Setup(s => s.IsEventExistsAsync(_eventId)).ReturnsAsync(false);
        
        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _registrationService.RegisterUserToEvent(_context, _eventId));
    }

    [Fact]
    public async Task RegisterUserToEvent_ShouldThrowException_WhenUserAlreadyRegistered()
    {
        // Arrange
        _userServiceMock.Setup(s => s.GetCurrentUserId(_context)).Returns(_userId);
        _eventServiceMock.Setup(s => s.IsEventExistsAsync(_eventId)).ReturnsAsync(true);
        _registrationRepositoryMock.Setup(r => r.GetByUserAndEventIdAsync(_userId, _eventId)).ReturnsAsync(new Registration(Guid.NewGuid(), _userId, _eventId, DateTime.UtcNow));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _registrationService.RegisterUserToEvent(_context, _eventId));
    }

    [Fact]
    public async Task RegisterUserToEvent_ShouldThrowException_WhenEventIsNotAvailable()
    {
        // Arrange
        _userServiceMock.Setup(s => s.GetCurrentUserId(_context)).Returns(_userId);
        _eventServiceMock.Setup(s => s.IsEventExistsAsync(_eventId)).ReturnsAsync(true);
        _eventServiceMock.Setup(s => s.GetEventById(_eventId))
            .ReturnsAsync(new Event(_eventId, "test", "test", DateTime.UtcNow, DateTime.UtcNow.AddHours(1),"test", EventCategory.Other, 100, [], []));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _registrationService.RegisterUserToEvent(_context, _eventId));
    }
    
    [Fact]
    public async Task UnregisterUserToEvent_ShouldUnregisterSuccessfully_WhenAllConditionsAreMet()
    {
        // Arrange
        _userServiceMock.Setup(s => s.GetCurrentUserId(_context)).Returns(_userId);
        _eventServiceMock.Setup(s => s.IsEventExistsAsync(_eventId)).ReturnsAsync(true);
        _registrationRepositoryMock.Setup(r => r.GetByUserAndEventIdAsync(_userId, _eventId)).ReturnsAsync(new Registration(Guid.NewGuid(), _userId, _eventId, DateTime.UtcNow));

        // Act
        await _registrationService.UnregisterUserToEvent(_context, _eventId);

        // Assert
        _registrationRepositoryMock.Verify(r => r.UnregisterUserFromEventAsync(_userId, _eventId), Times.Once);
    }  
    
    [Fact]
    public async Task UnregisterUserToEvent_ShouldThrowException_WhenEventDoesNotExist()
    {
        // Arrange
        _userServiceMock.Setup(s => s.GetCurrentUserId(_context)).Returns(_userId);
        _eventServiceMock.Setup(s => s.IsEventExistsAsync(_eventId)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _registrationService.UnregisterUserToEvent(_context, _eventId));
    }
    
    [Fact]
    public async Task UnregisterUserToEvent_ShouldThrowException_WhenUserNotRegistered()
    {
        // Arrange
        _userServiceMock.Setup(s => s.GetCurrentUserId(_context)).Returns(_userId);
        _eventServiceMock.Setup(s => s.IsEventExistsAsync(_eventId)).ReturnsAsync(true);
        _registrationRepositoryMock.Setup(r => r.GetByUserAndEventIdAsync(_userId, _eventId)).ThrowsAsync(new Exception());

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _registrationService.UnregisterUserToEvent(_context, _eventId));
    }
}