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
        console.log(`Total events loaded from API: ${data.events.length}`);

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

        if (document.querySelector('#calendar').children.length > 0) {
            refreshCalendar();
        }
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

    const navButtons = document.querySelectorAll('.nav-button');
    navButtons.forEach(button => {
        button.addEventListener('mouseenter', () => {
            button.style.backgroundColor = '#7b89b3'; 
        });
        button.addEventListener('mouseleave', () => {
            button.style.backgroundColor = '#92a1d1';
        });
    });
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

    const existingContainer = actionsHeader.querySelector('.action-buttons-container');
    if (existingContainer) {
        existingContainer.remove();
    }

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
            if (button.id === 'addEventBtn') {
                //Selected date remembering date
                const selectedDay = document.querySelector('.day.selected');
                const dateString = selectedDay ? selectedDay.dataset.date : null;
                setupAddEventForm(dateString);
            } else if (button.id === 'searchEventBtn') {
                setupSearchEventForm();
            } else if (button.id === 'deleteEventBtn') {
                setupDeleteEventForm();
            } else if (button.id === 'updateEventBtn') {
                console.log(`${button.text} button clicked`);
            }
        });

        buttonContainer.appendChild(btn);
    });

    actionsHeader.appendChild(buttonContainer);
}

function setupAddEventForm(dateString = null) {

    const eventsContainer = document.getElementById('events-list');
    const eventsHeader = document.querySelector('.events-header');
    eventsHeader.textContent = 'Add Event';

    const formHTML = `
        <div class="event-form">
            <div class="form-group">
                <label for="eventTitle">Title*:</label>
                <input type="text" id="eventTitle" class="form-input" required>
            </div>
            
            <div class="form-group checkbox-group">
                <label for="eventAllDay">All day:</label>
                <label class="toggle-switch">
                    <input type="checkbox" id="eventAllDay">
                    <span class="slider round"></span>
                </label>
            </div>
            
            <div id="dateOnlyGroup" class="form-group date-group" style="display:none;">
                <label>Date*:</label>
                <div class="date-inputs">
                    <input type="text" id="dateDay" class="form-input date-part" placeholder="DD" maxlength="2">
                    <input type="text" id="dateMonth" class="form-input date-part" placeholder="MM" maxlength="2">
                    <input type="text" id="dateYear" class="form-input date-part year-part" placeholder="YYYY" maxlength="4">
                </div>
            </div>
            
            <div id="startDateTimeGroup" class="form-group date-group">
                <label>Start*:</label>
                <div class="date-inputs">
                    <input type="text" id="startDay" class="form-input date-part" placeholder="DD" maxlength="2">
                    <input type="text" id="startMonth" class="form-input date-part" placeholder="MM" maxlength="2">
                    <input type="text" id="startYear" class="form-input date-part year-part" placeholder="YYYY" maxlength="4">
                    <input type="text" id="startHour" class="form-input date-part" placeholder="hh" maxlength="2">
                    <input type="text" id="startMinute" class="form-input date-part" placeholder="mm" maxlength="2">
                </div>
            </div>
            
            <div id="endDateTimeGroup" class="form-group date-group">
                <label>End*:</label>
                <div class="date-inputs">
                    <input type="text" id="endDay" class="form-input date-part" placeholder="DD" maxlength="2">
                    <input type="text" id="endMonth" class="form-input date-part" placeholder="MM" maxlength="2">
                    <input type="text" id="endYear" class="form-input date-part year-part" placeholder="YYYY" maxlength="4">
                    <input type="text" id="endHour" class="form-input date-part" placeholder="hh" maxlength="2">
                    <input type="text" id="endMinute" class="form-input date-part" placeholder="mm" maxlength="2">
                </div>
            </div>
            
            <div class="form-group">
                <label for="eventDescription">Description:</label>
                <textarea id="eventDescription" class="form-input textarea"></textarea>
            </div>
            
            <div class="form-group">
                <label>Location:</label>
                <div class="location-inputs">
                    <input type="text" id="eventLatitude" class="form-input" placeholder="X (latitude)">
                    <input type="text" id="eventLongitude" class="form-input" placeholder="Y (longitude)">
                </div>
            </div>
            
            <div class="form-actions">
                <button id="submitEventBtn" class="submit-button">Submit</button>
                <button id="cancelEventBtn" class="cancel-button">Cancel</button>
            </div>
        </div>
    `;

    eventsContainer.innerHTML = formHTML;

    if (dateString) {
        const date = new Date(dateString);
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();

        document.getElementById('dateDay').value = day;
        document.getElementById('dateMonth').value = month;
        document.getElementById('dateYear').value = year;

        document.getElementById('startDay').value = day;
        document.getElementById('startMonth').value = month;
        document.getElementById('startYear').value = year;

        document.getElementById('endDay').value = day;
        document.getElementById('endMonth').value = month;
        document.getElementById('endYear').value = year;
    }

    const allDayCheckbox = document.getElementById('eventAllDay');
    allDayCheckbox.addEventListener('change', function () {
        const dateOnlyGroup = document.getElementById('dateOnlyGroup');
        const startDateTimeGroup = document.getElementById('startDateTimeGroup');
        const endDateTimeGroup = document.getElementById('endDateTimeGroup');

        if (this.checked) {
            dateOnlyGroup.style.display = 'block';
            startDateTimeGroup.style.display = 'none';
            endDateTimeGroup.style.display = 'none';
        } else {
            dateOnlyGroup.style.display = 'none';
            startDateTimeGroup.style.display = 'block';
            endDateTimeGroup.style.display = 'block';
        }
    });

    const numberInputs = document.querySelectorAll('.date-part');
    numberInputs.forEach(input => {
        input.addEventListener('input', function () {
            this.value = this.value.replace(/[^0-9]/g, '');
        });
    });

    document.getElementById('submitEventBtn').addEventListener('click', submitEventForm);

    document.getElementById('cancelEventBtn').addEventListener('click', function () {
        // Reset
        const eventsHeader = document.querySelector('.events-header');
        eventsHeader.textContent = 'Selected Events';
        eventsContainer.innerHTML = '<div class="no-events">No events selected. Click on a day to view events.</div>';
    });
}

