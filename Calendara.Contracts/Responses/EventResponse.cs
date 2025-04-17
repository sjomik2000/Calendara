using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calendara.Contracts.Responses
{
    public class EventResponse
    {
        public Guid Id { get; init; }
        public string Title { get; init; }
        public bool AllDay { get; init; }

        public DateTime? StartDateTime { get; init; }

        public DateTime? EndDateTime { get; init; }

        public DateOnly? DateOnly { get; init; }
        public string Description { get; init; }
        public Coordinate Location { get; init; }
    }
}
