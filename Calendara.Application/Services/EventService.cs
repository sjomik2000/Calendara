using Calendara.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Calendara.Application.Services
{
    public class EventService : IEventService
    {
        public EventService()
        {

        }
        public Task<bool> CreateAsync(Event eventItem)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Event>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Event>> GetByDateAsync(DateTime date)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Event>> GetByDateRangeAsync(DateTime dateStart, DateTime dateEnd)
        {
            throw new NotImplementedException();
        }

        public Task<Event?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Event?> UpdateAsync(Event eventItem)
        {
            throw new NotImplementedException();
        }
    }
}
