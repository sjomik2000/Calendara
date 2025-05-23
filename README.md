# Calendara

## 1. Brief description

Calendara is a comprehensive event management application built with a modern technology stack: .NET 8 backend and HTML/CSS/JavaScript frontend. The system features a responsive calendar interface that enables users to efficiently track and manage their events with intuitive navigation and detailed event views.

The architecture of the project is broken down into several layers:
1) Backend: Processes API requests and interacts with the database
    + API layer: ASP.NET Core Web API implementing RESTful principles with comprehensive validation and CORS support, and mapping middleware.
    + Application layer: Business logic for event management, validation, repository patterns and DI injections.
    + Contracts layer: Contracts for HTTP requests/responses with custom GeoCoordinate support
2) Frontend: Displays Calendar on the web browser and fetches API calls
    + Web layer: Modular JavaScript with dynamic HTML templates and notifications for an interactive user experience
3) Database: PostgreSQL with Entity Framework Core for data persistence
4) Testing: Extensive unit and integration tests using xUnit with 95%+ coverage for all C# written backend code

## 2. Features

### A) Event Management
1) Add Events
   + Add events with title, description, and time
   + Support for all-day events
   + Geolocation integration with Google Maps
   + Input validation with success/failure notifications
![image](https://github.com/user-attachments/assets/0ea13d35-1ec9-4975-aca8-93ab57c91253)

2) Search Events
   + Multiple search options:
     - By ID
     - By specific date
     - By date range
     - Show all events
   + Multi-day event support
   + Sorted event display
   + Retrieved events count notification
![image](https://github.com/user-attachments/assets/70c83a50-f585-423b-820f-4cb2e103cddd)
![image](https://github.com/user-attachments/assets/3ccfb18b-3609-4726-bd57-e03db8acf5e5)

3) Update Events
   + Pre-filled form with existing event data
   + Real-time validation
   + Location and description optional fields
   + Support for modifying all-day event status
   + Input validation with success/failure notifications
![image](https://github.com/user-attachments/assets/8925b5c5-ac43-49db-8954-0995eeed4177)

4) Delete Events
   + Confirmation system
   + Success/failure notifications
![image](https://github.com/user-attachments/assets/2d6e2231-a0d6-412f-9000-03a8fc2d04a5)


### B) Calendar Interface
1) Interactive Navigation
   + Previous/Next month navigation
   + Previous/Next year navigation
   + Previous/Next month's greyed out day paddings
   + Current day highlighting (deep blue day square background)
   + Selected day highlighting with tracking (light blue outer lines)
![image](https://github.com/user-attachments/assets/e49087e2-5c31-4df6-a4ff-ad9307355019)

   
2) Event Display
   + List view of events with sorting algorithm
   + Multi-day event visualisation
   + Event details popup
   + Google Maps location integration
   + UK culture Weekday start and date formatting
![image](https://github.com/user-attachments/assets/7cb9b116-b557-469d-b349-53d7341411e5)

### C) API Architecture
1) RESTful Endpoints
   + Resource-based routing (/api/events/{id})
   + Standard HTTP methods (GET, POST, PUT, DELETE)
   + JSON request/response format
   + CORS support for web integration
![image](https://github.com/user-attachments/assets/f5ee00fe-bd8a-4c20-a66b-ad3656d882a0)

2) Data Validation
   + Request contract validation
   + Custom GeoCoordinate handling
   + Error response standardisation
   + Mapping middleware
![image](https://github.com/user-attachments/assets/48495534-0d40-4719-ba7f-fc724c94b298)

### D) Database Implementation
1) PostgreSQL Integration
   + Entity Framework Core code-first approach
   + Custom DateTime conversion support
   + Efficient data querying
![image](https://github.com/user-attachments/assets/be00e6bb-6f5d-4d0e-b664-7e06524c4db0)

### E) Testing Infrastructure
   + 95%+ code coverage for backend infrastructure
   + Custom mocks for HTTP and Validator handling
   + Events fixtures
   + Unit tests for all components
   + Used TDD approach to write controller actions
![image](https://github.com/user-attachments/assets/32b66285-0be2-4e82-84fc-f5d46d43bbb5)
![image](https://github.com/user-attachments/assets/b6218f57-d95f-4dd0-8616-30257c6a86ff)

### F) DevOps Integration
1) Azure DevOps Pipeline
   + Automated build process
   + Unit test execution
   + Combined API and Web deployment
   + Self-hosted pool configuration
![image](https://github.com/user-attachments/assets/a5c387d2-08b3-4e22-9877-b138b5241307)

## 3. Set up instructions

To run the Calendara application on your local machine, follow these steps:

### Installation requirements
1) Visual Studio 2022 or another compatible IDE
2) .NET 8 SDK installed on your machine
3) Git (for cloning the repository)
4) PostgreSQL database server (version 13 recommended)

