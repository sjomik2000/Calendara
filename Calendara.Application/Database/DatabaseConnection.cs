using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calendara.Application.Database
{
    public class DatabaseConnection : DbContext, IDatabaseConnection
    {
        private readonly IConfiguration _configuration;

        public DatabaseConnection(DbContextOptions<DatabaseConnection> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }
        // Parameterless constructor for design-time
        public DatabaseConnection()
        {
        }
        public DbSet<Models.Event> Events { get; set; }

        public DbContext Context => this;

        public async Task<IDbConnection> CreateConnectionAsync()
        {
            string connectionString = Database.GetConnectionString();
            var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task MigrateAsync()
        {
            await Database.MigrateAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (_configuration != null)
                {
                    var connectionString = _configuration["Database:ConnectionString"];
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        optionsBuilder.UseNpgsql(connectionString);
                        return;
                    }
                }
                // backup for null configuration
                optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=events;User ID=eventadmin;Password=admin1;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Event>(entity =>
            {
                //Model building constraints 
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.Property(e => e.AllDay).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.DateOnly) //DateOnly not supported in EF Core
                    .HasConversion(
                        d => d.HasValue
                            ? DateTime.SpecifyKind(d.Value.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc)
                            : (DateTime?)null,
                        d => d.HasValue ? DateOnly.FromDateTime(d.Value) : null);
                entity.OwnsOne(e => e.Location, loc =>
                {
                    loc.Property(g => g.Latitude);
                    loc.Property(g => g.Longitude);
                });
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
