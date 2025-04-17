using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Calendara.Application.Models;

namespace Calendara.Application.Services
{
    public interface IEventService
    {
        Task<bool> CreateAsync(Event eventItem);
        Task<Event?> GetByIdAsync(Guid id);

        Task<IEnumerable<Event>> GetAllAsync();

        Task<IEnumerable<Event>> GetByDateRangeAsync(DateTime dateStart, DateTime dateEnd);

        Task<IEnumerable<Event>> GetByDateAsync(DateTime date);

        Task<Event?> UpdateAsync(Event eventItem);

        Task<bool> DeleteAsync(Guid id);
    }
}
