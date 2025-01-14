using System.ComponentModel.DataAnnotations;

namespace TestTaskModsen.API.Contracts.Event;

public record CreateEventRequest(
    [Required] string Title,
    [Required] string Description,
    [Required] string StartDate,
    [Required] string EndDate,
    [Required] string Location,
    [Required] int Category,
    [Required] int Capacity);
