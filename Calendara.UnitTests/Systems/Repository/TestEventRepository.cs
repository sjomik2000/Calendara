using Calendara.Application.Database;
using Calendara.Application.Models;
using Calendara.Application.Repositories;
using Calendara.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Calendara.UnitTests.Systems.Repository
{
    public class TestEventRepository
    {
        [Fact]
        public async Task CreateAsync_OnSuccess_ReturnsTrue()
        {
            // Arrange
            var mockDbConnection = new Mock<IDatabaseConnection>();
            var mockContext = new Mock<DbContext>();
            var mockDbSet = new Mock<DbSet<Event>>();

            mockDbConnection.Setup(db => db.Context).Returns(mockContext.Object);
            mockContext.Setup(c => c.Set<Event>()).Returns(mockDbSet.Object);
            mockDbConnection.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);

            var repository = new EventRepository(mockDbConnection.Object);
            var testEvent = EventsFixtures.GetTestFixture();

            // Act
            var result = await repository.CreateAsync(testEvent);

            // Assert
            Assert.True(result);
            mockDbConnection.Verify(db => db.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_OnFailure_ReturnsFalse()
        {
            // Arrange
            var mockDbConnection = new Mock<IDatabaseConnection>();
            var mockContext = new Mock<DbContext>();
            var mockDbSet = new Mock<DbSet<Event>>();

            mockDbConnection.Setup(db => db.Context).Returns(mockContext.Object);
            mockContext.Setup(c => c.Set<Event>()).Returns(mockDbSet.Object);
            mockDbConnection.Setup(db => db.SaveChangesAsync()).ThrowsAsync(new Exception("Test exception"));

            var repository = new EventRepository(mockDbConnection.Object);
            var testEvent = EventsFixtures.GetTestFixture();

            // Act
            var result = await repository.CreateAsync(testEvent);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenEventExists_ReturnsTrue()
        {
            // Arrange
            var mockDbConnection = new Mock<IDatabaseConnection>();
            var mockContext = new Mock<DbContext>();
            var mockDbSet = new Mock<DbSet<Event>>();

            var id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30");
            var testEvent = EventsFixtures.GetTestFixture();

            mockDbConnection.Setup(db => db.Context).Returns(mockContext.Object);
            mockContext.Setup(c => c.Set<Event>()).Returns(mockDbSet.Object);
            mockDbSet.Setup(set => set.FindAsync(id)).ReturnsAsync(testEvent);
            mockDbConnection.Setup(db => db.SaveChangesAsync()).ReturnsAsync(1);

            var repository = new EventRepository(mockDbConnection.Object);

            // Act
            var result = await repository.DeleteByIdAsync(id);

            // Assert
            Assert.True(result);
            mockDbSet.Verify(set => set.Remove(testEvent), Times.Once);
            mockDbConnection.Verify(db => db.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenEventDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var mockDbConnection = new Mock<IDatabaseConnection>();
            var mockContext = new Mock<DbContext>();
            var mockDbSet = new Mock<DbSet<Event>>();

            var id = Guid.NewGuid();

            mockDbConnection.Setup(db => db.Context).Returns(mockContext.Object);
            mockContext.Setup(c => c.Set<Event>()).Returns(mockDbSet.Object);
            mockDbSet.Setup(set => set.FindAsync(id)).ReturnsAsync((Event)null);

            var repository = new EventRepository(mockDbConnection.Object);

            // Act
            var result = await repository.DeleteByIdAsync(id);

            // Assert
            Assert.False(result);
            mockDbSet.Verify(set => set.Remove(It.IsAny<Event>()), Times.Never);
            mockDbConnection.Verify(db => db.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenEventExists_ReturnsTrue()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: $"Events_Update_Success_{Guid.NewGuid()}")
                .Options;
            using (var dbContext = new DatabaseConnection(options, new Mock<IConfiguration>().Object))
            {
                var testEvent = EventsFixtures.GetTestFixture();
                dbContext.Events.Add(testEvent);
                await dbContext.SaveChangesAsync();
            }
            using (var dbContext = new DatabaseConnection(options, new Mock<IConfiguration>().Object))
            {
                var repository = new EventRepository(dbContext);
                var updatedEvent = new Event
                {
                    Id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30"),
                    Title = "Updated Title",
                    AllDay = false,
                    DateOnly = null,
                    StartDateTime = new DateTime(2025, 10, 15, 13, 0, 0),
                    EndDateTime = new DateTime(2025, 10, 15, 14, 0, 0),
                    Description = "Updated description",
                    Location = new GeoCoordinate(52.477973735285154, -1.8987571127313878)
                };

                // Act
                var result = await repository.UpdateAsync(updatedEvent);

                // Assert
                Assert.True(result);
            }

            // Verifying the update was persisted
            using (var dbContext = new DatabaseConnection(options, new Mock<IConfiguration>().Object))
            {
                var id = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30");
                var updatedEntity = await dbContext.Events.FindAsync(id);

                Assert.NotNull(updatedEntity);
                Assert.Equal("Updated Title", updatedEntity.Title);
                Assert.Equal("Updated description", updatedEntity.Description);
            }
        }

        [Fact]
        public async Task UpdateAsync_WhenEventDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: $"Events_Update_Failure_{Guid.NewGuid()}")
                .Options;

            var mockConfig = new Mock<IConfiguration>();
            var dbContext = new DatabaseConnection(options, mockConfig.Object);
            var dbConnection = (IDatabaseConnection)dbContext;
            var nonExistentEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = "Non-existent Event",
                AllDay = true,
                DateOnly = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                StartDateTime = null,
                EndDateTime = null,
                Description = "This event doesn't exist in the database",
                Location = null
            };

            var repository = new EventRepository(dbConnection);

            // Act
            var result = await repository.UpdateAsync(nonExistentEvent);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetByIdAsync_WhenEventExists_ReturnsEvent()
        {
            // Arrange
            var testEvent = EventsFixtures.GetTestFixture();
            var expectedId = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30"); 

            var options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: $"Events_GetById_{expectedId}")
                .Options;
            using (var dbContext = new DatabaseConnection(options, new Mock<IConfiguration>().Object))
            {
                dbContext.Events.Add(testEvent);
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new DatabaseConnection(options, new Mock<IConfiguration>().Object))
            {
                var repository = new EventRepository(dbContext);

                // Act
                var result = await repository.GetByIdAsync(expectedId);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(expectedId, result.Id);
                Assert.Equal(testEvent.Title, result.Title);
                Assert.Equal("This is a sample event 1.", result.Description);
            }
        }

        [Fact]
        public async Task GetByIdAsync_WhenEventDoesNotExist_ReturnsNull()
        {
            // Arrange
            var nonExistentId = new Guid("e2f36ce8-6db9-4cb0-b4bd-c4f4ac765922");

            var options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: $"Events_GetByIdNotFound_{nonExistentId}")
                .Options;

            using (var dbContext = new DatabaseConnection(options, new Mock<IConfiguration>().Object))
            {
                var repository = new EventRepository(dbContext);

                // Act
                var result = await repository.GetByIdAsync(nonExistentId);

                // Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task GetAllAsync_WhenEventsExist_ReturnsAllEvents()
        {
            // Arrange
            var testEventId = new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30");

            var options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: $"Events_GetAll_{testEventId}")
                .Options;

            var testEvents = EventsFixtures.GetAllTestFixtures();

            using (var dbContext = new DatabaseConnection(options, new Mock<IConfiguration>().Object))
            {
                dbContext.Events.AddRange(testEvents);
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new DatabaseConnection(options, new Mock<IConfiguration>().Object))
            {
                var repository = new EventRepository(dbContext);

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Equal(testEvents.Count, result.Count());
                var resultList = result.ToList();
                Assert.Contains(resultList, e => e.Id == new Guid("701bd75d-bd97-40e0-b4e3-828afe2acc30"));
                Assert.Contains(resultList, e => e.Id == new Guid("0b9c3623-95a3-43b6-a5df-db65b5e19093"));
                Assert.Contains(resultList, e => e.Id == new Guid("cae36ce8-6db9-4cb0-b4bd-c4f4ac765922"));
            }
        }

        [Fact]
        public async Task GetAllAsync_WhenNoEventsExist_ReturnsEmptyList()
        {
            // Arrange
            var unusedId = new Guid("d1e36ce8-6db9-4cb0-b4bd-c4f4ac765922");

            var options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: $"Events_GetAllEmpty_{unusedId}")
                .Options;

            using (var dbContext = new DatabaseConnection(options, new Mock<IConfiguration>().Object))
            {
                var repository = new EventRepository(dbContext);

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }

        [Fact]
        public void Constructor_Instantiates_DbConnection()
        {
            // Arrange
            var mockDbConnection = new Mock<IDatabaseConnection>();

            // Act
            var repository = new EventRepository(mockDbConnection.Object);

            // Assert
            Assert.NotNull(repository);
        }
    }
}
