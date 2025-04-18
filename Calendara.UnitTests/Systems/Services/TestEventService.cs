using Calendara.Application.Models;
using Calendara.Application.Repositories;
using Calendara.Application.Services;
using Calendara.UnitTests.Fixtures;
using Calendara.UnitTests.Helpers;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Calendara.UnitTests.Systems.Services
{
    public class TestEventService
    {
        private readonly Mock<IEventRepository> _mockEventRepository;
        private readonly Mock<IValidator<Event>> _mockEventValidator;
        private readonly EventService _sut;

        public TestEventService()
        {
            _mockEventRepository = new Mock<IEventRepository>();
            _mockEventValidator = new Mock<IValidator<Event>>();
            _sut = new EventService(_mockEventRepository.Object, _mockEventValidator.Object);
        }

        #region CreateAsync
        [Fact]
        public async Task CreateAsync_OnSuccess_ReturnsTrue()
        {
            // Arrange
            var eventItem = EventsFixtures.GetTestFixture();
            ValidatorMockSetup.SetupValidationSuccess(_mockEventValidator);
            _mockEventRepository
                .Setup(repo => repo.CreateAsync(eventItem))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.CreateAsync(eventItem);

            // Assert
            Assert.True(result);
            _mockEventValidator.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<Event>>(), default), Times.Once);
            _mockEventRepository.Verify(repo => repo.CreateAsync(eventItem), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_OnValidationFailure_ThrowsValidationException()
        {
            // Arrange
            var eventItem = EventsFixtures.GetInvalidEventFixture();
            var validationFailures = new List<ValidationFailure>{
                new ValidationFailure("Title", "Title must be provided.")};
            var validationResult = new ValidationResult(validationFailures);
            ValidatorMockSetup.SetupThrowValidationException(_mockEventValidator, validationFailures);


            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _sut.CreateAsync(eventItem));
            Assert.Contains("Title must be provided.", exception.Message);
            _mockEventRepository.Verify(repo => repo.CreateAsync(It.IsAny<Event>()), Times.Never);
        }
        #endregion
        #region GetByIdAsync
        [Fact]
        public async Task GetByIdAsync_OnSuccess_ReturnsEvent()
        {
            // Arrange
            var eventId = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30");
            var expectedEvent = EventsFixtures.GetTestFixture();
            _mockEventRepository
                .Setup(repo => repo.GetByIdAsync(eventId))
                .ReturnsAsync(expectedEvent);

            // Act
            var result = await _sut.GetByIdAsync(eventId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedEvent, result);
            _mockEventRepository.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_OnEventNotFound_ReturnsNull()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            _mockEventRepository
                .Setup(repo => repo.GetByIdAsync(eventId))
                .ReturnsAsync((Event)null);

            // Act
            var result = await _sut.GetByIdAsync(eventId);

            // Assert
            Assert.Null(result);
            _mockEventRepository.Verify(repo => repo.GetByIdAsync(eventId), Times.Once);
        }
        #endregion
        #region GetAllAsync
        [Fact]
        public async Task GetAllAsync_OnSuccess_ReturnsAllEvents()
        {
            // Arrange
            var expectedEvents = EventsFixtures.GetAllTestFixtures();
            _mockEventRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(expectedEvents);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            var eventsList = result.ToList();
            Assert.Equal(expectedEvents.Count, eventsList.Count);
            Assert.Equal(expectedEvents, eventsList);
            _mockEventRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_WithNoEvents_ReturnsEmptyList()
        {
            // Arrange
            var emptyList = new List<Event>();
            _mockEventRepository
                .Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(emptyList);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            Assert.Empty(result);
            _mockEventRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
        #endregion
        #region UpdateAsync
        [Fact]
        public async Task UpdateAsync_OnSuccess_ReturnsUpdatedEvent()
        {
            // Arrange
            var eventToUpdate = EventsFixtures.GetTestFixture();
            ValidatorMockSetup.SetupValidationSuccess(_mockEventValidator);

            _mockEventRepository
                .Setup(repo => repo.ExistsByIdAsync(eventToUpdate.Id))
                .ReturnsAsync(true);

            _mockEventRepository
                .Setup(repo => repo.UpdateAsync(eventToUpdate))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.UpdateAsync(eventToUpdate);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(eventToUpdate, result);
            _mockEventValidator.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<Event>>(), default), Times.Once);
            _mockEventRepository.Verify(repo => repo.ExistsByIdAsync(eventToUpdate.Id), Times.Once);
            _mockEventRepository.Verify(repo => repo.UpdateAsync(eventToUpdate), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_OnEventNotFound_ReturnsNull()
        {
            // Arrange
            var eventToUpdate = EventsFixtures.GetTestFixture();
            ValidatorMockSetup.SetupValidationSuccess(_mockEventValidator);

            _mockEventRepository
                .Setup(repo => repo.ExistsByIdAsync(eventToUpdate.Id))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.UpdateAsync(eventToUpdate);

            // Assert
            Assert.Null(result);
            _mockEventValidator.Verify(v => v.ValidateAsync(It.IsAny<ValidationContext<Event>>(), default), Times.Once);
            _mockEventRepository.Verify(repo => repo.ExistsByIdAsync(eventToUpdate.Id), Times.Once);
            _mockEventRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_OnValidationFailure_ThrowsValidationException()
        {
            // Arrange
            var eventToUpdate = EventsFixtures.GetInvalidEventFixture();
            var validationFailures = new List<ValidationFailure>{
                new ValidationFailure("Title", "Title must be provided.")
            };
            ValidatorMockSetup.SetupThrowValidationException(_mockEventValidator, validationFailures);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _sut.UpdateAsync(eventToUpdate));
            Assert.Contains("Title must be provided.", exception.Message);
            _mockEventRepository.Verify(repo => repo.ExistsByIdAsync(It.IsAny<Guid>()), Times.Never);
            _mockEventRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Event>()), Times.Never);
        }
        #endregion
        #region DeleteAsync
        [Fact]
        public async Task DeleteAsync_OnSuccess_ReturnsTrue()
        {
            // Arrange
            var eventId = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30");
            _mockEventRepository
                .Setup(repo => repo.DeleteByIdAsync(eventId))
                .ReturnsAsync(true);

            // Act
            var result = await _sut.DeleteAsync(eventId);

            // Assert
            Assert.True(result);
            _mockEventRepository.Verify(repo => repo.DeleteByIdAsync(eventId), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_OnEventNotFound_ReturnsFalse()
        {
            // Arrange
            var eventId = Guid.NewGuid();
            _mockEventRepository
                .Setup(repo => repo.DeleteByIdAsync(eventId))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.DeleteAsync(eventId);

            // Assert
            Assert.False(result);
            _mockEventRepository.Verify(repo => repo.DeleteByIdAsync(eventId), Times.Once);
        }
        #endregion
    }
}


