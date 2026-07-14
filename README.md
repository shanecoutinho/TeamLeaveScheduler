# Team Leave Scheduler

## Overview

Team Leave Scheduler is a lightweight HR leave management application that allows managers to:

- View approved leave for the next 30 days
- Submit leave requests
- Approve or reject pending leave requests
- Enforce business rules regarding team leave capacity and overlapping leave

## Tech Stack

### Backend
- ASP.NET Core 8 Web API
- Entity Framework Core
- SQLite

### Frontend
- React (Vite)
- Axios

## Features

- View approved leave for the next 30 days
- View pending leave requests
- Submit leave requests
- Approve leave requests
- Reject leave requests
- SQLite database with seeded data
- REST API
- Unit tests

## Business Rules

- No more than 30% of a team may be on approved leave on any working day.
- Weekends are not counted as working days.
- Public holidays do not count towards leave.
- Employees cannot submit leave that overlaps an existing approved leave request.
- End date cannot be before the start date.

## Running the Backend

```bash
cd backend/LeaveScheduler.API

dotnet restore

dotnet run
```

Swagger:

```
http://localhost:5083/swagger
```

## Running the Frontend

```bash
cd frontend

npm install

npm run dev
```

Frontend:

```
http://localhost:5173
```

## Running Tests

```bash
dotnet test
```
