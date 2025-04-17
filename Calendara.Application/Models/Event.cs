using System;
using System.Collections.Generic;
using System.Text;
using NetTopologySuite.Geometries; // To use Coordinate type 

namespace Calendara.Application.Models
{
    public class Event
    {
        //using private set instead of init as init only supported in C#9
        //using Event constructor to replace required keyword which is supported in C#11
        public required Guid Id { get; init; } 

        public required string Title { get; init; }

        public required bool AllDay { get; init; }
        public DateOnly? DateOnly { get; init; }

        public DateTime? StartDateTime { get; init; }

        public DateTime? EndDateTime { get; init; }

        //public DateTime? Reminder { get; set; }

        public string? Description { get; init; }

        public Coordinate? Location { get; init; }

    }
}
