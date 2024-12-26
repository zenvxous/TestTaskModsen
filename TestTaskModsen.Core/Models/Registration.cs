namespace TestTaskModsen.Core.Models;

public class Registration
{
    public Registration(Guid id, User user, Event @event, DateTime registrationDate)
    {
        Id = id;
        UserId = user.Id;
        User = user;
        EventId = @event.Id;
        Event = @event;
        RegistrationDate = registrationDate;
    }
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public User User { get; private set; }
    public Guid EventId { get; private set; }
    public Event Event { get; private set; }
    public DateTime RegistrationDate { get; private set; }
}