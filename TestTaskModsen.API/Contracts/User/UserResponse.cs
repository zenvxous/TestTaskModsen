using TestTaskModsen.API.Contracts.Registration;

namespace TestTaskModsen.API.Contracts.User;

public record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string Role,
    List<RegistrationResponse> Registrations);