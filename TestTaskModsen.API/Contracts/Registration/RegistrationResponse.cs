namespace TestTaskModsen.API.Contracts.Registration;

public record class RegistrationResponse(
    Guid Id,
    Guid UserId,
    Guid eventId,
    DateTime RegistrationDate);