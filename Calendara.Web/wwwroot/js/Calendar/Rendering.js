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

    //filtering events info
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

    // current day
    const today = new Date();
    if (nav === 0 && currentDay === today.getDate() &&
        month === today.getMonth() && year === today.getFullYear()) {
        daySquare.classList.add('currentDay');
    }

    //filtering events info
    const dateString = formatDateString(year, month + 1, currentDay);
    daySquare.dataset.date = dateString;

    addEventIndicators(daySquare, dateString);

    daySquare.addEventListener('click', () => handleDayClick(daySquare, dateString));
}

function renderNextMonthDay(daySquare, index, firstDayWeekday, daysInMonth, month, year) {
    const nextMonthDay = index - (firstDayWeekday + daysInMonth);
    daySquare.innerText = nextMonthDay;
    daySquare.classList.add('padding');

    //filtering events info
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

function refreshCalendar() {
    calendar.innerHTML = '';
    initLoad();
}