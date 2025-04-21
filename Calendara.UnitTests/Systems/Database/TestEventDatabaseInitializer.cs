using Calendara.Application.Database;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Calendara.UnitTests.Systems.Database
{
    public class TestEventDatabaseInitializer
    {
        private readonly Mock<IDatabaseConnection> _mockDbContext;
        private readonly DbInitializer _sut;
        public TestEventDatabaseInitializer()
        {
            _mockDbContext = new Mock<IDatabaseConnection>();
            _sut = new DbInitializer(_mockDbContext.Object);
        }

        [Fact]
        public async Task InitializeAsync_OnSuccess_Calls_MigrateAsync()
        {
            // Arrange
            _mockDbContext.Setup(db => db.MigrateAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _sut.InitializeAsync();

            // Assert
            _mockDbContext.Verify(db => db.MigrateAsync(), Times.Once());
        }

        [Fact]
        public async Task InitializeAsync_OnSucess_ShowsNoErrors()
        {
            // Arrange
            _mockDbContext.Setup(db => db.MigrateAsync())
                .Returns(Task.CompletedTask);

            // Act
            await _sut.InitializeAsync();

            // Assert
            Assert.True(true); 
        }

        [Fact]
        public async Task InitializeAsync_OnMigrationFailure_ThrowsException()
        {
            // Arrange
            var exception = new DbUpdateException("Database migration failed");
            _mockDbContext.Setup(db => db.MigrateAsync())
                .ThrowsAsync(exception);

            // Act & Assert
            var exceptionResult = await Assert.ThrowsAsync<Exception>(() => _sut.InitializeAsync());
            Assert.Contains("Exception occurred", exceptionResult.Message);
            Assert.Contains("Database migration failed", exceptionResult.Message);
        }

        [Fact]
        public void Constructor_OnCall_InstantiatesDbInitializer()
        {
            // Arrange
            var mockDbContext = new Mock<IDatabaseConnection>();

            // Act
            var initializer = new DbInitializer(mockDbContext.Object);

            // Assert
            Assert.NotNull(initializer);
        }

        [Fact]
        public async Task InitializeAsync_OnFailure_ContainsOriginalErrorMessage()
        {
            // Arrange
            var errorMessage = "Migration specific error";
            _mockDbContext.Setup(db => db.MigrateAsync())
                .ThrowsAsync(new InvalidOperationException(errorMessage));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _sut.InitializeAsync());
            Assert.Contains(errorMessage, exception.Message);
        }
    }
}
