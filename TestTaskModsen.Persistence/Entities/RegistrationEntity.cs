namespace TestTaskModsen.Persistence.Entities;

public class RegistrationEntity
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    public UserEntity User { get; set; }
    
    public Guid EventId { get; set; }
    
    public EventEntity Event { get; set; }
    
    public DateTime RegistrationDate { get; set; }
}