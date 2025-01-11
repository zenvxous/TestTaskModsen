using TestTaskModsen.Core.Enums;

namespace TestTaskModsen.Core.Models;

public class Event
{
    public Event(Guid id, string title, string description, DateTime startDate, DateTime endDate, string location,
        EventCategory category, int capacity, List<Registration> registrations, byte[] imageData)
    {
        Id = id;
        Title = title;
        Description = description;
        StartDate = startDate;
        EndDate = endDate;
        Location = location;
        Category = category;
        Capacity = capacity;
        Registrations = registrations;
        ImageData = imageData;
    }
    
    public Guid Id { get; private set; }
    
    public string Title { get; private set; }
    
    public string Description { get; private set; }
    
    public DateTime StartDate { get; private set; }
    
    public DateTime EndDate { get; private set; }
    
    public string Location { get; private set; }
    
    public EventCategory Category { get; private set; }
    
    public int Capacity { get; private set; }
    
    public List<Registration> Registrations { get; private set; }
    
    public byte[] ImageData { get; private set; }
}