using Calendara.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calendara.Application.Repositories 
{
    public interface IEventRepository
    {
        Task<bool> CreateAsync(Event eventItem);

        Task<Event?> GetByIdAsync(Guid id);

        Task<IEnumerable<Event>> GetAllAsync();

        Task<bool> UpdateAsync(Event eventItem);

        Task<bool> DeleteByIdAsync(Guid id);

        Task<bool> ExistsByIdAsync(Guid id);
    }
}
