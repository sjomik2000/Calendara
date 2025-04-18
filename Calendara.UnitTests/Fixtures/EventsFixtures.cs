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

        public static Event GetTestFixture4()
        {
            return new Event
            {
                Id = new Guid("d1e36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                Title = "Existing All-Day Event",
                AllDay = true,
                DateOnly = new DateOnly(2025, 12, 25),
                StartDateTime = null,
                EndDateTime = null,
                Description = "This is an all-day event stored in the database.",
                Location = new Coordinate(40.712776, -74.005974)
            };
        }

        public static Event GetTestFixture5()
        {
            return new Event
            {
                Id = new Guid("e2f36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                Title = "Existing Non-All-Day Event",
                AllDay = false,
                DateOnly = null,
                StartDateTime = new DateTime(2025, 11, 15, 10, 0, 0),
                EndDateTime = new DateTime(2025, 11, 15, 12, 0, 0),
                Description = "This is a non-all-day event stored in the database.",
                Location = new Coordinate(34.052235, -118.243683)
            };
        }

        public static Event GetTestFixture6()
        {
            return new Event
            {
                Id = new Guid("f3e36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                Title = "Single-Day Event",
                AllDay = true,
                DateOnly = new DateOnly(2025, 12, 25),
                StartDateTime = null,
                EndDateTime = null,
                Description = "Christmas Day Event",
                Location = new Coordinate(10, 20)
            };
        }

        public static Event GetTestFixture7()
        {
            return new Event
            {
                Id = new Guid("f4e36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                Title = "Multi-Day Event",
                AllDay = false,
                DateOnly = null,
                StartDateTime = new DateTime(2025, 12, 24, 18, 0, 0),
                EndDateTime = new DateTime(2025, 12, 26, 6, 0, 0),
                Description = "Christmas Eve to Boxing Day",
                Location = new Coordinate(30, 40)
            };
        }

        public static Event GetTestFixture8()
        {
            return new Event
            {
                Id = new Guid("a5e36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                Title = "Event Within Range",
                AllDay = true,
                DateOnly = new DateOnly(2025, 12, 20),
                StartDateTime = null,
                EndDateTime = null,
                Description = "Event happening on a single day within the range.",
                Location = new Coordinate(10, 20)
            };
        }

        public static Event GetTestFixture9()
        {
            return new Event
            {
                Id = new Guid("b6e36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                Title = "Event Spanning Range",
                AllDay = false,
                DateOnly = null,
                StartDateTime = new DateTime(2025, 12, 19, 18, 0, 0),
                EndDateTime = new DateTime(2025, 12, 21, 6, 0, 0),
                Description = "Event spanning multiple days within the range.",
                Location = new Coordinate(30, 40)
            };
        }

        public static Event GetTestFixture10()
        {
            return new Event
            {
                Id = new Guid("c7e36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                Title = "Event Outside Range",
                AllDay = false,
                DateOnly = null,
                StartDateTime = new DateTime(2025, 12, 25, 18, 0, 0),
                EndDateTime = new DateTime(2025, 12, 26, 6, 0, 0),
                Description = "Event happening completely outside the range.",
                Location = new Coordinate(50, 60)
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
                Location = new CoordinateResponse { X = 52.477973735285154, Y = -1.8987571127313878 }
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
                Location = new CoordinateResponse { X = 51.50084938904448, Y = -0.124582486769179 }
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
                    Location = new CoordinateResponse { X = 52.477973735285154, Y = -1.8987571127313878 }
                },
                new EventResponse
                {
                    Id = new Guid("0b9c3623-95a3-43b6-a5df-db65b5e19093"),
                    Title = "Sample Event 2",
                    AllDay = true,
                    DateOnly = new DateOnly(2025,10,15),
                    Description = "This is another sample event.",
                    Location = new CoordinateResponse { X = 51.50084938904448, Y = -0.124582486769179 }
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
                StartDateTime = new DateTime(2025, 05, 18, 21, 0, 0),
                EndDateTime = new DateTime(2025, 05, 19, 03, 0, 0),
                Description = "Contact manager",
                Location = null
            };
        }

        public static UpdateEventRequest UpdateEventRequestForFixture4()
        {
            return new UpdateEventRequest
            {
                Title = "Updated Event Title", 
                Description = "Updated description for the event." 
            };
        }

        public static UpdateEventRequest UpdateEventRequestForFixture5()
        {
            return new UpdateEventRequest
            {
                // Excluding all properties to simulate no changes
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
                StartDateTime = new DateTime(2025, 05, 18, 21, 0, 0),
                EndDateTime = new DateTime(2025, 05, 19, 03, 0, 0),
                Description = "Contact manager",
                Location = null
            };
        }

        public static Event UpdateTestFixtureExpectedForFixture4()
        {
            return new Event
            {
                Id = new Guid("d1e36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                Title = "Updated Event Title",
                AllDay = true,
                DateOnly = new DateOnly(2025, 12, 25), 
                StartDateTime = null,
                EndDateTime = null,
                Description = "Updated description for the event.",
                Location = new Coordinate(40.712776, -74.005974)
            };
        }

        public static Event UpdateTestFixtureExpectedForFixture5()
        {
            return new Event
            {
                Id = new Guid("e2f36ce8-6db9-4cb0-b4bd-c4f4ac765922"),
                Title = "Existing Non-All-Day Event", 
                AllDay = false,
                DateOnly = null,
                StartDateTime = new DateTime(2025, 11, 15, 10, 0, 0), 
                EndDateTime = new DateTime(2025, 11, 15, 12, 0, 0), 
                Description = "This is a non-all-day event stored in the database.", 
                Location = new Coordinate(34.052235, -118.243683) 
            };
        }

        public static Event GetInvalidEventFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "", // Invalid Title
                AllDay = false,
                StartDateTime = DateTime.UtcNow,
                EndDateTime = DateTime.UtcNow.AddHours(1),
                Description = "Invalid event for testing.",
                Location = null
            };
        }

        public static Event GetInvalidEventEmptyTitleFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "", // Invalid: Empty title
                AllDay = false,
                DateOnly = null,
                StartDateTime = DateTime.UtcNow.AddDays(1),
                EndDateTime = DateTime.UtcNow.AddDays(1).AddHours(1),
                Description = "Event with empty title",
                Location = new Coordinate(50, 50)
            };
        }
        public static Event GetInvalidEventLongTitleFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = new string('A', 101), // Invalid: Title exceeds 100 characters
                AllDay = false,
                DateOnly = null,
                StartDateTime = DateTime.UtcNow.AddDays(1),
                EndDateTime = DateTime.UtcNow.AddDays(1).AddHours(1),
                Description = "Event with too long title",
                Location = new Coordinate(50, 50)
            };
        }

        public static Event GetInvalidEventMissingDateOnlyFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "Invalid All-Day Event",
                AllDay = true,
                DateOnly = null, // Invalid: Missing DateOnly for all-day event
                StartDateTime = null,
                EndDateTime = null,
                Description = "All-day event missing date",
                Location = new Coordinate(50, 50)
            };
        }
        public static Event GetInvalidEventPastDateOnlyFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "Invalid Past Date Event",
                AllDay = true,
                DateOnly = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)), // Invalid past date
                StartDateTime = null,
                EndDateTime = null,
                Description = "All-day event with past date",
                Location = new Coordinate(50, 50)
            };
        }

        public static Event GetInvalidEventDateOnlyForNonAllDayFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "Invalid Non-All-Day Event",
                AllDay = false,
                DateOnly = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), // Invalid DateOnly for non all-day event
                StartDateTime = DateTime.UtcNow.AddDays(1),
                EndDateTime = DateTime.UtcNow.AddDays(1).AddHours(1),
                Description = "Non-all-day event with DateOnly",
                Location = new Coordinate(50, 50)
            };
        }

        public static Event GetInvalidEventFarFutureDateOnlyFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "Invalid Far Future Date Event",
                AllDay = true,
                DateOnly = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(6)), // Invalid as more than 5 years in future
                StartDateTime = null,
                EndDateTime = null,
                Description = "Event too far in the future",
                Location = new Coordinate(50, 50)
            };
        }
        public static Event GetInvalidEventMissingStartDateTimeFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "Invalid Event Missing Start",
                AllDay = false,
                DateOnly = null,
                StartDateTime = null, // Missing StartDateTime for non all-day event
                EndDateTime = DateTime.UtcNow.AddDays(1),
                Description = "Event missing start time",
                Location = new Coordinate(50, 50)
            };
        }

        public static Event GetInvalidEventPastStartDateTimeFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "Invalid Past Start Event",
                AllDay = false,
                DateOnly = null,
                StartDateTime = DateTime.UtcNow.AddDays(-1), // Past start time
                EndDateTime = DateTime.UtcNow.AddDays(1),
                Description = "Event with past start time",
                Location = new Coordinate(50, 50)
            };
        }

        public static Event GetInvalidEventStartDateTimeForAllDayFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "Invalid All-Day Event with Start",
                AllDay = true,
                DateOnly = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                StartDateTime = DateTime.UtcNow.AddDays(1), // Invalid StartDateTime for all-day event
                EndDateTime = null,
                Description = "All-day event with start time",
                Location = new Coordinate(50, 50)
            };
        }

        public static Event GetInvalidEventMissingEndDateTimeFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "Invalid Event Missing End",
                AllDay = false,
                DateOnly = null,
                StartDateTime = DateTime.UtcNow.AddDays(1),
                EndDateTime = null, // Missing EndDateTime for non all-day event
                Description = "Event missing end time",
                Location = new Coordinate(50, 50)
            };
        }

        public static Event GetInvalidEventEndDateTimeForAllDayFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "Invalid All-Day Event with End",
                AllDay = true,
                DateOnly = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                StartDateTime = null,
                EndDateTime = DateTime.UtcNow.AddDays(1).AddHours(2), // EndDateTime provided for all-day event
                Description = "All-day event with end time",
                Location = new Coordinate(50, 50)
            };
        }

        public static Event GetInvalidEventEndBeforeStartFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "Invalid Event End Before Start",
                AllDay = false,
                DateOnly = null,
                StartDateTime = DateTime.UtcNow.AddDays(2),
                EndDateTime = DateTime.UtcNow.AddDays(1), // EndDateTime before start
                Description = "Event with end time before start time",
                Location = new Coordinate(50, 50)
            };
        }

        public static Event GetInvalidEventTooLongDurationFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "Invalid Event Too Long",
                AllDay = false,
                DateOnly = null,
                StartDateTime = DateTime.UtcNow.AddDays(1),
                EndDateTime = DateTime.UtcNow.AddDays(9), // Duration exceeds 7 days
                Description = "Event exceeding maximum duration",
                Location = new Coordinate(50, 50)
            };
        }

        public static Event GetInvalidEventLongDescriptionFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "Invalid Event with Long Description",
                AllDay = false,
                DateOnly = null,
                StartDateTime = DateTime.UtcNow.AddDays(1),
                EndDateTime = DateTime.UtcNow.AddDays(1).AddHours(2),
                Description = new string('A', 501), // Description exceeds 500 characters
                Location = new Coordinate(50, 50)
            };
        }

        public static Event GetInvalidEventLocationFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "Invalid Event with Invalid Coordinates",
                AllDay = false,
                DateOnly = null,
                StartDateTime = DateTime.UtcNow.AddDays(1),
                EndDateTime = DateTime.UtcNow.AddDays(1).AddHours(2),
                Description = "Event with invalid location coordinates",
                Location = new Coordinate(200, 100) // Longitude out of range
            };
        }

        public static Event GetMultipleValidationFailuresFixture()
        {
            return new Event
            {
                Id = Guid.NewGuid(),
                Title = "",
                AllDay = false,
                DateOnly = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), 
                StartDateTime = DateTime.UtcNow.AddDays(-1),
                EndDateTime = null, 
                Description = new string('A', 501), 
                Location = new Coordinate(200, 100) 
            };
        }



    }
}