### Database Setup
1) Install PostgreSQL (download from [postgresql.org](https://www.postgresql.org/download/))
2) Create a new database named `events`
3) Create a user with credentials as specified in the appsettings.json:
   - Username: `eventadmin`
   - Password: `admin1`
4) Grant the user all privileges on the `events` database
5) Make sure PostgreSQL is running on port `5433` (default port is 5432, so you might need to change this in your PostgreSQL configuration or update the connection string in appsettings.json)

### Application Setup
1) Clone this repository to your local machine:
git clone https://github.com/yourusername/calendara.git cd calendara
2) Make sure ports 5001 (for Web) and 5002 (for API) are free and not blocked by your firewall
   - You can modify port configurations in:
     - `Calendara.Api/Properties/launchSettings.json` for the API
     - `Calendara.Web/Program.cs` for the Web application
3) Required NuGet packages will be restored automatically when building the solution, here is the list of main packages used:
   - Microsoft.EntityFrameworkCore
   - Npgsql.EntityFrameworkCore.PostgreSQL
   - FluentValidation
   - Microsoft.Extensions.DependencyInjection
   - FluentValidation
   - xUnit
   - Moq

### Running the Application
1) Open the solution in the IDE
2) Build the solution
3) Configure run to start both API and Web layer projects simultaneously
4) The application should now be accessible at:
   - Web Frontend: https://localhost:5001
   - API Endpoints: https://localhost:5002/api/events

## 4. ChangeLog
+ 2025.04.14 - Created GitHub repository, added .gitignore and modelled project structure and features,
created README.md with project breakdown.
+ 2025.04.15 - Created API, Application, Contracts and UnitTests base layers, started working 
on Controller class and Services class, registered Service DI, created Events model and 5 UnitTests.  
+ 2025.04.16 - Upgraded Project Framework from .NET Core 3.1 to .NET 8. Finished Calendara.Contracts layer, 
finished writing all Controller actions and mappings for requests and responses. Fully tested Create, 
GetById and GetAll methods.
+ 2025.04.17 - Created GitHub CI/CD Pipeline on dev.azure.com with self hosted pool. Fully tested Update 
method.
+ 2025.04.18 - Fully Tested Update, GetByDate, GetByDateRange and Delete controller methods. Finished 
writing Service layer and fully tested it. Finished writing Validation layer with mapping middleware and 
validation response failure and fully tested it. Finished writing repository layer for a temporary dictionary
database. Configured launchSettings.json and Startup.cs to launch API for specified port.
+ 2025.04.19 - Fixed request JSON formatting for Coordinates by creating custom GeoCoordinate class with 
request and response templates. Tested all HTTP requests through Postman. Added database connection 
and initialisation files.
+ 2025.04.20 - Written and configured database layer with EF core code first events initialisation. Configured 
appsettings.json and startup files to connect to PostgreSQL. Registered database services. Created custom datetime 
conversion for PostgreSQL. Updated repository files to connect to the database.
+ 2025.04.21 - Added Microsoft.EntityFramework.InMemory package for testing EF Core functionality. Fully tested 
database layer and repository files.
+ 2025.04.22 - Created Web base layer and added CORS configuration in API to connect to the Web layer. 
Created calendar container with previous/next month paddings and buttons to navigate to next/previous months and 
years. Added current day display and loaded all events from database. 
+ 2025.04.23 - Updated web layout (index.html) and styling (styles.css) with better structured containers 
for calendar and headers. Created list event container with sorting mini algorithm. Added selected and current day 
visual functionality. Created date and weekday formatting for UK culture displays. Added event display with event 
information with ability to open geolocation in Google maps. Added event action buttons (WIP).
+ 2025.04.24 - Added Add event functionality with remembering selected date and window change based on allday bool.
Added pop up notification window for successful creation or failed validation. Added Search event functionality with 
4 event buttons: By ID, By Date, By Date Range, Show all events. For retrieving multiple events added sorting algorithm
and multiday event support. Added notification of total retrieved events. Added Delete event functionality with 
notification of deletion success/failure. Using AI created a unique sample of 250 events and populated the database 
through Postman. Tested all added event actions with new database samples. 
+ 2025.04.25 - Added Update button functionality (WIP) with a display window to search for ID first and then display
update window with old event information pre filled.
+ 2025.04.26 - Finished Update event action functionality. Fixed bug when location and description wouldn't be removed if 
deleted. Fixed bug for incorrect multi-day event display. Fixed bug when updating all day event displayed DateTime 
properties. Refactored App.js file into modular files into Actions, Calendar, Events and Utilities for better readability. 
Added separate HTML files for event actions HTML forms. Added several browser console logs to detect and debug errors.
Updated Azure pipeline to build Web layer then combine API and Web to build full App.
+ 2025.04.27 - Updated README.md
