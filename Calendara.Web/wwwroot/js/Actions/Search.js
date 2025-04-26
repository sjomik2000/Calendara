function setupSearchEventForm() {
    const eventsContainer = document.getElementById('events-list');
    const eventsHeader = document.querySelector('.events-header');
    eventsHeader.textContent = 'Search Event';

    fetch('/html/Forms/searchEventForm.html')
        .then(response => response.text())
        .then(html => {
            eventsContainer.innerHTML = html;

            document.getElementById('searchByIdBtn').addEventListener('click', setupSearchById);
            document.getElementById('searchByDateBtn').addEventListener('click', setupSearchByDate);
            document.getElementById('searchByRangeBtn').addEventListener('click', setupSearchByDateRange);
            document.getElementById('searchAllBtn').addEventListener('click', searchAllEvents);
        })
        .catch(error => {
            console.error('Error loading search form:', error);
            eventsContainer.innerHTML = 'Error loading form. Please try again.';
        });
}

function setupSearchById() {
    const eventsContainer = document.getElementById('events-list');

    fetch('/html/Forms/searchByIdForm.html')
        .then(response => response.text())
        .then(html => {
            eventsContainer.innerHTML = html;

            document.getElementById('submitSearchIdBtn').addEventListener('click', searchById);
            document.getElementById('cancelSearchBtn').addEventListener('click', resetEventView);
        })
        .catch(error => {
            console.error('Error loading search by ID form:', error);
            eventsContainer.innerHTML = 'Error loading form. Please try again.';
        });
}

function setupSearchByDate() {
    const eventsContainer = document.getElementById('events-list');

    fetch('/html/Forms/searchByDateForm.html')
        .then(response => response.text())
        .then(html => {
            eventsContainer.innerHTML = html;

            document.getElementById('submitSearchDateBtn').addEventListener('click', searchByDate);
            document.getElementById('cancelSearchBtn').addEventListener('click', resetEventView);

            const numberInputs = document.querySelectorAll('.date-part');
            numberInputs.forEach(input => {
                input.addEventListener('input', function () {
                    this.value = this.value.replace(/[^0-9]/g, '');
                });
            });
        })
        .catch(error => {
            console.error('Error loading search by date form:', error);
            eventsContainer.innerHTML = 'Error loading form. Please try again.';
        });
}

function setupSearchByDateRange() {
    const eventsContainer = document.getElementById('events-list');

    fetch('/html/Forms/searchByDateRangeForm.html')
        .then(response => response.text())
        .then(html => {
            eventsContainer.innerHTML = html;

            document.getElementById('submitSearchRangeBtn').addEventListener('click', searchByDateRange);
            document.getElementById('cancelSearchBtn').addEventListener('click', resetEventView);

            const numberInputs = document.querySelectorAll('.date-part');
            numberInputs.forEach(input => {
                input.addEventListener('input', function () {
                    this.value = this.value.replace(/[^0-9]/g, '');
                });
            });
        })
        .catch(error => {
            console.error('Error loading search by date range form:', error);
            eventsContainer.innerHTML = 'Error loading form. Please try again.';
        });
}

async function searchById() {
    const eventId = document.getElementById('eventId').value.trim();

    if (!eventId) {
        showNotification('Please enter a valid ID', 'error');
        return;
    }

    try {
        await loadEvents();
        const response = await fetch(`https://localhost:5002/api/events/${eventId}`);

        if (response.ok) {
            const eventData = await response.json();
            displaySearchResults([eventData]);
            showNotification('Event has been retrieved successfully', 'success');
        } else {
            showNotification('Event not found, please enter a valid ID', 'error');
        }
    } catch (error) {
        console.error('Error searching for event:', error);
        showNotification('Error connecting to server', 'error');
    }
}

