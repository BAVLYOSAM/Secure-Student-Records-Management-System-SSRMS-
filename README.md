# 🔐 Secure Student Records Management System (SSRMS)

A secure database-driven system for managing student academic records while enforcing advanced Database Security concepts including RBAC, MLS, Flow Control, Inference Control, Encryption, and Role Request Management.

Developed as part of the **Database Security Course**.

---

## 📌 Project Overview

SSRMS is a secure academic management system that protects sensitive student information such as:

- Student Profiles
- Grades
- Attendance Records
- User Accounts
- Course Information

The project demonstrates the practical implementation of multiple database security models using **Microsoft SQL Server**.

---

## 🚀 Features

### 🔑 Authentication & Authorization

- Secure Login System
- Password Hashing
- User Authentication
- Role-Based Access Control (RBAC)

### 👥 User Roles

The system supports five different roles:

| Role | Permissions |
|--------|-------------|
| Admin | Full Access |
| Instructor | Manage Grades & Attendance |
| TA | Attendance Management |
| Student | View Own Data |
| Guest | View Public Information |

---

## 🛡 Security Models Implemented

### 1. Role-Based Access Control (RBAC)

- SQL Roles
- GRANT / REVOKE / DENY
- Role-based GUI restrictions
- Permission verification

### 2. Multi-Level Security (MLS)

Implemented using Bell-LaPadula Model:

- No Read Up (NRU)
- No Write Down (NWD)

Security Levels:

- Top Secret
- Secret
- Confidential
- Unclassified

### 3. Inference Control

- Restricted Views
- Query Set Size Control
- Protection Against Data Disclosure

### 4. Flow Control

- Prevents unauthorized movement of classified data
- Blocks data leakage between security levels

### 5. Encryption at Rest

Sensitive information is encrypted using AES:

- Passwords
- Student IDs
- Phone Numbers
- Grades

---

## 🗄 Database Schema

### Main Tables

- Students
- Instructors
- Courses
- Grades
- Attendance
- Users
- RoleRequests

---

## 📊 Security Matrix

| Function | Admin | Instructor | TA | Student | Guest |
|-----------|--------|------------|----|---------|-------|
| View Profile | ✅ | ✅ | ✅ | ✅ | ❌ |
| Edit Profile | ✅ | ✅ | ✅ | ❌ | ❌ |
| View Grades | ✅ | ✅ | ❌ | ❌ | ❌ |
| Edit Grades | ✅ | ✅ | ❌ | ❌ | ❌ |
| View Attendance | ✅ | ✅ | ✅ | Own Only | ❌ |
| Manage Users | ✅ | ❌ | ❌ | ❌ | ❌ |
| View Courses | ✅ | ✅ | ✅ | ✅ | ✅ |

---

## 🔄 Role Request Workflow

Students can request role upgrades:

- Student → TA
- TA → Instructor

### Request Process

1. User submits a request.
2. Request is stored in RoleRequests table.
3. Status becomes Pending.
4. Admin reviews the request.
5. Admin approves or denies.
6. User role is updated if approved.

---

## 💻 Technologies Used

### Database

- Microsoft SQL Server
- Stored Procedures
- Views
- Triggers
- SQL Roles

### Application

- GUI Application
- SQL Server Integration

### Security

- RBAC
- MLS
- AES Encryption
- Flow Control
- Inference Control

---

## 📂 Project Structure

```text
SSRMS
│
├── Database
│   ├── Tables.sql
│   ├── Views.sql
│   ├── Procedures.sql
│   ├── Roles.sql
│
├── GUI
│
├── Documentation
│
├── Screenshots
│
└── README.md
```

---

## 🎯 Learning Outcomes

This project demonstrates:

- Database Design
- SQL Server Security
- Access Control Models
- Data Encryption
- Secure Authentication
- Security-Aware System Design

---

## 👨‍💻 Team

Database Security Course Project

Faculty of Computer Science and Information Technology

Helwan National University

---

## ⭐ Project Highlights

✔ Role-Based Access Control (RBAC)

✔ Multi-Level Security (MLS)

✔ Inference Control

✔ Flow Control

✔ AES Encryption

✔ Secure Authentication

✔ Role Upgrade Workflow

✔ SQL Server Implementation
