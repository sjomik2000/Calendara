function formatDateString(year, month, day) {
    return `${year}-${String(month).padStart(2, '0')}-${String(day).padStart(2, '0')}`;
}

function getEventDate(event) {
    if (event.dateOnly) {
        return event.dateOnly;
    }

    if (event.startDateTime) {
        const date = new Date(event.startDateTime);
        const year = date.getUTCFullYear();
        const month = String(date.getUTCMonth() + 1).padStart(2, '0');
        const day = String(date.getUTCDate()).padStart(2, '0');
        return `${year}-${month}-${day}`;
    }

    return null;
}

function formatEventTime(event) {
    if (event.allDay) {
        return 'All day';
    }

    if (event.startDateTime && event.endDateTime) {
        const start = new Date(event.startDateTime);
        const end = new Date(event.endDateTime);

        // multiple day event check
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