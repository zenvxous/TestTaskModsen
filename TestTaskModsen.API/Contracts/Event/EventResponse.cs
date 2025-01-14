using TestTaskModsen.API.Contracts.Registration;

namespace TestTaskModsen.API.Contracts.Event;

public record EventResponse(
    Guid Id,
    string Title,
    string Description,
    DateTime StartDate,
    DateTime EndDate,
    string Location,
    string Category,
    int Capacity,
    List<RegistrationResponse> Registrations);