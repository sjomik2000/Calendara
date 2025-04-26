function setupAddEventForm(dateString = null) {
    const eventsContainer = document.getElementById('events-list');
    const eventsHeader = document.querySelector('.events-header');
    eventsHeader.textContent = 'Add Event';

    fetch('/html/Forms/addEventForm.html')
        .then(response => response.text())
        .then(formHTML => {
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
            document.getElementById('cancelEventBtn').addEventListener('click', resetEventView);
        })
        .catch(error => {
            console.error('Error loading add event form:', error);
            eventsContainer.innerHTML = 'Error loading form. Please try again.';
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