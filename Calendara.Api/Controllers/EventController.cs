using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Calendara.Application.Services;
using Calendara.Contracts.Responses;
using Calendara.Api.Mapping;
using Calendara.Contracts.Requests;

namespace Calendara.Api.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        public EventController()
        {

        }

        [HttpPost("api/events/")]
        public async Task<IActionResult> Create([FromBody]CreateEventRequest request)
        {
            var eventItem = request.MapToEvent();
            var result = await _eventService.CreateAsync(eventItem);
            return CreatedAtAction(nameof(GetById), new { id = eventItem.Id }, eventItem);
        }

        [HttpGet("api/events/{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var eventItem = await _eventService.GetByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            var response = eventItem.MapToResponse();
            return Ok(response);
        }

        [HttpGet("api/events/by-date")]
        public async Task<IActionResult> GetByDate(DateOnly date)
        {
            var eventItems = await _eventService.GetAllAsync();
            var response = eventItems
                .Where(x => x.DateOnly == date ||
                    (x.StartDateTime.HasValue && x.EndDateTime.HasValue &&
                     date.ToDateTime(TimeOnly.MinValue) >= x.StartDateTime.Value &&
                     date.ToDateTime(TimeOnly.MinValue) <= x.EndDateTime.Value))
                .Select(x => x.MapToResponse());
            return Ok(response);
        }

        [HttpGet("api/events/by-date-range")]
        public async Task<IActionResult> GetByDateRange(DateOnly startDate, DateOnly endDate)
        {
            var eventItems = await _eventService.GetAllAsync();
            var response = eventItems
                .Where(x =>
            (x.DateOnly.HasValue && x.DateOnly >= startDate && x.DateOnly <= endDate) || 
            (x.StartDateTime.HasValue && x.EndDateTime.HasValue &&
             x.StartDateTime.Value.Date <= endDate.ToDateTime(TimeOnly.MinValue) &&
             x.EndDateTime.Value.Date >= startDate.ToDateTime(TimeOnly.MinValue)))
                .Select(x => x.MapToResponse());
            return Ok(response);
        }

        [HttpGet("api/events")]
        public async Task<IActionResult> GetAll()
        {
            var eventItems = await _eventService.GetAllAsync();
            var response = eventItems.Select(x => x.MapToResponse());
            return Ok(response);
        }

        [HttpPut("api/events/{id:guid}")]
        public async Task<IActionResult> Update([FromBody]UpdateEventRequest request, [FromRoute] Guid id)
        {
            var eventItem = await _eventService.GetByIdAsync(id);
            eventItem = request.MapToEvent(id, eventItem);
            var updated = await _eventService.UpdateAsync(eventItem);
            if (updated is null)
            {
                return NotFound();
            }
            var response = updated.MapToResponse();
            return Ok(response);
        }

        [HttpDelete("api/events/{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var eventItem = await _eventService.GetByIdAsync(id);
            var deleted = await _eventService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }

    }
}
