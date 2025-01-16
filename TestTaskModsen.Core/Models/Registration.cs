namespace TestTaskModsen.Core.Models;

public class Registration
{
    public Registration(Guid id, Guid userId, Guid eventId, DateTime registrationDate)
    {
        Id = id;
        UserId = userId;
        EventId = eventId;
        RegistrationDate = registrationDate;
    }
    
    public Guid Id { get; private set; }
    
    public Guid UserId { get; private set; }
    
    public Guid EventId { get; private set; }
    
    public DateTime RegistrationDate { get; private set; }
}