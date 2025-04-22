let nav = 0; // Navigation index from current month
let selected = null;
let events = [];
let sortedEventsByDate = {};
const calendar = document.getElementById('calendar');
const weekdays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];

document.addEventListener('DOMContentLoaded', () => {
    console.log('Calendara web application initialized');

    // Calendar functionality:
    loadEvents();
    initLoad();
});

async function loadEvents() {
    try {
        const response = await fetch('https://localhost:5002/api/events');
        const data = await response.json();
        console.log('Events loaded:', data);
        // Events processing:
        events = data.events;
    } catch (error) {
        console.error('Error loading events:', error);
    }
}

function initLoad() {
    const date = new Date();

    const currentMonth = date.getMonth() + nav;
    const year = date.getFullYear() + Math.floor(currentMonth / 12);
    const month = ((currentMonth % 12) + 12) % 12;

    // Header display for current year & month
    const monthDisplay = document.getElementById('header');
    const monthNames = ['January', 'February', 'March', 'April', 'May', 'June',
        'July', 'August', 'September', 'October', 'November', 'December'];
    monthDisplay.innerHTML = `
        <div class="month-nav">
            <button id="prevButton" class="nav-button">Prev</button>
            <div id="monthDisplay">${monthNames[month]}</div>
            <button id="nextButton" class="nav-button">Next</button>
        </div>
        <div class="year-nav">
            <button id="prevYearButton" class="nav-button year-button">Prev</button>
            <div id="yearDisplay">${year}</div>
            <button id="nextYearButton" class="nav-button year-button">Next</button>
        </div>
    `;

    // Button event listeners
    document.getElementById('prevButton').addEventListener('click', () => {
        nav--;
        calendar.innerHTML = '';
        initLoad();
    });

    document.getElementById('nextButton').addEventListener('click', () => {
        nav++;
        calendar.innerHTML = '';
        initLoad();
    });

    document.getElementById('prevYearButton').addEventListener('click', () => {
        nav = nav - 12;
        calendar.innerHTML = '';
        initLoad();
    });

    document.getElementById('nextYearButton').addEventListener('click', () => {
        nav = nav + 12;
        calendar.innerHTML = '';
        initLoad();
    });

    calendar.innerHTML = '';

    const firstDayOfMonth = new Date(year, month, 1);
    const daysInMonth = new Date(year, month + 1, 0).getDate();

    // Conversion from Sunday week start to Monday week start
    const firstDayWeekday = (firstDayOfMonth.getDay() + 6) % 7;

    const daysInPrevMonth = new Date(year, month, 0).getDate();

    for (let i = 1; i <= 42; i++) {
        const daySquare = document.createElement('div');
        daySquare.classList.add('day');

        if (i <= firstDayWeekday) {
            const prevMonthDay = daysInPrevMonth - firstDayWeekday + i;
            daySquare.innerText = prevMonthDay;
            daySquare.classList.add('padding');

            // Data for filtering events
            const prevMonth = month === 0 ? 11 : month - 1;
            const prevYear = month === 0 ? year - 1 : year;
            daySquare.dataset.date = `${prevYear}-${String(prevMonth + 1).padStart(2, '0')}-${String(prevMonthDay).padStart(2, '0')}`;
        }
        else if (i > firstDayWeekday && i <= firstDayWeekday + daysInMonth) {
            // Current month days
            const currentDay = i - firstDayWeekday;
            daySquare.innerText = currentDay;
            // Check for current day
            const today = new Date();
            if (nav === 0 && currentDay === today.getDate() && month === today.getMonth() && year === today.getFullYear()) {
                daySquare.classList.add('currentDay');
            }

            // Data for filtering events
            daySquare.dataset.date = `${year}-${String(month + 1).padStart(2, '0')}-${String(currentDay).padStart(2, '0')}`;

            const dateString = daySquare.dataset.date;
            const eventsForDay = events.filter(event => {
                const eventDate = event.dateOnly ?
                    event.dateOnly.split('T')[0] :
                    (event.startDateTime ? new Date(event.startDateTime).toISOString().split('T')[0] : null);
                return eventDate === dateString;
            });

            if (eventsForDay.length > 0) {
                eventsForDay.forEach(event => {
                    const eventDiv = document.createElement('div');
                    eventDiv.classList.add('event');
                    eventDiv.innerText = event.title;
                    daySquare.appendChild(eventDiv);
                });
            }

            // Click handler to add new events (WIP)
            daySquare.addEventListener('click', () => {
                selected = dateString;
                console.log('Selected:', selected);
            });
        }
        else {
            const nextMonthDay = i - (firstDayWeekday + daysInMonth);
            daySquare.innerText = nextMonthDay;
            daySquare.classList.add('padding');
            // Data for filtering events
            const nextMonth = month === 11 ? 0 : month + 1;
            const nextYear = month === 11 ? year + 1 : year;
            daySquare.dataset.date = `${nextYear}-${String(nextMonth + 1).padStart(2, '0')}-${String(nextMonthDay).padStart(2, '0')}`;
        }

        calendar.appendChild(daySquare);
    }
}