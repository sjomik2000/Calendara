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