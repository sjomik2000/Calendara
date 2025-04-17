using System;
using System.Collections.Generic;
using System.Text;
using Calendara.Application.Models;
using NetTopologySuite.Geometries;
using Calendara.Contracts.Responses;
using Calendara.Contracts.Requests;

namespace Calendara.UnitTests.Fixtures
{
    public static class EventsFixtures
    {
        public static CreateEventRequest GetCreateRequestFixture1()
        {
            return new CreateEventRequest
            {
                Title = "Sample Event 1",
                AllDay = false,
                DateOnly = null,
                StartDateTime = new DateTime(2025, 10, 15, 13, 0, 0),
                EndDateTime = new DateTime(2025, 10, 15, 14, 0, 0),
                Description = "This is a sample event 1.",
                Location = new Coordinate(52.477973735285154, -1.8987571127313878)
            };
        }
        public static CreateEventRequest GetCreateRequestFixture2()
        {
            return new CreateEventRequest
            {
                Title = "Sample Event 2",
                AllDay = true,
                DateOnly = new DateOnly(2025, 10, 15),
                StartDateTime = null,
                EndDateTime = null,
                Description = "This is another sample event.",
                Location = new Coordinate(51.50084938904448, -0.124582486769179)
            };
        }
        public static CreateEventRequest GetCreateRequestFixture3()
        {
            return new CreateEventRequest
            {
                Title = "Party!",
                AllDay = false,
                DateOnly = null,
                StartDateTime = new DateTime(2025, 05, 18, 21, 0, 0),
                EndDateTime = new DateTime(2025, 05, 19, 03, 0, 0),
                Description = null,
                Location = null
            };
        }

