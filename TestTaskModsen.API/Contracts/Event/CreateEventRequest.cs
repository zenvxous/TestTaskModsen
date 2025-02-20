using System.ComponentModel.DataAnnotations;
using TestTaskModsen.Core.Enums;

namespace TestTaskModsen.API.Contracts.Event;

public record CreateEventRequest(
    [Required] string Title,
    [Required] string Description,
    [Required] DateTime StartDate,
    [Required] DateTime EndDate,
    [Required] string Location,
    [Required] EventCategory Category,
    [Required] int Capacity);
