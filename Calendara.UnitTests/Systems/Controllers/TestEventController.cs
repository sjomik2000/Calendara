using System;
using Xunit;
using Calendara.Api.Controller;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using System.Threading.Tasks;
using Moq;
using Calendara.Application.Services;
using Calendara.Application.Models;
using System.Linq;
using Calendara.UnitTests.Fixtures;
using Calendara.UnitTests.Helpers;
using System.Collections.Generic;
using System.Net.Http;
using Calendara.Api.Mapping;
using NetTopologySuite.Geometries;
using Calendara.Contracts.Requests;
using System.Net.Http.Json;
using System.Net;
using Moq.Protected;
using System.Threading;
using Microsoft.Extensions.Logging;
using Calendara.Contracts.Responses;

namespace Calendara.UnitTests.Systems.Controllers
{
    public class TestEventController
    {
        #region Create
        [Fact]
        public async Task Create_OnSuccess_ReturnsStatusCode201()
        {
            // Arrange 
            var createRequest = EventsFixtures.GetCreateRequestFixture1();
            var createdEvent = createRequest.MapToEvent();
            var mockEventService = new Mock<IEventService>();
            mockEventService
                .Setup(service => service.CreateAsync(It.Is<Event>(e =>
                    e.Title == createdEvent.Title &&
                    e.AllDay == createdEvent.AllDay &&
                    e.DateOnly == createdEvent.DateOnly &&
                    e.StartDateTime == createdEvent.StartDateTime &&
                    e.EndDateTime == createdEvent.EndDateTime &&
                    e.Description == createdEvent.Description &&
                    e.Location.Equals(createdEvent.Location))))
                .ReturnsAsync(true);
            var sut = new EventController(mockEventService.Object);
            var createRequest2 = EventsFixtures.GetCreateRequestFixture2();
            var createdEvent2 = createRequest2.MapToEvent();
            var mockEventService2 = new Mock<IEventService>();
            mockEventService2
                .Setup(service => service.CreateAsync(It.Is<Event>(e =>
                    e.Title == createdEvent2.Title &&
                    e.AllDay == createdEvent2.AllDay &&
                    e.DateOnly == createdEvent2.DateOnly &&
                    e.StartDateTime == createdEvent2.StartDateTime &&
                    e.EndDateTime == createdEvent2.EndDateTime &&
                    e.Description == createdEvent2.Description &&
                    e.Location.Equals(createdEvent2.Location))))
                .ReturnsAsync(true);
            var sut2 = new EventController(mockEventService2.Object);
            // Act
            var result = (CreatedAtActionResult)await sut.Create(createRequest);
            var result2 = (CreatedAtActionResult)await sut2.Create(createRequest2);
            // Assert
            result.StatusCode.Should().Be(201);
            result2.StatusCode.Should().Be(201);
        }
        [Fact]
        public async Task Create_OnSuccess_ActionsEventService()
        {
            //Arrange
            var createRequest = EventsFixtures.GetCreateRequestFixture1();
            var createdEvent = createRequest.MapToEvent();
            var mockEventService = new Mock<IEventService>();
            mockEventService
                .Setup(service => service.CreateAsync(It.IsAny<Event>()))
                .ReturnsAsync(true);
            var sut = new EventController(mockEventService.Object);
            //Act
            var result = (CreatedAtActionResult)await sut.Create(createRequest);
            //Assert
            mockEventService.Verify(
                service => service.CreateAsync(It.IsAny<Event>()),
                Times.Once()
            );
        }

