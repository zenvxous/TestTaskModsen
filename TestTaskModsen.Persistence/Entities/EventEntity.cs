using TestTaskModsen.Core.Enums;

namespace TestTaskModsen.Persistence.Entities;

public class EventEntity
{
    public Guid Id { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public string Location { get; private set; } = string.Empty;
    
    public EventCategory Category { get; set; }
    
    public int Capacity { get; set; }
    
    public List<RegistrationEntity> Registrations { get; set; } = [];
    
    public byte[] ImageData { get; set; }
}