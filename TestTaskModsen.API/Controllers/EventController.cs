using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestTaskModsen.API.Contracts.Event;
using TestTaskModsen.API.Contracts.Registration;
using TestTaskModsen.Core.Enums;
using TestTaskModsen.Core.Interfaces.Services;
using TestTaskModsen.Core.Models;

namespace TestTaskModsen.API.Controllers;

[ApiController]
[Route("event")]
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
    public async Task<ActionResult<PagedResult<EventResponse>>> GetAllEvents([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var events = await _eventService.GetAllEvents(pageNumber, pageSize);
        
        var response = new PagedResult<EventResponse>(
            events.Items
                .Select(CreateEventResponse),
            events.TotalItems,
            events.PageNumber,
            events.PageSize);
        
        return Ok(response);
    }

    [HttpGet("{eventId::guid}")]
    public async Task<ActionResult<EventResponse>> GetEventById(Guid eventId)
    {
        var @event = await _eventService.GetEventById(eventId);

        var response = CreateEventResponse(@event);
        
        return Ok(response);
    }

    [HttpGet("{eventTitle::alpha}")]
    public async Task<ActionResult<EventResponse>> GetEventByTitle(string eventTitle)
    {
        var @event = await _eventService.GetEventByTitle(eventTitle);
        
        var response = CreateEventResponse(@event);
        
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
    {
        var startDate = DateTime.Parse(request.StartDate).ToUniversalTime();
        var endDate = DateTime.Parse(request.EndDate).ToUniversalTime();
        var category = (EventCategory)request.Category;

        var @event = new Event(
            Guid.NewGuid(),
            request.Title,
            request.Description,
            startDate,
            endDate,
            request.Location,
            category,
            request.Capacity,
            [],
            []);
        
        await _eventService.CreateEvent(@event);
        
        return Ok();
    }

    [HttpPut]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventRequest request)
    {
        var startDate = DateTime.Parse(request.StartDate).ToUniversalTime();
        var endDate = DateTime.Parse(request.EndDate).ToUniversalTime();
        var category = (EventCategory)request.Category;
        
        var @event = new Event(
            request.Id,
            request.Title,
            request.Description,
            startDate,
            endDate,
            request.Location,
            category,
            request.Capacity,
            [],
            []);
        
        await _eventService.UpdateEvent(@event);
        
        return Ok();
    }

    [HttpDelete("{eventId::guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> DeleteEventById(Guid eventId)
    {
        await _eventService.DeleteEvent(eventId);
        
        return Ok();
    }

    [HttpGet("filters")]
    public async Task<ActionResult<PagedResult<EventResponse>>> GetEventsByFilters(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? startDate = null,
        [FromQuery] string? endDate = null,
        [FromQuery] string? location = null,
        [FromQuery] int? category = null)
    {
        DateTime? parsedStartDate = string.IsNullOrEmpty(startDate) ? null : DateTime.Parse(startDate).ToUniversalTime();
        DateTime? parsedEndDate = string.IsNullOrEmpty(endDate) ? null : DateTime.Parse(endDate).ToUniversalTime();
        
        EventCategory? parsedCategory = category.HasValue ? (EventCategory)category.Value : null;
        
        var events = await _eventService.GetEventsByFilter(
            pageNumber,
            pageSize,
            parsedStartDate,
            parsedEndDate,
            location,
            parsedCategory);
        
        var response = new PagedResult<EventResponse>(
            events.Items
                .Select(CreateEventResponse),
            events.TotalItems,
            events.PageNumber,
            events.PageSize);
        
        return Ok(response);
    }

    [HttpPut("image/{eventId::guid}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> UpdateEventImage(Guid eventId, IFormFile file)
    {
        if (file.Length == 0)
            throw new Exception("File is empty");
        
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        
        await _eventService.UpdateImageData(eventId, memoryStream.ToArray());
        
        return Ok();
    }

    [HttpGet("image/{eventId::guid}")]
    public async Task<IActionResult> GetImageById(Guid eventId)
    {
        var imageData = await _eventService.GetImageData(eventId);
        
        return File(imageData, "image/jpeg");
    }
}