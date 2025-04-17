using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calendara.Application.Models;

namespace Calendara.Contracts.Responses
{
    public class EventsResponse
    {
        IEnumerable<EventResponse> events { get; set; } = Enumerable.Empty<EventResponse>();
    }
}
