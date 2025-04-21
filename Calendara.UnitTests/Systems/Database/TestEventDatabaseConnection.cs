using Calendara.Application.Database;
using Calendara.Application.Models;
using Calendara.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Moq;
using Npgsql;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Calendara.UnitTests.Systems.Database
{
    public class TestEventDatabaseConnection
    {
        private readonly Mock<IConfiguration> _mockConfig;

        public TestEventDatabaseConnection()
        {
            _mockConfig = new Mock<IConfiguration>();
        }

        [Fact]
        public void Constructor_WithParameters_CreatesObject()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase("TestDb1")
                .Options;

            // Act
            var dbConnection = new DatabaseConnection(options, _mockConfig.Object);

            // Assert
            Assert.NotNull(dbConnection);
        }

        [Fact]
        public void Constructor_WithoutParameters_CreatesObject()
        {
            // Act
            var dbConnection = new DatabaseConnection();

            // Assert
            Assert.NotNull(dbConnection);
        }

        [Fact]
        public void Context_ReturnsItself()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase("TestDb2")
                .Options;
            var dbConnection = new DatabaseConnection(options, _mockConfig.Object);

            // Act
            var context = dbConnection.Context;

            // Assert
            Assert.Equal(dbConnection, context);
        }

        [Fact]
        public async Task SaveChangesAsync_SavesData()
        {
            // Arrange
            var uniqueDbName = $"TestDb3_{Guid.NewGuid()}";
            var options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase(databaseName: uniqueDbName)
                .Options;
            var dbConnection = new DatabaseConnection(options, _mockConfig.Object);
            var uniqueId = Guid.NewGuid();
            var testEvent = new Event
            {
                Id = uniqueId,
                Title = "Test Event",
                AllDay = true,
                DateOnly = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
            };

            Assert.Empty(dbConnection.Events);
            dbConnection.Events.Add(testEvent);

            // Act
            var result = await dbConnection.SaveChangesAsync();

            // Assert
            Assert.Equal(1, result);
        }

        [Fact]
        public void OnConfiguring_WithConnectionString_UsesConfigValue()
        {
            // Arrange
            var configSection = new Mock<IConfigurationSection>();
            configSection.Setup(s => s.Value).Returns("TestConnectionString");

            _mockConfig
                .Setup(c => c.GetSection("Database:ConnectionString"))
                .Returns(configSection.Object);

            var optionsBuilder = new DbContextOptionsBuilder<DatabaseConnection>();
            var testConnection = new TestDatabaseConnectionExposed(optionsBuilder.Options, _mockConfig.Object);
            // Resetting options builder to simulate unconfigured state
            optionsBuilder = new DbContextOptionsBuilder<DatabaseConnection>();

            // Act
            testConnection.PublicOnConfiguring(optionsBuilder);

            // Assert
            var extensions = optionsBuilder.Options.Extensions.ToList();
            bool hasNpgsqlExtension = extensions.Any(e => e.GetType().FullName.Contains("Npgsql"));
            Assert.True(hasNpgsqlExtension, "Options should include Npgsql provider");
        }

        [Fact]
        public void EventsProperty_IsNotNull()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DatabaseConnection>()
                .UseInMemoryDatabase("TestDb4")
                .Options;

            // Act
            var dbConnection = new DatabaseConnection(options, _mockConfig.Object);

            // Assert
            Assert.NotNull(dbConnection.Events);
        }

        [Fact]
        public void MigrateAsync_MethodExists()
        {
            // Couldn't find a simple solution to test MigrateAsync so just checking that it exists
            // Arrange
            var dbConnection = new DatabaseConnection();

            // Act & Assert
            var method = typeof(DatabaseConnection).GetMethod("MigrateAsync");
            Assert.NotNull(method);
        }


        // Helper class to expose protected OnConfiguring method for OnConfiguring_WithConnectionString_UsesConfigValue()
        private class TestDatabaseConnectionExposed : DatabaseConnection
        {
            public TestDatabaseConnectionExposed(DbContextOptions<DatabaseConnection> options, IConfiguration configuration)
                : base(options, configuration)
            {
            }

            public void PublicOnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                base.OnConfiguring(optionsBuilder);
            }
        }
    }
}