        public static List<Event> GetAllTestFixtures()
        {
            return new List<Event>
            {
                new Event
                {
                    Id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30"),
                    Title = "Sample Event 1",
                    AllDay = false,
                    DateOnly = null,
                    StartDateTime = new DateTime(2025,10,15,13,0,0),
                    EndDateTime = new DateTime(2025,10,15,14,0,0),
                    Description = "This is a sample event 1.",
                    Location = new Coordinate(52.477973735285154, -1.8987571127313878)
                },
                new Event
                {
                    Id = new Guid("0b9c3623-95a3-43b6-a5df-db65b5e19093"),
                    Title = "Sample Event 2",
                    AllDay = true,
                    DateOnly = new DateOnly(2025,10,15),
                    StartDateTime = null,
                    EndDateTime = null,
                    Description = "This is another sample event.",
                    Location = new Coordinate(51.50084938904448, -0.124582486769179)
                },
                new Event
                {
                    Id = new Guid("cae36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                    Title = "Party!",
                    AllDay = false,
                    DateOnly = null,
                    StartDateTime = new DateTime(2025, 05,18, 21, 0,0),
                    EndDateTime = new DateTime(2025, 05,19, 03,0,0),
                    Description = null,
                    Location = null
                }
            };
        }

        public static Event GetTestFixture()
        {
            return new Event
            {
                Id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30"),
                Title = "Sample Event 1",
                AllDay = false,
                DateOnly = null,
                StartDateTime = new DateTime(2025, 10, 15, 13, 0, 0),
                EndDateTime = new DateTime(2025, 10, 15, 14, 0, 0),
                Description = "This is a sample event 1.",
                Location = new Coordinate(52.477973735285154, -1.8987571127313878)
            };
        }

        public static Event GetTestFixture2()
        {
            return new Event
            {
                Id = new Guid("0b9c3623-95a3-43b6-a5df-db65b5e19093"),
                Title = "Sample Event 2",
                AllDay = true,
                DateOnly = new DateOnly(2025, 10, 15),
                StartDateTime = null,
                EndDateTime = null,
                Description = "This is another sample event.",
                Location = new Coordinate(51.50084938904448, -0.124582486769179)
            };
        }

        public static Event GetTestFixture3()
        {
            return new Event
            {
                Id = new Guid("cae36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                Title = "Party!",
                AllDay = false,
                DateOnly = null,
                StartDateTime = new DateTime(2025, 05, 18, 21, 0, 0),
                EndDateTime = new DateTime(2025, 05, 19, 03, 0, 0),
                Description = null,
                Location = null
            };
        }

        public static EventResponse MappedEvent1()
        {
            return new EventResponse
            {
                Id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30"),
                Title = "Sample Event 1",
                AllDay = false,
                StartDateTime = new DateTime(2025, 10, 15, 13, 0, 0),
                EndDateTime = new DateTime(2025, 10, 15, 14, 0, 0),
                Description = "This is a sample event 1.",
                Location = new Coordinate(52.477973735285154, -1.8987571127313878)
            };
        }

        public static EventResponse MappedEvent2()
        {
            return new EventResponse
            {
                Id = new Guid("0b9c3623-95a3-43b6-a5df-db65b5e19093"),
                Title = "Sample Event 2",
                AllDay = true,
                DateOnly = new DateOnly(2025, 10, 15),
                Description = "This is another sample event.",
                Location = new Coordinate(51.50084938904448, -0.124582486769179)
            };
        }

        public static EventResponse MappedEvent3()
        {
            return new EventResponse
            {
                Id = new Guid("cae36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                Title = "Party!",
                AllDay = false,
                StartDateTime = new DateTime(2025, 05, 18, 21, 0, 0),
                EndDateTime = new DateTime(2025, 05, 19, 03, 0, 0),
                Description = null,
                Location = null
            };
        }

        public static IEnumerable<EventResponse> MappedAllEvents()
        {
            return new List<EventResponse>
            {
                new EventResponse
                {
                    Id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30"),
                    Title = "Sample Event 1",
                    AllDay = false,
                    StartDateTime = new DateTime(2025,10,15,13,0,0),
                    EndDateTime = new DateTime(2025,10,15,14,0,0),
                    Description = "This is a sample event 1.",
                    Location = new Coordinate(52.477973735285154, -1.8987571127313878)
                },
                new EventResponse
                {
                    Id = new Guid("0b9c3623-95a3-43b6-a5df-db65b5e19093"),
                    Title = "Sample Event 2",
                    AllDay = true,
                    DateOnly = new DateOnly(2025,10,15),
                    Description = "This is another sample event.",
                    Location = new Coordinate(51.50084938904448, -0.124582486769179)
                },
                new EventResponse
                {
                    Id = new Guid("cae36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                    Title = "Party!",
                    AllDay = false,
                    StartDateTime = new DateTime(2025, 05,18, 21, 0,0),
                    EndDateTime = new DateTime(2025, 05,19, 03,0,0),
                    Description = null,
                    Location = null
                }

            };
        }
        public static UpdateEventRequest UpdateEventRequestForFixture1()
        {
            return new UpdateEventRequest
            {
                Title = "Sample Event 2",
                AllDay = true,
                DateOnly = new DateOnly(2025, 10, 15),
                StartDateTime = null,
                EndDateTime = null,
                Description = "This is another sample event.",
                Location = new Coordinate(51.50084938904448, -0.124582486769179)
            };
        }
        public static UpdateEventRequest UpdateEventRequestForFixture2()
        {
            return new UpdateEventRequest
            {
                Title = "Sample Event 1",
                AllDay = false,
                StartDateTime = new DateTime(2025, 10, 15, 13, 0, 0),
                EndDateTime = new DateTime(2025, 10, 15, 14, 0, 0),
                Description = "This is a sample event 1.",
                Location = new Coordinate(52.477973735285154, -1.8987571127313878)
            };
        }
        public static UpdateEventRequest UpdateEventRequestForFixture3()
        {
            return new UpdateEventRequest
            {
                Title = "Party Cancelled ;(",
                AllDay = false,
                DateOnly = null,
                StartDateTime = null,
                EndDateTime = null,
                Description = "Contact manager",
                Location = null
            };
        }
        public static Event UpdateTestFixtureExpectedForFixture1()
        {
            return new Event
            {
                Id = new Guid("0b9c3623-95a3-43b6-a5df-db65b5e19093"),
                Title = "Sample Event 2",
                AllDay = true,
                DateOnly = new DateOnly(2025, 10, 15),
                StartDateTime = null,
                EndDateTime = null,
                Description = "This is another sample event.",
                Location = new Coordinate(51.50084938904448, -0.124582486769179)
            };
        }

        public static Event UpdateTestFixtureExpectedForFixture2()
        {
            return new Event
            {
                Id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30"),
                Title = "Sample Event 1",
                AllDay = false,
                StartDateTime = new DateTime(2025, 10, 15, 13, 0, 0),
                EndDateTime = new DateTime(2025, 10, 15, 14, 0, 0),
                Description = "This is a sample event 1.",
                Location = new Coordinate(52.477973735285154, -1.8987571127313878)
            };
        }

        public static Event UpdateTestFixtureExpectedForFixture3()
        {
            return new Event
            {
                Id = new Guid("cae36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                Title = "Party Cancelled ;(",
                AllDay = false,
                DateOnly = null,
                StartDateTime = null,
                EndDateTime = null,
                Description = "Contact manager",
                Location = null
            };
        }
    }
}
