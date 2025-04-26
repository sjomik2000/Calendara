using Calendara.Application.Database;
using Calendara.Application.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendara.Application.Repositories
{
    public class EventRepository : IEventRepository         
    {
        private readonly IDatabaseConnection _dbConnection;

        public EventRepository(IDatabaseConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<bool> CreateAsync(Event eventItem)
        {
            try
            {
                await _dbConnection.Context.Set<Event>().AddAsync(eventItem);
                await _dbConnection.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var eventToDelete = await _dbConnection.Context.Set<Event>().FindAsync(id);
            if (eventToDelete == null)
            {
                return false;
            }

            _dbConnection.Context.Set<Event>().Remove(eventToDelete);
            await _dbConnection.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await _dbConnection.Context.Set<Event>().AnyAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Event>> GetAllAsync()
        {
            return await _dbConnection.Context.Set<Event>()
                .AsNoTracking() // Improves performance for read only calls
                .ToListAsync();
        }

        public async Task<Event> GetByIdAsync(Guid id)
        {
            return await _dbConnection.Context.Set<Event>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> UpdateAsync(Event eventItem)
        {
            try
            {
                var existingEvent = await _dbConnection.Context.Set<Event>().FindAsync(eventItem.Id);
                if (existingEvent == null)
                {
                    return false;
                }

                _dbConnection.Context.Set<Event>().Remove(existingEvent);
                await _dbConnection.SaveChangesAsync();
                await _dbConnection.Context.Set<Event>().AddAsync(eventItem);
                await _dbConnection.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating event: {ex.Message}");
                return false;
            }
        }
    }
}
