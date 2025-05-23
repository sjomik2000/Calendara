﻿using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calendara.Contracts.Requests
{
    public class UpdateEventRequest
    {
        public string? Title { get; init; }

        public bool? AllDay { get; init; }
        public DateOnly? DateOnly { get; init; }

        public DateTime? StartDateTime { get; init; }

        public DateTime? EndDateTime { get; init; }

        public string? Description { get; init; }

        public CoordinateRequest? Location { get; init; }

    }
}
