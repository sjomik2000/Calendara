function setupUpdateEventForm() {
    const eventsContainer = document.getElementById('events-list');
    const eventsHeader = document.querySelector('.events-header');
    eventsHeader.textContent = 'Update Event';

    fetch('/html/Forms/updateIdForm.html')
        .then(response => response.text())
        .then(html => {
            eventsContainer.innerHTML = html;

            document.getElementById('submitSearchEventBtn').addEventListener('click', findEventForUpdate);
            document.getElementById('cancelUpdateBtn').addEventListener('click', resetEventView);
        })
        .catch(error => {
            console.error('Error loading update ID form:', error);
            eventsContainer.innerHTML = 'Error loading form. Please try again.';
        });
}

async function findEventForUpdate() {
    const eventId = document.getElementById('updateEventId').value.trim();

    if (!eventId) {
        showNotification('Please enter a valid ID', 'error');
        return;
    }

    try {
        const response = await fetch(`https://localhost:5002/api/events/${eventId}`);

        if (response.ok) {
            const eventData = await response.json();
            showNotification('Event has been retrieved successfully', 'success');
            populateUpdateForm(eventData);
        } else {
            showNotification('Event not found, please enter a valid ID', 'error');
        }
    } catch (error) {
        console.error('Error retrieving event:', error);
        showNotification('Error connecting to server', 'error');
    }
}

function populateUpdateForm(event) {
    const eventsContainer = document.getElementById('events-list');

    let dateInfo = extractDateInfo(event);

    fetch('html/Forms/updateEventForm.html')
        .then(response => response.text())
        .then(formTemplate => {
            const formHTML = formTemplate
                .replace('{{eventId}}', event.id || '')
                .replace('{{eventTitle}}', event.title || '')
                .replace('{{eventAllDay}}', event.allDay ? 'checked' : '')
                .replace('{{dateOnlyDisplay}}', event.allDay ? 'block' : 'none')
                .replace('{{dateTimeDisplay}}', !event.allDay ? 'block' : 'none')
                .replace('{{dateDay}}', dateInfo.dateDay)
                .replace('{{dateMonth}}', dateInfo.dateMonth)
                .replace('{{dateYear}}', dateInfo.dateYear)
                .replace('{{startDay}}', dateInfo.startDay)
                .replace('{{startMonth}}', dateInfo.startMonth)
                .replace('{{startYear}}', dateInfo.startYear)
                .replace('{{startHour}}', dateInfo.startHour)
                .replace('{{startMinute}}', dateInfo.startMinute)
                .replace('{{endDay}}', dateInfo.endDay)
                .replace('{{endMonth}}', dateInfo.endMonth)
                .replace('{{endYear}}', dateInfo.endYear)
                .replace('{{endHour}}', dateInfo.endHour)
                .replace('{{endMinute}}', dateInfo.endMinute)
                .replace('{{eventDescription}}', event.description || '')
                .replace('{{latitude}}', event.location ? event.location.latitude : '')
                .replace('{{longitude}}', event.location ? event.location.longitude : '');

            eventsContainer.innerHTML = formHTML;
            setupEventFormListeners();
        })
        .catch(error => {
            console.error('Error loading update form template:', error);
            eventsContainer.innerHTML = 'Error loading form. Please try again.';
        });
}

function extractDateInfo(event) {
    let dateInfo = {
        dateDay: '', dateMonth: '', dateYear: '',
        startDay: '', startMonth: '', startYear: '', startHour: '', startMinute: '',
        endDay: '', endMonth: '', endYear: '', endHour: '', endMinute: ''
    };

    if (event.dateOnly) {
        const date = new Date(event.dateOnly);
        dateInfo.dateDay = String(date.getDate()).padStart(2, '0');
        dateInfo.dateMonth = String(date.getMonth() + 1).padStart(2, '0');
        dateInfo.dateYear = date.getFullYear();

        dateInfo.startDay = dateInfo.endDay = dateInfo.dateDay;
        dateInfo.startMonth = dateInfo.endMonth = dateInfo.dateMonth;
        dateInfo.startYear = dateInfo.endYear = dateInfo.dateYear;
    }

    if (event.startDateTime) {
        const start = new Date(event.startDateTime);
        dateInfo.startDay = String(start.getDate()).padStart(2, '0');
        dateInfo.startMonth = String(start.getMonth() + 1).padStart(2, '0');
        dateInfo.startYear = start.getFullYear();
        dateInfo.startHour = String(start.getHours()).padStart(2, '0');
        dateInfo.startMinute = String(start.getMinutes()).padStart(2, '0');

        dateInfo.dateDay = dateInfo.startDay;
        dateInfo.dateMonth = dateInfo.startMonth;
        dateInfo.dateYear = dateInfo.startYear;
    }

    if (event.endDateTime) {
        const end = new Date(event.endDateTime);
        dateInfo.endDay = String(end.getDate()).padStart(2, '0');
        dateInfo.endMonth = String(end.getMonth() + 1).padStart(2, '0');
        dateInfo.endYear = end.getFullYear();
        dateInfo.endHour = String(end.getHours()).padStart(2, '0');
        dateInfo.endMinute = String(end.getMinutes()).padStart(2, '0');
    }

    return dateInfo;
}