async function searchByDate() {
    const day = document.getElementById('searchDay').value.trim();
    const month = document.getElementById('searchMonth').value.trim();
    const year = document.getElementById('searchYear').value.trim();

    if (!day || !month || !year) {
        showNotification('Please enter a valid date', 'error');
        return;
    }

    const dateString = `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
    const searchDate = new Date(dateString);

    try {
        await loadEvents();
        const response = await fetch('https://localhost:5002/api/events');

        if (response.ok) {
            const data = await response.json();
            const allEvents = data.events || [];

            console.log('All events:', allEvents);

            const filteredEvents = allEvents.filter(event => {
                if (event.allDay && event.dateOnly === dateString) {
                    return true;
                }

                if (event.startDateTime && event.endDateTime) {
                    const start = new Date(event.startDateTime);
                    const end = new Date(event.endDateTime);

                    const eventStartDate = new Date(start.getFullYear(), start.getMonth(), start.getDate());
                    const eventEndDate = new Date(end.getFullYear(), end.getMonth(), end.getDate());
                    const searchDateOnly = new Date(searchDate.getFullYear(), searchDate.getMonth(), searchDate.getDate());

                    return searchDateOnly >= eventStartDate && searchDateOnly <= eventEndDate;
                }

                return false;
            });

            console.log('Filtered events for date', dateString, ':', filteredEvents);

            const processedResults = filteredEvents.map(event => {
                const processedEvent = { ...event };

                if (event.startDateTime && event.endDateTime && !event.dateOnly) {
                    processedEvent.dateOnly = dateString;
                }

                return processedEvent;
            });

            displaySearchResults(processedResults);

            const eventCount = processedResults.length;
            if (eventCount > 0) {
                const message = eventCount === 1 ?
                    '1 event has been found' :
                    `${eventCount} events have been found`;
                showNotification(message, 'success');
            } else {
                showNotification('No events found', 'success');
            }
        } else {
            showNotification('Error searching for events', 'error');
        }
    } catch (error) {
        console.error('Error searching for events:', error);
        showNotification('Error connecting to server', 'error');
    }
}

async function searchByDateRange() {
    const startDay = document.getElementById('startSearchDay').value.trim();
    const startMonth = document.getElementById('startSearchMonth').value.trim();
    const startYear = document.getElementById('startSearchYear').value.trim();

    const endDay = document.getElementById('endSearchDay').value.trim();
    const endMonth = document.getElementById('endSearchMonth').value.trim();
    const endYear = document.getElementById('endSearchYear').value.trim();

    if (!startDay || !startMonth || !startYear || !endDay || !endMonth || !endYear) {
        showNotification('Please enter valid start and end dates', 'error');
        return;
    }

    const startDateString = `${startYear}-${startMonth.padStart(2, '0')}-${startDay.padStart(2, '0')}`;
    const endDateString = `${endYear}-${endMonth.padStart(2, '0')}-${endDay.padStart(2, '0')}`;

    const startDate = new Date(startDateString);
    const endDate = new Date(endDateString);

    if (endDate < startDate) {
        showNotification('End date cannot be earlier than start date', 'error');
        return;
    }

    try {
        await loadEvents();
        const response = await fetch('https://localhost:5002/api/events');

        if (response.ok) {
            const data = await response.json();
            const allEvents = data.events || [];

            console.log('All events:', allEvents);

            const filteredEvents = allEvents.filter(event => {
                if (event.allDay && event.dateOnly) {
                    const eventDate = new Date(event.dateOnly);
                    return eventDate >= startDate && eventDate <= endDate;
                }

                if (event.startDateTime && event.endDateTime) {
                    const eventStart = new Date(event.startDateTime);
                    const eventEnd = new Date(event.endDateTime);

                    const eventStartDate = new Date(eventStart.getFullYear(), eventStart.getMonth(), eventStart.getDate());
                    const eventEndDate = new Date(eventEnd.getFullYear(), eventEnd.getMonth(), eventEnd.getDate());

                    return eventStartDate <= endDate && eventEndDate >= startDate;
                }

                return false;
            });

            console.log('Filtered events for date range', startDateString, 'to', endDateString, ':', filteredEvents);

            displaySearchResults(filteredEvents);

            const eventCount = filteredEvents.length;
            if (eventCount > 0) {
                const message = eventCount === 1 ?
                    '1 event has been found' :
                    `${eventCount} events have been found`;
                showNotification(message, 'success');
            } else {
                showNotification('No events found', 'success');
            }
        } else {
            showNotification('Error searching for events', 'error');
        }
    } catch (error) {
        console.error('Error searching for events:', error);
        showNotification('Error connecting to server', 'error');
    }
}

async function searchAllEvents() {
    try {
        const response = await fetch('https://localhost:5002/api/events');

        if (response.ok) {
            const data = await response.json();
            const searchResults = data.events;

            console.log(`Total events received: ${searchResults.length}`);

            displaySearchResults(searchResults);

            const eventCount = searchResults.length;
            if (eventCount > 0) {
                const message = eventCount === 1 ?
                    '1 event has been found' :
                    `${eventCount} events have been found`;
                showNotification(message, 'success');
            } else {
                showNotification('No events found', 'success');
            }
        } else {
            showNotification('Error retrieving events', 'error');
        }
    } catch (error) {
        console.error('Error retrieving events:', error);
        showNotification('Error connecting to server', 'error');
    }
}