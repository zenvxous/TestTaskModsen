using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestTaskModsen.API.Contracts.Event;
using TestTaskModsen.API.Contracts.Registration;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Interfaces.Services;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.API.Controllers;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;

    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    private EventResponse CreateEventResponse(Event @event)
    {
        return new EventResponse(
            @event.Id,
            @event.Title,
            @event.Description,
            @event.StartDate,
            @event.EndDate,
            @event.Location,
            @event.Category.ToString(),
            @event.Capacity,
            @event.Registrations
                .Select(r => new RegistrationResponse(
                    r.Id,
                    r.UserId,
                    r.EventId,
                    r.RegistrationDate))
                .ToList());
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<EventResponse>>> GetAllEvents([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var events = await _eventService.GetAllEvents(pageNumber, pageSize, cancellationToken);
        
        var response = new PagedResult<EventResponse>(
            events.Items
                .Select(CreateEventResponse),
            events.TotalItems,
            events.PageNumber,
            events.PageSize);
        
        return Ok(response);
    }

    [HttpGet("{id::guid}")]
    public async Task<ActionResult<EventResponse>> GetEventById(Guid id, CancellationToken cancellationToken)
    {
        var @event = await _eventService.GetEventById(id, cancellationToken);

        var response = CreateEventResponse(@event);
        
        return Ok(response);
    }

    [HttpGet("title/{title}")]
    public async Task<ActionResult<EventResponse>> GetEventByTitle(string title, CancellationToken cancellationToken)
    {
        var @event = await _eventService.GetEventByTitle(title, cancellationToken);
        
        var response = CreateEventResponse(@event);
        
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request, CancellationToken cancellationToken)
    {
        await _eventService.CreateEvent(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            request.StartDate,
            request.EndDate,
            request.Location,
            request.Category,
            request.Capacity,
            cancellationToken);
        
        return Ok();
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] UpdateEventRequest request, CancellationToken cancellationToken)
    {
        await _eventService.UpdateEvent(
            id,
            request.Title,
            request.Description,
            request.StartDate,
            request.EndDate,
            request.Location,
            request.Category,
            request.Capacity,
            cancellationToken);
        
        return Ok();
    }

    [HttpDelete("{id::guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteEventById(Guid id, CancellationToken cancellationToken)
    {
        await _eventService.DeleteEvent(id, cancellationToken);
        
        return Ok();
    }

    [HttpGet("filters")]
    public async Task<ActionResult<PagedResult<EventResponse>>> GetEventsByFilters(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null,
        [FromQuery] string? location = null,
        [FromQuery] EventCategory? category = null,
        CancellationToken cancellationToken = default)
    {
        var events = await _eventService.GetEventsByFilter(
            pageNumber,
            pageSize,
            startDate,
            endDate,
            location,
            category,
            cancellationToken);
        
        var response = new PagedResult<EventResponse>(
            events.Items
                .Select(CreateEventResponse),
            events.TotalItems,
            events.PageNumber,
            events.PageSize);
        
        return Ok(response);
    }

    [HttpPut("{id:guid}/image")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateEventImage(Guid id, IFormFile file, CancellationToken cancellationToken)
    {
        if (file.Length == 0)
            throw new FileLoadException("File is empty");
        
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream, cancellationToken);
        
        await _eventService.UpdateImageData(id, memoryStream.ToArray(), cancellationToken);
        
        return Ok();
    }

    [HttpGet("{id:guid}/image")]
    public async Task<IActionResult> GetImageById(Guid id, CancellationToken cancellationToken)
    {
        var imageData = await _eventService.GetImageData(id, cancellationToken);
        
        return File(imageData, "image/jpeg");
    }
}