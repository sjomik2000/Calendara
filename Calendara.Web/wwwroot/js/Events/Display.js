function handleDayClick(daySquare, dateString) {
    // previous selection removal
    const previousSelected = document.querySelector('.day.selected');
    if (previousSelected) {
        previousSelected.classList.remove('selected');
    }

    const eventsHeader = document.querySelector('.events-header');
    if (eventsHeader.textContent !== 'Selected Events') {
        eventsHeader.textContent = 'Selected Events';
    }

    daySquare.classList.add('selected');

    displayEvents(getEventsForDate(dateString), dateString);
}

function displayEvents(events, date) {
    const eventsContainer = document.getElementById('events-list');

    if (!events || events.length === 0) {
        eventsContainer.innerHTML = '<div class="no-events">No events for this day.</div>';
        return;
    }

    // Sorting all day events, then sorting by start time
    const sortedEvents = [...events].sort((a, b) => {
        if (a.allDay && !b.allDay) return -1;
        if (!a.allDay && b.allDay) return 1;
        if (a.startDateTime && b.startDateTime) {
            return new Date(a.startDateTime) - new Date(b.startDateTime);
        }
        return 0;
    });

    let eventsHTML = '';
    sortedEvents.forEach(event => {
        eventsHTML += generateEventHTML(event);
    });

    eventsContainer.innerHTML = eventsHTML;
}

function generateEventHTML(event) {
    // Rounding location to 3d.p for better readability
    let locationHTML = '';
    if (event.location) {
        const lat = parseFloat(event.location.latitude).toFixed(3);
        const lng = parseFloat(event.location.longitude).toFixed(3);
        const mapsUrl = `https://www.google.com/maps/search/?api=1&query=${event.location.latitude},${event.location.longitude}`;

        locationHTML = `<div class="event-location">
            Location: <a href="${mapsUrl}" target="_blank" class="location-link">{${lat}, ${lng}}</a>
        </div>`;
    }

    const formattedDateTime = formatEventDateTime(event);

    return `
        <div class="event-list-item">
            <div class="event-title">${event.title}</div>
            <div class="event-date">${formattedDateTime}</div>
            ${event.description ? `<div class="event-description">${event.description}</div>` : ''}
            ${locationHTML}
            <div class="event-location">ID: ${event.id}</div>
        </div>
    `;
}

function formatEventDateTime(event) {
    const formatDate = (date) => {
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();
        return `${day}/${month}/${year}`;
    };

    if (event.allDay) {
        if (event.dateOnly) {
            const date = new Date(event.dateOnly);
            return `${formatDate(date)} All day`;
        }
        return 'All day';
    }

    if (event.startDateTime && event.endDateTime) {
        const start = new Date(event.startDateTime);
        const end = new Date(event.endDateTime);

        const startDate = start.toISOString().split('T')[0];
        const endDate = end.toISOString().split('T')[0];
        const isMultiDay = startDate !== endDate;

        const formatTimeOnly = (date) => {
            return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false });
        };

        if (isMultiDay) {
            return `${formatDate(start)} ${formatTimeOnly(start)} - ${formatDate(end)} ${formatTimeOnly(end)}`;
        } else {
            return `${formatDate(start)} ${formatTimeOnly(start)} - ${formatTimeOnly(end)}`;
        }
    }

    return '';
}

function displaySearchResults(events) {
    if (!events || events.length === 0) {
        const eventsContainer = document.getElementById('events-list');
        eventsContainer.innerHTML = '<div class="no-events">No events found.</div>';
        return;
    }

    const sortedEvents = [...events].sort((a, b) => {
        const dateA = a.dateOnly || (a.startDateTime ? new Date(a.startDateTime).toISOString().split('T')[0] : null);
        const dateB = b.dateOnly || (b.startDateTime ? new Date(b.startDateTime).toISOString().split('T')[0] : null);

        if (dateA !== dateB) {
            return dateA.localeCompare(dateB);
        }

        if (a.allDay && !b.allDay) return -1;
        if (!a.allDay && b.allDay) return 1;

        if (a.startDateTime && b.startDateTime) {
            return new Date(a.startDateTime) - new Date(b.startDateTime);
        }
        return 0;
    });

    let eventsHTML = '';
    sortedEvents.forEach(event => {
        eventsHTML += generateEventHTML(event);
    });

    const eventsContainer = document.getElementById('events-list');
    eventsContainer.innerHTML = eventsHTML;
}

function resetEventView() {
    const eventsHeader = document.querySelector('.events-header');
    eventsHeader.textContent = 'Selected Events';

    const eventsContainer = document.getElementById('events-list');
    eventsContainer.innerHTML = '<div class="no-events">No events selected. Click on a day to view events.</div>';
}