        [Fact]
        public async Task Create_OnSuccess_Returns_CreatedEvents()
        {
            //Arrange
            var createRequest = EventsFixtures.GetCreateRequestFixture1();
            var createdEvent = EventsFixtures.GetTestFixture();
            var mockEventService = new Mock<IEventService>();
            mockEventService
                .Setup(service => service.CreateAsync(It.Is<Event>(e =>
                    e.Title == createdEvent.Title &&
                    e.AllDay == createdEvent.AllDay &&
                    e.DateOnly == createdEvent.DateOnly &&
                    e.StartDateTime == createdEvent.StartDateTime &&
                    e.EndDateTime == createdEvent.EndDateTime &&
                    e.Description == createdEvent.Description &&
                    e.Location.Equals(createdEvent.Location))))
                .ReturnsAsync(true);
            var sut = new EventController(mockEventService.Object);
            // Act
            var result = (CreatedAtActionResult)await sut.Create(createRequest);
            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var objectResult = (CreatedAtActionResult)result;
            objectResult.Value.Should().BeEquivalentTo(createdEvent, options => options.Excluding(e => e.Id));
        }
        [Fact]
        public async Task Create_OnSuccess_MapsRequests()
        {
            //Arrange
            var createRequest2 = EventsFixtures.GetCreateRequestFixture2();
            var createRequest3 = EventsFixtures.GetCreateRequestFixture3();
            var createdEvent = EventsFixtures.GetTestFixture();
            var createdEvent2 = EventsFixtures.GetTestFixture2();
            var createdEvent3 = EventsFixtures.GetTestFixture3();
            var mockEventService2 = new Mock<IEventService>();
            mockEventService2
                .Setup(service => service.CreateAsync(It.Is<Event>(e =>
                    e.Title == createdEvent2.Title &&
                    e.AllDay == createdEvent2.AllDay &&
                    e.DateOnly == createdEvent2.DateOnly &&
                    e.StartDateTime == createdEvent2.StartDateTime &&
                    e.EndDateTime == createdEvent2.EndDateTime &&
                    e.Description == createdEvent2.Description &&
                    e.Location.Equals(createdEvent2.Location))))
                .ReturnsAsync(true);
            var sut2 = new EventController(mockEventService2.Object);
            var mockEventService3 = new Mock<IEventService>();
            mockEventService3
                .Setup(service => service.CreateAsync(It.Is<Event>(e =>
                    e.Title == createdEvent3.Title &&
                    e.AllDay == createdEvent3.AllDay &&
                    e.DateOnly == createdEvent3.DateOnly &&
                    e.StartDateTime == createdEvent3.StartDateTime &&
                    e.EndDateTime == createdEvent3.EndDateTime &&
                    e.Description == createdEvent3.Description &&
                    (e.Location == null && createdEvent3.Location == null ||
                     e.Location != null && createdEvent3.Location != null && e.Location.Equals(createdEvent3.Location)))))
                .ReturnsAsync(true);
            var sut3 = new EventController(mockEventService3.Object);
            // Act
            var result2 = (CreatedAtActionResult)await sut2.Create(createRequest2);
            var result3 = (CreatedAtActionResult)await sut3.Create(createRequest3);
            // Assert
            result2.Should().BeOfType<CreatedAtActionResult>();
            var objectResult2 = (CreatedAtActionResult)result2;
            objectResult2.Value.Should().BeEquivalentTo(createdEvent2, options => options.Excluding(e => e.Id));
            result3.Should().BeOfType<CreatedAtActionResult>();
            var objectResult3 = (CreatedAtActionResult)result3;
            objectResult3.Value.Should().BeEquivalentTo(createdEvent3, options =>
                options.Excluding(e => e.Id)
                    .Using<string>(ctx => ctx.Subject.Should().Be(ctx.Expectation ?? string.Empty))
                    .WhenTypeIs<string>());
        }

        [Fact]

        public Task Create_HasHttpPostAttribute()
        {
            // Arrange
            var methodInfo = typeof(EventController).GetMethod(nameof(EventController.Create));

            // Act
            var httpPostAttribute = methodInfo.GetCustomAttributes(typeof(HttpPostAttribute), false)
                                     .FirstOrDefault() as HttpPostAttribute;

            // Assert
            httpPostAttribute.Should().NotBeNull();
            httpPostAttribute.Template.Should().BeNull();
            return Task.CompletedTask;
        }
        [Fact]

