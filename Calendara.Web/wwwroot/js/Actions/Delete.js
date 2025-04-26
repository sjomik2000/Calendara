function setupDeleteEventForm() {
    const eventsContainer = document.getElementById('events-list');
    const eventsHeader = document.querySelector('.events-header');
    eventsHeader.textContent = 'Delete Event';

    fetch('/html/Forms/deleteEventForm.html')
        .then(response => response.text())
        .then(html => {
            eventsContainer.innerHTML = html;

            document.getElementById('submitDeleteBtn').addEventListener('click', deleteEvent);
            document.getElementById('cancelDeleteBtn').addEventListener('click', resetEventView);
        })
        .catch(error => {
            console.error('Error loading delete form:', error);
            eventsContainer.innerHTML = 'Error loading form. Please try again.';
        });
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