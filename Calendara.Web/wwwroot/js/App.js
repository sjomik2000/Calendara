let nav = 0; // Navigation index from current month
const calendar = document.getElementById('calendar');
const weekdays = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
const monthNames = ['January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'];

document.addEventListener('DOMContentLoaded', () => {
    console.log('Calendara web application initialized');
    loadAllScripts().then(() => {
        loadEvents();
        initLoad();
        initActionButtons();
        console.log('Application fully initialized');
    });
});

function loadScript(src) {
    return new Promise((resolve, reject) => {
        const script = document.createElement('script');
        script.src = src;
        script.onload = () => resolve();
        script.onerror = () => reject(new Error(`Failed to load script: ${src}`));
        document.head.appendChild(script);
    });
}

async function loadAllScripts() {
    try {
        await loadScript('js/Utilities/DateFormatting.js');
        await loadScript('js/Utilities/Notifications.js');
        await loadScript('js/Calendar/Rendering.js');
        await loadScript('js/Calendar/Navigation.js');
        await loadScript('js/Events/Loader.js');
        await loadScript('js/Events/Display.js');
        await loadScript('js/Actions/Add.js');
        await loadScript('js/Actions/Search.js');
        await loadScript('js/Actions/Update.js');
        await loadScript('js/Actions/Delete.js');
    } catch (error) {
        console.error('Error loading scripts:', error);
    }
}

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
                // Selected date remembering date
                const selectedDay = document.querySelector('.day.selected');
                const dateString = selectedDay ? selectedDay.dataset.date : null;
                setupAddEventForm(dateString);
            } else if (button.id === 'searchEventBtn') {
                setupSearchEventForm();
            } else if (button.id === 'deleteEventBtn') {
                setupDeleteEventForm();
            } else if (button.id === 'updateEventBtn') {
                setupUpdateEventForm();
            }
        });

        buttonContainer.appendChild(btn);
    });

    actionsHeader.appendChild(buttonContainer);
}