function setupEventFormListeners() {
    const allDayCheckbox = document.getElementById('eventAllDay');
    const dateOnlyGroup = document.getElementById('dateOnlyGroup');
    const startDateTimeGroup = document.getElementById('startDateTimeGroup');
    const endDateTimeGroup = document.getElementById('endDateTimeGroup');

    if (allDayCheckbox.checked) {
        dateOnlyGroup.style.display = 'block';
        startDateTimeGroup.style.display = 'none';
        endDateTimeGroup.style.display = 'none';
    } else {
        dateOnlyGroup.style.display = 'none';
        startDateTimeGroup.style.display = 'block';
        endDateTimeGroup.style.display = 'block';
    }

    allDayCheckbox.addEventListener('change', function () {
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

    document.getElementById('submitUpdateBtn').addEventListener('click', submitUpdateForm);
    document.getElementById('cancelUpdateBtn').addEventListener('click', resetEventView);
}

function submitUpdateForm() {
    const eventId = document.getElementById('eventId').value.trim();
    const title = document.getElementById('eventTitle').value.trim();
    const allDay = document.getElementById('eventAllDay').checked;
    const description = document.getElementById('eventDescription').value.trim();
    const latitude = document.getElementById('eventLatitude').value.trim();
    const longitude = document.getElementById('eventLongitude').value.trim();

    if (!title) {
        showNotification('Title is required', 'error');
        return;
    }

    let eventData = {
        id: eventId,
        title: title,
        allDay: allDay,
        description: description || ""
    };

    eventData.location = (latitude && longitude)
        ? { latitude: parseFloat(latitude), longitude: parseFloat(longitude) }
        : null;

    if (allDay) {
        const day = document.getElementById('dateDay').value.trim();
        const month = document.getElementById('dateMonth').value.trim();
        const year = document.getElementById('dateYear').value.trim();

        if (!day || !month || !year) {
            showNotification('Date is required for all-day events', 'error');
            return;
        }

        eventData.dateOnly = `${year}-${month.padStart(2, '0')}-${day.padStart(2, '0')}`;
        eventData.startDateTime = null;
        eventData.endDateTime = null;
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

        const startDateTime = new Date(`${startYear}-${startMonth.padStart(2, '0')}-${startDay.padStart(2, '0')}T${startHour.padStart(2, '0')}:${startMinute.padStart(2, '0')}:00Z`);
        const endDateTime = new Date(`${endYear}-${endMonth.padStart(2, '0')}-${endDay.padStart(2, '0')}T${endHour.padStart(2, '0')}:${endMinute.padStart(2, '0')}:00Z`);

        if (endDateTime <= startDateTime) {
            showNotification('End time must be after start time', 'error');
            return;
        }

        eventData.startDateTime = startDateTime.toISOString();
        eventData.endDateTime = endDateTime.toISOString();
        eventData.dateOnly = null;
    }

    console.log("Event update payload:", JSON.stringify(eventData));

    updateEvent(eventData);
}

async function updateEvent(eventData) {
    try {
        console.log("Sending update with data:", JSON.stringify(eventData));

        const response = await fetch(`https://localhost:5002/api/events/${eventData.id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(eventData)
        });

        if (response.ok) {
            const updatedEvent = await response.json();
            console.log("Server returned updated event:", updatedEvent);

            await loadEvents();

            showNotification('Event has been updated successfully', 'success');

            await loadEvents();
            const eventIndex = events.findIndex(e => e.id === updatedEvent.id);
            if (eventIndex !== -1) {
                events[eventIndex] = updatedEvent;
            }
            refreshCalendar();
            resetEventView();
        } else {
            const responseText = await response.text();
            console.log("Server error response text:", responseText);

            let errorMessage = 'Failed to update event';

            try {
                if (responseText) {
                    const errorData = JSON.parse(responseText);
                    console.log("Error response parsed:", errorData);

                    if (errorData.errors && Array.isArray(errorData.errors)) {
                        const errorMessages = errorData.errors.map(err => err.message);
                        errorMessage = errorMessages.join(', ');
                    } else if (errorData.errors) {
                        errorMessage = Object.values(errorData.errors)
                            .flat()
                            .join(', ');
                    } else if (errorData.title) {
                        errorMessage = errorData.title;
                        if (errorData.detail) {
                            errorMessage += `: ${errorData.detail}`;
                        }
                    } else if (errorData.message) {
                        errorMessage = errorData.message;
                    } else if (typeof errorData === 'string') {
                        errorMessage = errorData;
                    }
                }
            } catch (parseError) {
                console.error('Error parsing error response:', parseError);
                if (responseText) {
                    errorMessage = responseText;
                }
            }

            showNotification(errorMessage, 'error');
        }
    } catch (error) {
        console.error('Error updating event:', error);
        showNotification('Error connecting to server', 'error');
    }
}