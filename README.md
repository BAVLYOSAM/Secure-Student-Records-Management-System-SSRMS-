# Database Security RBAC System

A secure Role-Based Access Control (RBAC) system developed as a Database Security course project.

The system provides user authentication, role management, role request workflows, and administrative approval processes while enforcing security principles and preventing unauthorized privilege escalation.

---

## Overview

This project implements a secure user management platform based on RBAC (Role-Based Access Control).

Users can register, log in, and request role upgrades. Administrators review and approve or deny role requests through a dedicated dashboard.

The system ensures that users cannot assign roles to themselves and all role changes must be approved by an administrator.

---

## Features

### Authentication & Authorization

- User Registration
- Secure Login
- Password Protection
- Session Management
- Role-Based Access Control (RBAC)

### User Roles

The system supports multiple roles such as:

- Student
- Instructor
- Teaching Assistant
- Admin

Roles can be customized based on project requirements.

---

### Role Request System

Users can submit requests to change their current role.

Each request contains:

- Username
- Current Role
- Requested Role
- Reason
- Submission Date
- Request Status

Request statuses include:

- Pending
- Approved
- Denied

---

### Admin Dashboard

Administrators can view all pending role requests.

The dashboard displays:

- Username
- Current Role
- Requested Role
- Reason
- Date Submitted
- Status

Admin actions:

- Approve Request
- Deny Request

---

### Approval Workflow

When approved:

- User role is updated in the Users table.
- Request status becomes "Approved".

When denied:

- User role remains unchanged.
- Request status becomes "Denied".

---

### Security Features

- Principle of Least Privilege
- Role-Based Access Control
- Secure Authentication
- Controlled Role Escalation
- Audit-Friendly Request Tracking
- Prevention of Unauthorized Privilege Changes

---

## Database Design

### Users Table

| Column |
|----------|
| UserID |
| Username |
| Email |
| Password |
| Role |

---

### RoleRequests Table

| Column |
|----------|
| RequestID |
| UserID |
| CurrentRole |
| RequestedRole |
| Reason |
| Status |
| CreatedAt |

---

## System Workflow

1. User registers or logs in.
2. User submits a role request.
3. Request is stored in the RoleRequests table.
4. Status is set to Pending.
5. Admin reviews request.
6. Admin approves or denies.
7. System updates records accordingly.

---

## Technologies Used

### Frontend
- HTML5
- CSS3
- JavaScript

### Backend
- PHP / Node.js / ASP.NET (depending on implementation)

### Database
- MySQL
- SQL Server

---

## Security Concepts Applied

- Authentication
- Authorization
- RBAC
- Access Control Policies
- Privilege Management
- Database Security Best Practices

---

## Project Structure

```text
Database-Security-RBAC-System/
│
├── frontend/
│   ├── login
│   ├── register
│   ├── dashboard
│
├── backend/
│   ├── controllers
│   ├── services
│   ├── middleware
│
├── database/
│   ├── schema.sql
│   ├── seed.sql
│
├── screenshots/
│
├── README.md
│
└── LICENSE