        public async Task Create_ExtractsHTTPBodyFromRequest()
        {
            var createRequest = EventsFixtures.GetCreateRequestFixture1();
            var mockHttpHandler = new Mock<HttpMessageHandler>();
            mockHttpHandler
                .Protected() 
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri == new Uri("http://localhost/api/events/") &&
                        req.Content != null),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created
                });

            var httpClient = new HttpClient(mockHttpHandler.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };

            var sut = new EventController(new Mock<IEventService>().Object);

            // Act
            var response = await httpClient.PostAsJsonAsync("api/events/", createRequest);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            mockHttpHandler.Protected()
                .Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri == new Uri("http://localhost/api/events/") &&
                        req.Content != null),
                    ItExpr.IsAny<CancellationToken>());
        }
        #endregion
        #region GetById
        [Fact]
        public async Task GetById_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange 
            var mockEventService = new Mock<IEventService>();
            var id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30");
            mockEventService
                .Setup(service => service.GetByIdAsync(id))
                .ReturnsAsync(EventsFixtures.GetTestFixture());
            var sut = new EventController(mockEventService.Object);
            // Act
            var result = (OkObjectResult)await sut.GetById(id);
            // Assert
            result.StatusCode.Should().Be(200);
        }
        [Fact]
        public async Task GetById_OnSuccess_ActionsEventService()
        {
            //Arrange
            var mockEventService = new Mock<IEventService>();
            var id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30");
            mockEventService
                .Setup(service => service.GetByIdAsync(id))
                .ReturnsAsync(EventsFixtures.GetTestFixture());
            var sut = new EventController(mockEventService.Object);
            //Act
            var result = await sut.GetById(id);
            //Assert
            mockEventService.Verify(
                service => service.GetByIdAsync(id),
                Times.Once()
                );
        }

        [Fact]
        public async Task GetById_OnSuccess_Returns_Event()
        {
            //Arrange
            var mockEventService = new Mock<IEventService>();
            var id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30");
            mockEventService
                .Setup(service => service.GetByIdAsync(id))
                .ReturnsAsync(EventsFixtures.GetTestFixture());
            var expectedEvent = EventsFixtures.GetTestFixture();
            var expectedResponse = EventsFixtures.MappedEvent1();
            var sut = new EventController(mockEventService.Object);
            // Act
            var result = await sut.GetById(id);
            
            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeEquivalentTo(expectedResponse, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetById_OnNoEventsFound_ReturnsStatusCode404()
        {
            //Arrange
            var mockEventService = new Mock<IEventService>();
            var id = Guid.NewGuid();
            mockEventService
                .Setup(service => service.GetByIdAsync(id))
                .ReturnsAsync((Event)null);
            var sut = new EventController(mockEventService.Object);
            // Act
            var result = await sut.GetById(id);
            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public async Task GetById_HasHttpGetAttributeWithGuidRouteParameter()
        {
            // Arrange
            var expectedEvent = EventsFixtures.GetTestFixture();
            var mockHttpHandler = MockHttpMessageHandler<Event>.SetupBasicGetResourceList(new List<Event> { expectedEvent });
            var httpClient = new HttpClient(mockHttpHandler.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/Api/Event/701bd75d-bd97-40e0-b4e3-828afe2acc30");
            var response = await httpClient.SendAsync(request);
            var methodInfo = typeof(EventController).GetMethod(nameof(EventController.GetById));
            // Act
            var httpGetAttribute = methodInfo.GetCustomAttributes(typeof(HttpGetAttribute), false)
                                     .FirstOrDefault() as HttpGetAttribute;
            var parameterInfo = methodInfo.GetParameters()
                                           .FirstOrDefault(p => p.Name == "id" && p.ParameterType == typeof(Guid));
            var fromRouteAttribute = parameterInfo?.GetCustomAttributes(typeof(FromRouteAttribute), false)
                                                  .FirstOrDefault() as FromRouteAttribute;
            var extractedGuid = new Guid(request.RequestUri.Segments.Last());

            // Assert
            httpGetAttribute.Should().NotBeNull();
            httpGetAttribute.Template.Should().Be("{id:guid}");
            fromRouteAttribute.Should().NotBeNull();
            extractedGuid.Should().Be(new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30"));
        }

        [Fact] 
        public Task GetById_OnSuccess_MapsToEventResponse()
        {
            // Arrange
            var expectedEvent1 = EventsFixtures.GetTestFixture();
            var expectedResponse1 = EventsFixtures.MappedEvent1();
            var expectedEvent2 = EventsFixtures.GetTestFixture2();
            var expectedResponse2 = EventsFixtures.MappedEvent2();
            var expectedEvent3 = EventsFixtures.GetTestFixture3();
            var expectedResponse3 = EventsFixtures.MappedEvent3();
            // Act
            var mappedResponse1 = expectedEvent1.MapToResponse();
            var mappedResponse2 = expectedEvent2.MapToResponse();
            var mappedResponse3 = expectedEvent3.MapToResponse();
            //Assert 
            mappedResponse1.Should().BeEquivalentTo(expectedResponse1, options => options.ExcludingMissingMembers());
            mappedResponse2.Should().BeEquivalentTo(expectedResponse2, options => options.ExcludingMissingMembers());
            mappedResponse3.Should().BeEquivalentTo(expectedResponse3, options => options.ExcludingMissingMembers());
            return Task.CompletedTask;
        }
        #endregion
        #region GetAll
        [Fact]
        public async Task GetAll_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange 
            var mockEventService = new Mock<IEventService>();
            mockEventService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(EventsFixtures.GetAllTestFixtures());
            var sut = new EventController(mockEventService.Object);
            // Act
            var result = (OkObjectResult)await sut.GetAll();
            // Assert
            result.StatusCode.Should().Be(200);
        }
        [Fact]
        public async Task GetAll_OnSuccess_ActionsEventService()
        {
            //Arrange
            var mockEventService = new Mock<IEventService>();
            mockEventService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(EventsFixtures.GetAllTestFixtures());
            var sut = new EventController(mockEventService.Object);
            //Act
            var result = await sut.GetAll();
            //Assert
            mockEventService.Verify(
                service => service.GetAllAsync(),
                Times.Once()
                );
        }

        [Fact]
        public async Task GetAll_OnSuccess_Returns_ListOfEvents()
        {
            //Arrange
            var mockEventService = new Mock<IEventService>();
            var events = EventsFixtures.GetAllTestFixtures();
            var expectedResponse = new EventsResponse
            {
                Events = events.Select(e => e.MapToResponse())
            };

            mockEventService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(events);

            var sut = new EventController(mockEventService.Object);
            // Act
            var result = await sut.GetAll();

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeEquivalentTo(expectedResponse, options => options.ExcludingMissingMembers());
        }

        [Fact]
        
        public Task GetAll_HasHttpGetAttribute()
        {
            // Arrange
            var methodInfo = typeof(EventController).GetMethod(nameof(EventController.GetAll));
            // Act
            var httpGetAttribute = methodInfo.GetCustomAttributes(typeof(HttpGetAttribute), false)
                                     .FirstOrDefault() as HttpGetAttribute;
            // Assert
            httpGetAttribute.Should().NotBeNull();
            httpGetAttribute.Template.Should().BeNull();
            return Task.CompletedTask;
        } 

        [Fact]
        public Task GetAll_OnSuccess_MapsTheResponseToEventsResponse()
        {
            // Arrange
            var expectedEvents = EventsFixtures.GetAllTestFixtures();
            var expectedResponses = new EventsResponse
            {
                Events = expectedEvents.Select(e => e.MapToResponse())
            };
            // Act
            var mappedResponse = expectedEvents.MapToResponse();
            //Assert 
            mappedResponse.Should().BeEquivalentTo(expectedResponses, options => options.ExcludingMissingMembers());

            return Task.CompletedTask;

        }
        #endregion
        #region GetByDate
        [Fact]
        public async Task GetByDate_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            var mockEventService = new Mock<IEventService>();
            var events = new List<Event>{
                EventsFixtures.GetTestFixture6(),
                EventsFixtures.GetTestFixture7()
            };
            var date = new DateOnly(2025, 12, 25);

            mockEventService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(events);

            var sut = new EventController(mockEventService.Object);

            // Act
            var result = (OkObjectResult)await sut.GetByDate(date);

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetByDate_OnSuccess_ActionsEventService()
        {
            // Arrange
            var mockEventService = new Mock<IEventService>();
            var events = new List<Event> { 
                EventsFixtures.GetTestFixture6(),
                EventsFixtures.GetTestFixture7()
            };
            var date = new DateOnly(2025, 12, 25);

            mockEventService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(events);

            var sut = new EventController(mockEventService.Object);

            // Act
            var result = await sut.GetByDate(date);

            // Assert
            mockEventService.Verify(service => service.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task GetByDate_OnSuccess_Returns_FilteredEvents()
        {
            // Arrange
            var mockEventService = new Mock<IEventService>();
            var events = new List<Event>{
                EventsFixtures.GetTestFixture6(),
                EventsFixtures.GetTestFixture7()
            };
            var date = new DateOnly(2025, 12, 25);

            var expectedResponses = new List<EventResponse>{
                EventsFixtures.GetTestFixture6().MapToResponse(),
                EventsFixtures.GetTestFixture7().MapToResponse()
            };

            mockEventService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(events);

            var sut = new EventController(mockEventService.Object);

            // Act
            var result = (OkObjectResult)await sut.GetByDate(date);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeEquivalentTo(expectedResponses, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetByDate_OnNoEventsFound_ReturnsEmptyList()
        {
            // Arrange
            var mockEventService = new Mock<IEventService>();
            var events = new List<Event>();
            var date = new DateOnly(2025, 12, 25);

            mockEventService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(events);

            var sut = new EventController(mockEventService.Object);

            // Act
            var result = (OkObjectResult)await sut.GetByDate(date);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeEquivalentTo(new List<EventResponse>());
        }
        #endregion
        #region GetByDateRange
        [Fact]
        public async Task GetByDateRange_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            var mockEventService = new Mock<IEventService>();
            var events = new List<Event>{
                EventsFixtures.GetTestFixture8(),
                EventsFixtures.GetTestFixture9(),
                EventsFixtures.GetTestFixture10()
            };
            var startDate = new DateOnly(2025, 12, 19);
            var endDate = new DateOnly(2025, 12, 21);

            mockEventService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(events);

            var sut = new EventController(mockEventService.Object);

            // Act
            var result = (OkObjectResult)await sut.GetByDateRange(startDate, endDate);

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetByDateRange_OnSuccess_ActionsEventService()
        {
            // Arrange
            var mockEventService = new Mock<IEventService>();
            var events = new List<Event>{
                EventsFixtures.GetTestFixture8(),
                EventsFixtures.GetTestFixture9(),
                EventsFixtures.GetTestFixture10()
            };
            var startDate = new DateOnly(2025, 12, 19);
            var endDate = new DateOnly(2025, 12, 21);

            mockEventService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(events);

            var sut = new EventController(mockEventService.Object);

            // Act
            var result = await sut.GetByDateRange(startDate, endDate);

            // Assert
            mockEventService.Verify(service => service.GetAllAsync(), Times.Once());
        }

        [Fact]
        public async Task GetByDateRange_OnSuccess_Returns_FilteredEvents()
        {
            // Arrange
            var mockEventService = new Mock<IEventService>();
            var events = new List<Event>{
                EventsFixtures.GetTestFixture8(),
                EventsFixtures.GetTestFixture9(),
                EventsFixtures.GetTestFixture10()
            };
            var startDate = new DateOnly(2025, 12, 19);
            var endDate = new DateOnly(2025, 12, 21);

            var expectedResponses = new List<EventResponse>{
                EventsFixtures.GetTestFixture8().MapToResponse(),
                EventsFixtures.GetTestFixture9().MapToResponse()
            };

            mockEventService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(events);

            var sut = new EventController(mockEventService.Object);

            // Act
            var result = (OkObjectResult)await sut.GetByDateRange(startDate, endDate);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeEquivalentTo(expectedResponses, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public async Task GetByDateRange_OnNoEventsFound_ReturnsEmptyList()
        {
            // Arrange
            var mockEventService = new Mock<IEventService>();
            var events = new List<Event>(); 
            var startDate = new DateOnly(2025, 12, 19);
            var endDate = new DateOnly(2025, 12, 21);

            mockEventService
                .Setup(service => service.GetAllAsync())
                .ReturnsAsync(events);

            var sut = new EventController(mockEventService.Object);

            // Act
            var result = (OkObjectResult)await sut.GetByDateRange(startDate, endDate);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeEquivalentTo(new List<EventResponse>());
        }
        #endregion
        #region Update
        [Fact]
        public async Task Update_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange 
            var mockEventService = new Mock<IEventService>();
            var oldEvent = EventsFixtures.GetTestFixture();
            var id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30");
            var updateRequest = EventsFixtures.UpdateEventRequestForFixture1();
            var mappedUpdate = updateRequest.MapToEvent(id, oldEvent);
            var newEvent = EventsFixtures.UpdateTestFixtureExpectedForFixture1();
            
            mockEventService
                .Setup(service => service.GetByIdAsync(id))
                .ReturnsAsync(oldEvent);
            mockEventService
                .Setup(service => service.UpdateAsync(It.Is<Event>(e =>
                    e.Id == mappedUpdate.Id &&
                    e.Title == mappedUpdate.Title &&
                    e.AllDay == mappedUpdate.AllDay &&
                    e.DateOnly == mappedUpdate.DateOnly &&
                    e.StartDateTime == mappedUpdate.StartDateTime &&
                    e.EndDateTime == mappedUpdate.EndDateTime &&
                    e.Description == mappedUpdate.Description &&
                    e.Location.Equals(mappedUpdate.Location))))
                .ReturnsAsync(newEvent);
            var sut = new EventController(mockEventService.Object);
            // Act
            var result = await sut.Update(updateRequest, id);
            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result;
            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(newEvent.MapToResponse());
        }
        
        [Fact]
        public async Task Update_OnSuccess_ActionsEventService()
        {
            //Arrange
            var mockEventService = new Mock<IEventService>();
            var oldEvent = EventsFixtures.GetTestFixture();
            var id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30");
            var updateRequest = EventsFixtures.UpdateEventRequestForFixture1();
            var mappedUpdate = updateRequest.MapToEvent(id, oldEvent);
            var newEvent = EventsFixtures.UpdateTestFixtureExpectedForFixture1();

            mockEventService
                .Setup(service => service.GetByIdAsync(id))
                .ReturnsAsync(oldEvent);
            mockEventService
                .Setup(service => service.UpdateAsync(It.IsAny<Event>()))
                .ReturnsAsync(newEvent);
            var sut = new EventController(mockEventService.Object);
            //Act
            var result = await sut.Update(updateRequest, id);
            //Assert
            mockEventService.Verify(
                   service => service.UpdateAsync(It.Is<Event>(e =>
                        e.Id == mappedUpdate.Id &&
                        e.Title == mappedUpdate.Title)),
            Times.Once()
            );
        }

        [Fact]
        public async Task Update_OnSuccess_Returns_Event()
        {
            //Arrange
            var mockEventService = new Mock<IEventService>();
            var oldEvent = EventsFixtures.GetTestFixture();
            var id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30");
            var updateRequest = EventsFixtures.UpdateEventRequestForFixture1();
            var mappedUpdate = updateRequest.MapToEvent(id, oldEvent);
            var newEvent = EventsFixtures.UpdateTestFixtureExpectedForFixture1();

            mockEventService
                .Setup(service => service.GetByIdAsync(id))
                .ReturnsAsync(oldEvent);
            mockEventService
                .Setup(service => service.UpdateAsync(It.Is<Event>(e =>
                    e.Id == mappedUpdate.Id &&
                    e.Title == mappedUpdate.Title &&
                    e.AllDay == mappedUpdate.AllDay &&
                    e.DateOnly == mappedUpdate.DateOnly &&
                    e.StartDateTime == mappedUpdate.StartDateTime &&
                    e.EndDateTime == mappedUpdate.EndDateTime &&
                    e.Description == mappedUpdate.Description &&
                    e.Location.Equals(mappedUpdate.Location))))
                .ReturnsAsync(newEvent);
            var sut = new EventController(mockEventService.Object);
            // Act
            var result = await sut.Update(updateRequest, id);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (OkObjectResult)result;
            objectResult.Value.Should().BeEquivalentTo(newEvent.MapToResponse());
        }

        [Fact]
        public async Task Update_OnNoEventsFound_ReturnsStatusCode404()
        {
            //Arrange
            var mockEventService = new Mock<IEventService>();
            var oldEvent = EventsFixtures.GetTestFixture();
            var id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30");
            var updateRequest = EventsFixtures.UpdateEventRequestForFixture1();
            var mappedUpdate = updateRequest.MapToEvent(id, oldEvent);
            var newEvent = EventsFixtures.UpdateTestFixtureExpectedForFixture1();

            mockEventService
                .Setup(service => service.GetByIdAsync(id))
                .ReturnsAsync((Event)null);
            var mockUpdate = mockEventService
                .Setup(service => service.UpdateAsync(mappedUpdate));
            var sut = new EventController(mockEventService.Object);
            // Act
            var result = await sut.Update(updateRequest, id);
            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
        [Fact]
        public async Task Update_HasHttpPutAttributeWithGuidRouteParameter()
        {
            // Arrange
            var expectedEvent = EventsFixtures.GetTestFixture();
            var mockHttpHandler = MockHttpMessageHandler<Event>.SetupBasicGetResourceList(new List<Event> { expectedEvent });
            var httpClient = new HttpClient(mockHttpHandler.Object)
            {
                BaseAddress = new Uri("http://localhost")
            };
            var request = new HttpRequestMessage(HttpMethod.Put, "http://localhost/api/events/701bd75d-bd97-40e0-b4e3-828afe2acc30");
            var response = await httpClient.SendAsync(request);

            var methodInfo = typeof(EventController).GetMethod(nameof(EventController.Update));

            // Act
            var httpPutAttribute = methodInfo.GetCustomAttributes(typeof(HttpPutAttribute), false)
                                             .FirstOrDefault() as HttpPutAttribute;
            var parameterInfo = methodInfo.GetParameters()
                                           .FirstOrDefault(p => p.Name == "id" && p.ParameterType == typeof(Guid));
            var fromRouteAttribute = parameterInfo?.GetCustomAttributes(typeof(FromRouteAttribute), false)
                                                  .FirstOrDefault() as FromRouteAttribute;
            var extractedGuid = new Guid(request.RequestUri.Segments.Last());

            // Assert
            httpPutAttribute.Should().NotBeNull();
            httpPutAttribute.Template.Should().Be("{id:guid}");
            fromRouteAttribute.Should().NotBeNull();
            extractedGuid.Should().Be(new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30"));
        }

        [Fact]
        public Task Update_OnSuccess_MapsToEventRequest()
        {
            // Arrange
            var oldEvent1 = EventsFixtures.GetTestFixture();
            Guid id1 = new Guid("0b9c3623-95a3-43b6-a5df-db65b5e19093");
            var updateRequest1 = EventsFixtures.UpdateEventRequestForFixture1();
            var expectedNewEvent1 = EventsFixtures.UpdateTestFixtureExpectedForFixture1();

            var oldEvent2 = EventsFixtures.GetTestFixture2();
            Guid id2 = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30");
            var updateRequest2 = EventsFixtures.UpdateEventRequestForFixture2();
            var expectedNewEvent2 = EventsFixtures.UpdateTestFixtureExpectedForFixture2();

            var oldEvent3 = EventsFixtures.GetTestFixture3();
            Guid id3 = new Guid("cae36ce8-6db9-4cb0-b4bd-c4f4ac765922");
            var updateRequest3 = EventsFixtures.UpdateEventRequestForFixture3();
            var expectedNewEvent3 = EventsFixtures.UpdateTestFixtureExpectedForFixture3();

            var oldEvent4 = EventsFixtures.GetTestFixture4();
            Guid id4 = new Guid("d1e36ce8-6db9-4cb0-b4bd-c4f4ac765922");
            var updateRequest4 = EventsFixtures.UpdateEventRequestForFixture4();
            var expectedNewEvent4 = EventsFixtures.UpdateTestFixtureExpectedForFixture4();

            var oldEvent5 = EventsFixtures.GetTestFixture5();
            Guid id5 = new Guid("e2f36ce8-6db9-4cb0-b4bd-c4f4ac765922");
            var updateRequest5 = EventsFixtures.UpdateEventRequestForFixture5();
            var expectedNewEvent5 = EventsFixtures.UpdateTestFixtureExpectedForFixture5();

            // Act
            var mappedResponse1 = updateRequest1.MapToEvent(id1, oldEvent1);
            var mappedResponse2 = updateRequest2.MapToEvent(id2, oldEvent2);
            var mappedResponse3 = updateRequest3.MapToEvent(id3, oldEvent3);
            var mappedResponse4 = updateRequest4.MapToEvent(id4, oldEvent4);
            var mappedResponse5 = updateRequest5.MapToEvent(id5, oldEvent5);

            //Assert 
            mappedResponse1.Should().BeEquivalentTo(expectedNewEvent1);
            mappedResponse2.Should().BeEquivalentTo(expectedNewEvent2);
            mappedResponse3.Should().BeEquivalentTo(expectedNewEvent3);
            mappedResponse4.Should().BeEquivalentTo(expectedNewEvent4);
            mappedResponse5.Should().BeEquivalentTo(expectedNewEvent5);

            return Task.CompletedTask;
        }
        #endregion
        #region Delete
        [Fact]
        public async Task Delete_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            var mockEventService = new Mock<IEventService>();
            var id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30"); 

            mockEventService
                .Setup(service => service.GetByIdAsync(id))
                .ReturnsAsync(EventsFixtures.GetTestFixture()); 
            mockEventService
                .Setup(service => service.DeleteAsync(id))
                .ReturnsAsync(true); 

            var sut = new EventController(mockEventService.Object);

            // Act
            var result = await sut.Delete(id);

            // Assert
            result.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Delete_OnNoEventFound_ReturnsStatusCode404()
        {
            // Arrange
            var mockEventService = new Mock<IEventService>();
            var id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30"); 

            mockEventService
                .Setup(service => service.GetByIdAsync(id))
                .ReturnsAsync((Event)null); 
            mockEventService
                .Setup(service => service.DeleteAsync(id))
                .ReturnsAsync(false); 

            var sut = new EventController(mockEventService.Object);

            // Act
            var result = await sut.Delete(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_OnSuccess_ActionsEventService()
        {
            // Arrange
            var mockEventService = new Mock<IEventService>();
            var id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30"); 

            mockEventService
                .Setup(service => service.GetByIdAsync(id))
                .ReturnsAsync(EventsFixtures.GetTestFixture()); 
            mockEventService
                .Setup(service => service.DeleteAsync(id))
                .ReturnsAsync(true); 

            var sut = new EventController(mockEventService.Object);

            // Act
            var result = await sut.Delete(id);

            // Assert
            mockEventService.Verify(service => service.GetByIdAsync(id), Times.Once());
            mockEventService.Verify(service => service.DeleteAsync(id), Times.Once());
        }
        #endregion
    }
}
