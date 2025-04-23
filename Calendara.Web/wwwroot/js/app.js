let nav = 0; // Navigation index from current month
let events = [];
let sortedEventsByDate = {};
const calendar = document.getElementById('calendar');
const weekdays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
const monthNames = ['January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'];



document.addEventListener('DOMContentLoaded', () => {
    console.log('Calendara web application initialized');
    loadEvents();
    initLoad();
    initActionButtons();
});

//Date formatting

function formatDateString(year, month, day) {
    return `${year}-${String(month).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
}

function getEventDate(event) {
    return event.dateOnly ||
        (event.startDateTime ? new Date(event.startDateTime).toISOString().split('T')[0] : null);
}

function formatEventTime(event) {
    if (event.allDay) {
        return 'All day';
    }

    if (event.startDateTime && event.endDateTime) {
        const start = new Date(event.startDateTime);
        const end = new Date(event.endDateTime);

        // Check for multiple day event
        const startDate = start.toISOString().split('T')[0];
        const endDate = end.toISOString().split('T')[0];
        const isMultiDay = startDate !== endDate;

        // helpers formating 
        const formatDate = (date) => {
            const day = String(date.getDate()).padStart(2, '0');
            const month = String(date.getMonth() + 1).padStart(2, '0');
            const year = date.getFullYear();
            return `${day}/${month}/${year}`;
        };

        const formatTimeOnly = (date) => {
            return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit', hour12: false });
        };

        const formatDateAndTime = (date) => {
            return `${formatDate(date)} ${formatTimeOnly(date)}`;
        };

        if (isMultiDay) {
            return `${formatDateAndTime(start)} - ${formatDateAndTime(end)}`;
        } else {
            return `${formatTimeOnly(start)} - ${formatTimeOnly(end)}`;
        }
    }

    return '';
}

//Loading & Filtering

async function loadEvents() {
    try {
        const response = await fetch('https://localhost:5002/api/events');
        const data = await response.json();
        console.log('Events loaded:', data);

        // Process multi-day events
        events = data.events.flatMap(event => {
            if (!(event.startDateTime && event.endDateTime)) {
                return [event];
            }
            const start = new Date(event.startDateTime);
            const end = new Date(event.endDateTime);

            // Comparing dates
            const startDate = new Date(start);
            startDate.setHours(0, 0, 0, 0);

            const endDate = new Date(end);
            endDate.setHours(0, 0, 0, 0);

            // Single-day event
            if (startDate.getTime() === endDate.getTime()) {
                return [event];
            }

            // Multiday event copies for each day
            const result = [];
            const currentDate = new Date(startDate);

            while (currentDate <= endDate) {
                const eventCopy = { ...event };
                eventCopy.dateOnly = currentDate.toISOString().split('T')[0];
                result.push(eventCopy);
                currentDate.setDate(currentDate.getDate() + 1);
            }

            return result;
        });
    } catch (error) {
        console.error('Error loading events:', error);
    }
}

function getEventsForDate(dateString) {
    return events.filter(event => getEventDate(event) === dateString);
}

//Calendar rendering

function initLoad() {
    const date = new Date();
    const currentMonth = date.getMonth() + nav;
    const year = date.getFullYear() + Math.floor(currentMonth / 12);
    const month = ((currentMonth % 12) >= 0) ? (currentMonth % 12) : (currentMonth % 12 + 12);

    renderCalendarHeader(year, month);
    setupNavButtons();
    calendar.innerHTML = '';
    renderCalendarDays(year, month);
}

function renderCalendarHeader(year, month) {
    const monthDisplay = document.getElementById('header');
    monthDisplay.innerHTML = `
        <div class="month-nav">
            <button id="prevButton" class="nav-button"><</button>
            <div id="monthDisplay">${monthNames[month]}</div>
            <button id="nextButton" class="nav-button">></button>
        </div>
        <div class="year-nav">
            <button id="prevYearButton" class="nav-button year-button"><<</button>
            <div id="yearDisplay">${year}</div>
            <button id="nextYearButton" class="nav-button year-button">>></button>
        </div>
    `;
}

function setupNavButtons() {
    const navActions = {
        'prevButton': -1,
        'nextButton': 1,
        'prevYearButton': -12,
        'nextYearButton': 12
    };

    for (const [id, value] of Object.entries(navActions)) {
        document.getElementById(id).addEventListener('click', () => {
            nav += value;
            refreshCalendar();
        });
    }
}
function refreshCalendar() {
    calendar.innerHTML = '';
    initLoad();
}

function renderCalendarDays(year, month) {
    const firstDayOfMonth = new Date(year, month, 1);
    const daysInMonth = new Date(year, month + 1, 0).getDate();

    // Changing start weekday from Sunday to Monday (UK format)
    const firstDayWeekday = (firstDayOfMonth.getDay() + 6) % 7;
    const daysInPrevMonth = new Date(year, month, 0).getDate();

    for (let i = 1; i <= 42; i++) {
        const daySquare = document.createElement('div');
        daySquare.classList.add('day');

        if (i <= firstDayWeekday) {
            renderPreviousMonthDay(daySquare, i, firstDayWeekday, daysInPrevMonth, month, year);
        }
        else if (i > firstDayWeekday && i <= firstDayWeekday + daysInMonth) {
            renderCurrentMonthDay(daySquare, i, firstDayWeekday, month, year);
        }
        else {
            renderNextMonthDay(daySquare, i, firstDayWeekday, daysInMonth, month, year);
        }

        calendar.appendChild(daySquare);
    }
}

function renderPreviousMonthDay(daySquare, index, firstDayWeekday, daysInPrevMonth, month, year) {
    const prevMonthDay = daysInPrevMonth - firstDayWeekday + index;
    daySquare.innerText = prevMonthDay;
    daySquare.classList.add('padding');

    // Data for filtering events
    const prevMonth = month === 0 ? 11 : month - 1;
    const prevYear = month === 0 ? year - 1 : year;
    daySquare.dataset.date = formatDateString(prevYear, prevMonth + 1, prevMonthDay);
}

function renderCurrentMonthDay(daySquare, index, firstDayWeekday, month, year) {
    const currentDay = index - firstDayWeekday;

    // day number 
    const dayNumberDiv = document.createElement('div');
    dayNumberDiv.classList.add('day-number');
    dayNumberDiv.innerText = currentDay;
    daySquare.appendChild(dayNumberDiv);

    // Check for current day
    const today = new Date();
    if (nav === 0 && currentDay === today.getDate() &&
        month === today.getMonth() && year === today.getFullYear()) {
        daySquare.classList.add('currentDay');
    }

    // Data for filtering events
    const dateString = formatDateString(year, month + 1, currentDay);
    daySquare.dataset.date = dateString;

    addEventIndicators(daySquare, dateString);

    // click handler
    daySquare.addEventListener('click', () => handleDayClick(daySquare, dateString));
}

function renderNextMonthDay(daySquare, index, firstDayWeekday, daysInMonth, month, year) {
    const nextMonthDay = index - (firstDayWeekday + daysInMonth);
    daySquare.innerText = nextMonthDay;
    daySquare.classList.add('padding');

    // Data for filtering events
    const nextMonth = month === 11 ? 0 : month + 1;
    const nextYear = month === 11 ? year + 1 : year;
    daySquare.dataset.date = formatDateString(nextYear, nextMonth + 1, nextMonthDay);
}

function addEventIndicators(daySquare, dateString) {
    const eventsForDay = getEventsForDate(dateString);

    if (eventsForDay.length > 0) {
        const eventSummaryDiv = document.createElement('div');
        eventSummaryDiv.classList.add('event-summary');

        const eventText = eventsForDay.length === 1 ? '1 event' : `${eventsForDay.length} events`;
        eventSummaryDiv.innerText = eventText;

        daySquare.appendChild(eventSummaryDiv);
    }
}

//Event handling
function handleDayClick(daySquare, dateString) {
    // previous selection removal
    const previousSelected = document.querySelector('.day.selected');
    if (previousSelected) {
        previousSelected.classList.remove('selected');
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

    // Sorting all day events at the top, then sort by start time
    const sortedEvents = [...events].sort((a, b) => {
        if (a.allDay && !b.allDay) return -1;
        if (!a.allDay && b.allDay) return 1;
        if (a.startDateTime && b.startDateTime) {
            return new Date(a.startDateTime) - new Date(b.startDateTime);
        }
        return 0;
    });

    // HTML generation for each event in selected day
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

    return `
        <div class="event-list-item">
            <div class="event-title">${event.title}</div>
            <div class="event-date">${formatEventTime(event)}</div>
            ${event.description ? `<div class="event-description">${event.description}</div>` : ''}
            ${locationHTML}
            <div class="event-location">ID: ${event.id}</div>
        </div>
    `;
}

//Action buttons for events
function initActionButtons() {
    const actionsHeader = document.querySelector('.events-actions-header');

    const buttonContainer = document.createElement('div');
    buttonContainer.classList.add('action-buttons-container');

    const buttons = [
        { id: 'addEventBtn', text: 'Add' },
        { id: 'searchEventBtn', text: 'Search' },
        { id: 'updateEventBtn', text: 'Update' },
        { id: 'deleteEventBtn', text: 'Delete' }
    ];

    buttons.forEach(button => {
        const btn = document.createElement('button');
        btn.id = button.id;
        btn.innerText = button.text;
        btn.classList.add('action-button');

        btn.addEventListener('click', () => {
            console.log(`${button.text} button clicked`);
        });

        buttonContainer.appendChild(btn);
    });

    actionsHeader.appendChild(buttonContainer);
}
