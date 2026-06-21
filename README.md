# SSRMS - Secure Student Records Management System(using sql server)
using sql server## Overview
A Windows Forms application with a modern dark-themed dashboard and role-based access control for university student record management.

## Features
- Login system with hashed passwords
- Admin, Instructor, TA, Student, and Guest roles
- Role-based sidebar navigation and RBAC enforcement
- Admin user management and role request handling
- Student role upgrade request form
- Dashboard statistics and secure content panels

## Project Structure
- `Program.cs` - application entry point
- `Data/DatabaseHelper.cs` - SQL Server connection and authentication logic
- `Data/Models.cs` - shared data models and session state
- `Forms/LoginForm.cs` - secure login experience
- `Forms/MainDashboardForm.cs` - role-driven main interface
- `Forms/Theme.cs` - shared dark UI palette

## Database Schema
Create the following tables in SQL Server:

```sql
CREATE TABLE Users (
    Username NVARCHAR(50) PRIMARY KEY,
    PasswordHash NVARCHAR(256) NOT NULL,
    PasswordSalt NVARCHAR(256) NOT NULL,
    Role NVARCHAR(32) NOT NULL,
    ClearanceLevel INT NOT NULL,
    FullName NVARCHAR(128) NOT NULL,
    Department NVARCHAR(64),
    Email NVARCHAR(128)
);

CREATE TABLE Students (
    StudentId INT IDENTITY PRIMARY KEY,
    FullName NVARCHAR(128),
    Program NVARCHAR(64),
    YearLevel INT,
    ClearanceLevel INT
);

CREATE TABLE Instructors (
    InstructorId INT IDENTITY PRIMARY KEY,
    FullName NVARCHAR(128),
    Department NVARCHAR(64),
    ClearanceLevel INT
);

CREATE TABLE Courses (
    CourseCode NVARCHAR(16) PRIMARY KEY,
    Title NVARCHAR(128),
    Department NVARCHAR(64),
    PublicInfo NVARCHAR(256)
);

CREATE TABLE Grades (
    GradeId INT IDENTITY PRIMARY KEY,
    StudentId INT REFERENCES Students(StudentId),
    CourseCode NVARCHAR(16) REFERENCES Courses(CourseCode),
    CourseTitle NVARCHAR(128),
    Score DECIMAL(5,2),
    Term NVARCHAR(32)
);

CREATE TABLE Attendance (
    AttendanceId INT IDENTITY PRIMARY KEY,
    StudentId INT REFERENCES Students(StudentId),
    CourseCode NVARCHAR(16) REFERENCES Courses(CourseCode),
    Date DATETIME,
    Status NVARCHAR(32)
);

CREATE TABLE RoleRequests (
    RequestId INT IDENTITY PRIMARY KEY,
    Username NVARCHAR(50) REFERENCES Users(Username),
    CurrentRole NVARCHAR(32),
    RequestedRole NVARCHAR(32),
    Justification NVARCHAR(512),
    RequestDate DATETIME,
    Status NVARCHAR(32)
);
```

## Setup
1. Open the project in Visual Studio.
2. Update the connection string in `Data/DatabaseHelper.cs`.
3. Build and run the app.
4. Seed an admin user by calling `DatabaseHelper.CreateAccount(...)` or insert a hashed user manually.

## Notes
- The application uses role-based access and session tracking.
- UI uses a dark palette and responsive panel layout for a professional look.
- Add additional forms or modules for deeper role-specific workflows.
