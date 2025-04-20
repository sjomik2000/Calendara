using Calendara.Application.Models;
using Calendara.Contracts.Responses;
using Calendara.Contracts.Requests;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Calendara.Api.Mapping
{
    public static class ContractMapping
    {
        public static Event MapToEvent(this CreateEventRequest request)
        {
            if (request.AllDay == true)
            {
                return new Event
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    AllDay = request.AllDay,
                    DateOnly = request.DateOnly,
                    StartDateTime = null,
                    EndDateTime = null,
                    Description = request.Description ?? string.Empty,
                    Location = request.Location != null 
                        ? new GeoCoordinate(request.Location.Latitude, request.Location.Longitude)
                        : null
                };
            }
            else
            {
                return new Event
                {
                    Id = Guid.NewGuid(),
                    Title = request.Title,
                    AllDay = request.AllDay,
                    DateOnly = null,
                    StartDateTime = DateTime.SpecifyKind(request.StartDateTime.Value, DateTimeKind.Utc),
                    EndDateTime = DateTime.SpecifyKind(request.EndDateTime.Value, DateTimeKind.Utc),
                    Description = request.Description ?? string.Empty,
                    Location = request.Location != null
                        ? new GeoCoordinate(request.Location.Latitude, request.Location.Longitude)
                        : null
                };
            }
        }
        public static Event MapToEvent(this UpdateEventRequest request, Guid id, Event eventItem)
        {
            if (request.AllDay == true)
            {
                return new Event
                {
                    Id = id,
                    Title = request.Title ?? eventItem.Title,
                    AllDay = (bool)request.AllDay,
                    DateOnly = request.DateOnly ?? eventItem.DateOnly,
                    StartDateTime = null,
                    EndDateTime = null,
                    Description = request.Description is null &&
                               request.GetType().GetProperty(nameof(request.Description))?.GetValue(request) == null
                               ? eventItem.Description
                               : request.Description,
                    Location = request.Location is null && 
                               request.GetType().GetProperty(nameof(request.Location))?.GetValue(request) == null
                               ? eventItem.Location
                               : new GeoCoordinate(request.Location.Latitude, request.Location.Longitude)
                };
            }
            else if (request.AllDay is null && eventItem.AllDay == true)
            {
                return new Event
                {
                    Id = id,
                    Title = request.Title ?? eventItem.Title,
                    AllDay = eventItem.AllDay,
                    DateOnly = request.DateOnly ?? eventItem.DateOnly,
                    StartDateTime = null,
                    EndDateTime = null,
                    Description = request.Description is null &&
                               request.GetType().GetProperty(nameof(request.Description))?.GetValue(request) == null
                               ? eventItem.Description
                               : request.Description,
                    Location = request.Location is null &&
                               request.GetType().GetProperty(nameof(request.Location))?.GetValue(request) == null
                               ? eventItem.Location
                               : new GeoCoordinate(request.Location.Latitude, request.Location.Longitude)
                };
            }
            else if (request.AllDay == false)
            {
                return new Event
                {
                    Id = id,
                    Title = request.Title ?? eventItem.Title,
                    AllDay = (bool)request.AllDay,
                    DateOnly = null,
                    StartDateTime = request.StartDateTime.HasValue
                               ? DateTime.SpecifyKind(request.StartDateTime.Value, DateTimeKind.Utc)
                               : eventItem.StartDateTime,
                    EndDateTime = request.EndDateTime.HasValue
                               ? DateTime.SpecifyKind(request.EndDateTime.Value, DateTimeKind.Utc)
                               : eventItem.EndDateTime,
                    Description = request.Description is null &&
                               request.GetType().GetProperty(nameof(request.Description))?.GetValue(request) == null
                               ? eventItem.Description
                               : request.Description,
                    Location = request.Location is null &&
                               request.GetType().GetProperty(nameof(request.Location))?.GetValue(request) == null
                               ? eventItem.Location
                               : new GeoCoordinate(request.Location.Latitude, request.Location.Longitude)
                };
            }
            else if (request.AllDay is null && eventItem.AllDay == false)
            {
                return new Event
                {
                    Id = id,
                    Title = request.Title ?? eventItem.Title,
                    AllDay = eventItem.AllDay,
                    DateOnly = null,
                    StartDateTime = request.StartDateTime ?? eventItem.StartDateTime,
                    EndDateTime = request.EndDateTime ?? eventItem.EndDateTime,
                    Description = request.Description is null &&
                               request.GetType().GetProperty(nameof(request.Description))?.GetValue(request) == null
                               ? eventItem.Description
                               : request.Description,
                    Location = request.Location is null &&
                               request.GetType().GetProperty(nameof(request.Location))?.GetValue(request) == null
                               ? eventItem.Location
                               : new GeoCoordinate(request.Location.Latitude, request.Location.Longitude)
                };
            }
            else
            {
                throw new ArgumentException("Something went wrong, investigate UpdateRequest Mapping");
            }
        }
        public static EventResponse MapToResponse(this Event eventItem)
        {
            if (eventItem.AllDay == true)
                return new EventResponse
                {
                    Id = eventItem.Id,
                    Title = eventItem.Title,
                    AllDay = eventItem.AllDay,
                    DateOnly = eventItem.DateOnly,
                    Description = eventItem.Description,
                    Location = eventItem.Location != null
                        ? new CoordinateResponse { Latitude = eventItem.Location.Latitude, Longitude = eventItem.Location.Longitude }
                        : null
                };
            else
            {
                return new EventResponse
                {
                    Id = eventItem.Id,
                    Title = eventItem.Title,
                    AllDay = eventItem.AllDay,
                    StartDateTime = eventItem.StartDateTime,
                    EndDateTime = eventItem.EndDateTime,
                    Description = eventItem.Description,
                    Location = eventItem.Location != null
                        ? new CoordinateResponse { Latitude = eventItem.Location.Latitude, Longitude = eventItem.Location.Longitude }
                        : null
                };  
            }
        }

        public static EventsResponse MapToResponse(this IEnumerable<Event> eventItems)
        {
            return new EventsResponse
            {
                Events = eventItems.Select(MapToResponse)
            };
        }
        
    }
}
