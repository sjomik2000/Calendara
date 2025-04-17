using Calendara.Application.Models;
using Calendara.Contracts.Responses;
using Calendara.Contracts.Requests;
using NetTopologySuite.Geometries;
using System;

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
                    Location = request.Location ?? new Coordinate(0, 0)
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
                    StartDateTime = request.StartDateTime,
                    EndDateTime = request.EndDateTime,
                    Description = request.Description ?? string.Empty,
                    Location = request.Location ?? new Coordinate(0, 0)
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
                    Description = request.Description ?? eventItem.Description,
                    Location = request.Location ?? eventItem.Location
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
                    Description = request.Description ?? eventItem.Description,
                    Location = request.Location ?? eventItem.Location
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
                    StartDateTime = request.StartDateTime ?? eventItem.StartDateTime,
                    EndDateTime = request.EndDateTime ?? eventItem.EndDateTime,
                    Description = request.Description ?? eventItem.Description,
                    Location = request.Location ?? eventItem.Location
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
                    Description = request.Description ?? eventItem.Description,
                    Location = request.Location ?? eventItem.Location
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
                    Location = eventItem.Location
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
                    Location = eventItem.Location
                };  
            }
        }
        
    }
}
