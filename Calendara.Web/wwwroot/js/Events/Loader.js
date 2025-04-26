let events = [];
let sortedEventsByDate = {};

async function loadEvents() {
    try {
        const response = await fetch('https://localhost:5002/api/events');
        const data = await response.json();
        console.log(`Total events loaded from API: ${data.events.length}`);

        events = [];

        events = data.events.flatMap(event => {
            if (event.allDay || !(event.startDateTime && event.endDateTime)) {
                return [event];
            }

            const start = new Date(event.startDateTime);
            const end = new Date(event.endDateTime);

            const startDate = new Date(Date.UTC(start.getUTCFullYear(), start.getUTCMonth(), start.getUTCDate()));
            const endDate = new Date(Date.UTC(end.getUTCFullYear(), end.getUTCMonth(), end.getUTCDate()));

            // Single-day event
            if (startDate.getTime() === endDate.getTime()) {
                return [event];
            }

            const result = [];
            const currentDate = new Date(startDate);

            while (currentDate <= endDate) {
                // Create a clean copy of the event to avoid reference issues
                const eventCopy = JSON.parse(JSON.stringify(event));

                const year = currentDate.getUTCFullYear();
                const month = String(currentDate.getUTCMonth() + 1).padStart(2, '0');
                const day = String(currentDate.getUTCDate()).padStart(2, '0');
                eventCopy.dateOnly = `${year}-${month}-${day}`;

                eventCopy.id = event.id;

                result.push(eventCopy);

                currentDate.setUTCDate(currentDate.getUTCDate() + 1);
            }

            return result;
        });

        console.log(`Events after processing: ${events.length}`);

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