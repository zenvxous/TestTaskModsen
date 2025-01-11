using TestTaskModsen.Core.Models;

namespace TestTaskModsen.API.Contracts.User;

public record class UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    List<Registration> Registrations);