function submitEventForm() {

    const title = document.getElementById('eventTitle').value.trim();
    const allDay = document.getElementById('eventAllDay').checked;

    if (!title) {
        showNotification('Title is required', 'error');
        return;
    }

    let eventData = {
        title: title,
        allDay: allDay,
        description: document.getElementById('eventDescription').value.trim() || null
    };

    const latitude = document.getElementById('eventLatitude').value.trim();
    const longitude = document.getElementById('eventLongitude').value.trim();

    if (latitude && longitude) {
        eventData.location = {
            latitude: parseFloat(latitude),
            longitude: parseFloat(longitude)
        };
    }

    if (allDay) {
        const day = document.getElementById('dateDay').value.trim();
        const month = document.getElementById('dateMonth').value.trim();
        const year = document.getElementById('dateYear').value.trim();

        if (!day || !month || !year) {
            showNotification('Date is required for all-day events', 'error');
            return;
        }

        eventData.dateOnly = `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
    } else {
        const startDay = document.getElementById('startDay').value.trim();
        const startMonth = document.getElementById('startMonth').value.trim();
        const startYear = document.getElementById('startYear').value.trim();
        const startHour = document.getElementById('startHour').value.trim();
        const startMinute = document.getElementById('startMinute').value.trim();

        const endDay = document.getElementById('endDay').value.trim();
        const endMonth = document.getElementById('endMonth').value.trim();
        const endYear = document.getElementById('endYear').value.trim();
        const endHour = document.getElementById('endHour').value.trim();
        const endMinute = document.getElementById('endMinute').value.trim();

        if (!startDay || !startMonth || !startYear || !startHour || !startMinute) {
            showNotification('Start date and time are required', 'error');
            return;
        }

        if (!endDay || !endMonth || !endYear || !endHour || !endMinute) {
            showNotification('End date and time are required', 'error');
            return;
        }

        //ISO string dates for API
        const startDateTime = new Date(`${startYear}-${startMonth.padStart(2, '0')}-${startDay.padStart(2, '0')}T${startHour.padStart(2, '0')}:${startMinute.padStart(2, '0')}:00Z`);
        const endDateTime = new Date(`${endYear}-${endMonth.padStart(2, '0')}-${endDay.padStart(2, '0')}T${endHour.padStart(2, '0')}:${endMinute.padStart(2, '0')}:00Z`);
        
        if (endDateTime <= startDateTime) {
            showNotification('End time must be after start time', 'error');
            return;
        }

        eventData.startDateTime = startDateTime.toISOString();
        eventData.endDateTime = endDateTime.toISOString();
    }

    createEvent(eventData);
}

async function createEvent(eventData) {
    try {
        const response = await fetch('https://localhost:5002/api/events', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(eventData)
        });

        if (response.ok) {
            showNotification('Event has been added successfully', 'success');
            await loadEvents();
            refreshCalendar();
            resetEventView();
        } else {
            const errorData = await response.json();
            if (errorData && errorData.errors) {
                const errorMessages = Object.values(errorData.errors).flat().join(', ');
                showNotification(errorMessages, 'error');
            } else {
                showNotification('Failed to create event', 'error');
            }
        }
    } catch (error) {
        console.error('Error creating event:', error);
        showNotification('Error connecting to server', 'error');
    }
}

function showNotification(message, type) {
    const existingNotification = document.querySelector('.notification');
    if (existingNotification) {
        existingNotification.remove();
    }

    const notification = document.createElement('div');
    notification.className = `notification ${type}`;
    notification.textContent = message;

    const actionsHeader = document.querySelector('.events-actions-header');
    actionsHeader.prepend(notification);

    setTimeout(() => {
        notification.classList.add('fade-out');
        setTimeout(() => {
            notification.remove();
        }, 500);
    }, 5000);
}

function resetEventView() {
    const eventsHeader = document.querySelector('.events-header');
    eventsHeader.textContent = 'Selected Events';

    const eventsContainer = document.getElementById('events-list');
    eventsContainer.innerHTML = '<div class="no-events">No events selected. Click on a day to view events.</div>';
}

function setupSearchEventForm() {
    const eventsContainer = document.getElementById('events-list');
    const eventsHeader = document.querySelector('.events-header');
    eventsHeader.textContent = 'Search Event';

    const searchOptionsHTML = `
        <div class="search-options">
            <button id="searchByIdBtn" class="search-option-button">By ID</button>
            <button id="searchByDateBtn" class="search-option-button">By Date</button>
            <button id="searchByRangeBtn" class="search-option-button">By Date Range</button>
            <button id="searchAllBtn" class="search-option-button">Show all events</button>
        </div>
    `;

    eventsContainer.innerHTML = searchOptionsHTML;

    document.getElementById('searchByIdBtn').addEventListener('click', setupSearchById);
    document.getElementById('searchByDateBtn').addEventListener('click', setupSearchByDate);
    document.getElementById('searchByRangeBtn').addEventListener('click', setupSearchByDateRange);
    document.getElementById('searchAllBtn').addEventListener('click', searchAllEvents);
}

function setupSearchById() {
    const eventsContainer = document.getElementById('events-list');

    const searchByIdHTML = `
        <div class="event-form">
            <div class="form-group">
                <label for="eventId">ID*:</label>
                <input type="text" id="eventId" class="form-input" placeholder="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx">
            </div>
            
            <div class="form-actions">
                <button id="submitSearchIdBtn" class="submit-button">Submit</button>
                <button id="cancelSearchBtn" class="cancel-button">Cancel</button>
            </div>
        </div>
    `;

    eventsContainer.innerHTML = searchByIdHTML;

    document.getElementById('submitSearchIdBtn').addEventListener('click', searchById);
    document.getElementById('cancelSearchBtn').addEventListener('click', resetEventView);
}

function setupSearchByDate() {
    const eventsContainer = document.getElementById('events-list');

    const searchByDateHTML = `
        <div class="event-form">
            <div class="form-group">
                <label>Date*:</label>
                <div class="date-inputs">
                    <input type="text" id="searchDay" class="form-input date-part" placeholder="DD" maxlength="2">
                    <input type="text" id="searchMonth" class="form-input date-part" placeholder="MM" maxlength="2">
                    <input type="text" id="searchYear" class="form-input date-part year-part" placeholder="YYYY" maxlength="4">
                </div>
            </div>
            
            <div class="form-actions">
                <button id="submitSearchDateBtn" class="submit-button">Submit</button>
                <button id="cancelSearchBtn" class="cancel-button">Cancel</button>
            </div>
        </div>
    `;

    eventsContainer.innerHTML = searchByDateHTML;

    document.getElementById('submitSearchDateBtn').addEventListener('click', searchByDate);
    document.getElementById('cancelSearchBtn').addEventListener('click', resetEventView);

    const numberInputs = document.querySelectorAll('.date-part');
    numberInputs.forEach(input => {
        input.addEventListener('input', function () {
            this.value = this.value.replace(/[^0-9]/g, '');
        });
    });
}

function setupSearchByDateRange() {
    const eventsContainer = document.getElementById('events-list');

    const searchByRangeHTML = `
        <div class="event-form">
            <div class="form-group">
                <label>Date Start*:</label>
                <div class="date-inputs">
                    <input type="text" id="startSearchDay" class="form-input date-part" placeholder="DD" maxlength="2">
                    <input type="text" id="startSearchMonth" class="form-input date-part" placeholder="MM" maxlength="2">
                    <input type="text" id="startSearchYear" class="form-input date-part year-part" placeholder="YYYY" maxlength="4">
                </div>
            </div>
            
            <div class="form-group">
                <label>Date End*:</label>
                <div class="date-inputs">
                    <input type="text" id="endSearchDay" class="form-input date-part" placeholder="DD" maxlength="2">
                    <input type="text" id="endSearchMonth" class="form-input date-part" placeholder="MM" maxlength="2">
                    <input type="text" id="endSearchYear" class="form-input date-part year-part" placeholder="YYYY" maxlength="4">
                </div>
            </div>
            
            <div class="form-actions">
                <button id="submitSearchRangeBtn" class="submit-button">Submit</button>
                <button id="cancelSearchBtn" class="cancel-button">Cancel</button>
            </div>
        </div>
    `;

    eventsContainer.innerHTML = searchByRangeHTML;

    document.getElementById('submitSearchRangeBtn').addEventListener('click', searchByDateRange);
    document.getElementById('cancelSearchBtn').addEventListener('click', resetEventView);

    const numberInputs = document.querySelectorAll('.date-part');
    numberInputs.forEach(input => {
        input.addEventListener('input', function () {
            this.value = this.value.replace(/[^0-9]/g, '');
        });
    });
}

async function searchById() {
    const eventId = document.getElementById('eventId').value.trim();

    if (!eventId) {
        showNotification('Please enter a valid ID', 'error');
        return;
    }

    try {
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
        const response = await fetch('https://localhost:5002/api/events');

        if (response.ok) {
            const data = await response.json();
            const allEvents = data.events || [];

            console.log('All events:', allEvents);

            const filteredEvents = allEvents.filter(event => {
                //All-day event with dateOnly matching
                if (event.allDay && event.dateOnly === dateString) {
                    return true;
                }

                //Event with start/end time on selected date or overlapping selected date
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

function setupDeleteEventForm() {
    const eventsContainer = document.getElementById('events-list');
    const eventsHeader = document.querySelector('.events-header');
    eventsHeader.textContent = 'Delete Event';

    const deleteFormHTML = `
        <div class="event-form">
            <div class="form-group">
                <label for="deleteEventId">ID*:</label>
                <input type="text" id="deleteEventId" class="form-input" placeholder="xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx">
            </div>
            
            <div class="form-actions">
                <button id="submitDeleteBtn" class="submit-button">Submit</button>
                <button id="cancelDeleteBtn" class="cancel-button">Cancel</button>
            </div>
        </div>
    `;

    eventsContainer.innerHTML = deleteFormHTML;

    document.getElementById('submitDeleteBtn').addEventListener('click', deleteEvent);
    document.getElementById('cancelDeleteBtn').addEventListener('click', resetEventView);
}

async function deleteEvent() {
    const eventId = document.getElementById('deleteEventId').value.trim();

    if (!eventId) {
        showNotification('Please enter a valid ID', 'error');
        return;
    }

    try {
        const response = await fetch(`https://localhost:5002/api/events/${eventId}`, {
            method: 'DELETE'
        });

        if (response.ok) {
            showNotification('Event has been deleted successfully', 'success');
            await loadEvents();
            refreshCalendar();
            resetEventView();
        } else {
            showNotification('Event not found, please enter a valid ID', 'error');
        }
    } catch (error) {
        console.error('Error deleting event:', error);
        showNotification('Error connecting to server', 'error');
    }
}