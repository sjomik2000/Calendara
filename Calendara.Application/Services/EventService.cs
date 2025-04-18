using Calendara.Application.Models;
using Calendara.Application.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Calendara.Application.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IValidator<Event> _eventValidator;
        public EventService(IEventRepository eventRepository, IValidator<Event> eventValidator)
        {
            _eventRepository = eventRepository;
            _eventValidator = eventValidator;

        }
        public async Task<bool> CreateAsync(Event eventItem)
        {
            await _eventValidator.ValidateAndThrowAsync(eventItem);
            return await _eventRepository.CreateAsync(eventItem);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            return _eventRepository.DeleteByIdAsync(id);
        }

        public Task<IEnumerable<Event>> GetAllAsync()
        {
            return _eventRepository.GetAllAsync();
        }

        public Task<Event?> GetByIdAsync(Guid id)
        {
            return _eventRepository.GetByIdAsync(id);
        }

        public async Task<Event?> UpdateAsync(Event eventItem)
        {
            await _eventValidator.ValidateAndThrowAsync(eventItem);
            var exists = await _eventRepository.ExistsByIdAsync(eventItem.Id);
            if (!exists)
            {
                return null;
            }
            await _eventRepository.UpdateAsync(eventItem);
            return eventItem;
        }
    }
}
