using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.Persistence.Entities;

public class UserEntity
{
    public Guid Id { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string PasswordHash { get; set; } = string.Empty;
    
    public UserRole Role { get; set; }

    public List<RegistrationEntity> Registrations { get; set; } = [];
}