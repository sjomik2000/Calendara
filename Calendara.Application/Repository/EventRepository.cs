using Calendara.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calendara.Application.Repositories
{
    public class EventRepository : IEventRepository         
    {
        private readonly Dictionary<Guid, Event> _eventsRepository = new();
        public Task<bool> CreateAsync(Event eventItem)
        {
            if (_eventsRepository.ContainsKey(eventItem.Id))
            {
                return Task.FromResult(false);
            }

            _eventsRepository.Add(eventItem.Id, eventItem);
            return Task.FromResult(true);
        }
        public Task<bool> DeleteByIdAsync(Guid id)
        {
            var result = _eventsRepository.Remove(id);
            return Task.FromResult(true);
        }

        public Task<bool> ExistsByIdAsync(Guid id)
        {
            return Task.FromResult(_eventsRepository.ContainsKey(id));
        }

        public Task<IEnumerable<Event>> GetAllAsync()
        {
            return Task.FromResult(_eventsRepository.Values.AsEnumerable());
        }

        public Task<Event> GetByIdAsync(Guid id)
        {
            if (_eventsRepository.TryGetValue(id, out var eventItem))
            {
                return Task.FromResult<Event?>(eventItem);
            }
            return Task.FromResult<Event?>(null);
        }

        public Task<bool> UpdateAsync(Event eventItem)
        {
            if (!_eventsRepository.ContainsKey(eventItem.Id))
            {
                return Task.FromResult(false);
            }
            _eventsRepository[eventItem.Id] = eventItem;
            return Task.FromResult(true);
        }
    }
}
