# SmartEMS .NET API

SmartEMS is a full-stack Employee Management System built using ASP.NET Core Web API, Angular, and MySQL.

This backend provides secure REST APIs for employee management, attendance tracking, leave workflows, and attendance correction management with JWT-based authentication and role-based authorization.

---

# 🚀 Features

## Authentication & Authorization
- JWT Authentication
- Role-Based Access Control (Admin / Employee)
- Secure Password Hashing using BCrypt
- Protected APIs with Authorization

---

## Admin Features
- Admin Dashboard Summary
- Create Employee
- View Employees with Pagination & Search
- Update Employee Details
- Activate / Deactivate Employees
- Reset Employee Password
- Approve / Reject Leave Requests
- Approve / Reject Attendance Corrections

---

## Employee Features
- Employee Dashboard
- View & Update Profile
- Change Password
- Punch In / Punch Out Attendance
- Attendance Calendar
- Apply Leave
- Request Attendance Correction
- View Leave & Correction Status

---

# 🛠️ Tech Stack

- ASP.NET Core 8 Web API
- Entity Framework Core
- MySQL
- JWT Authentication
- AutoMapper
- BCrypt
- Clean Architecture
- REST API

---

# 🏗️ Architecture

The backend follows clean architecture principles:

```text
Controllers → Services → Repositories → Database
```

## Layers

### Controllers
Handle HTTP requests and responses.

### Services
Contain business logic and workflows.

### Repositories
Handle database operations and data access.

---

# 🔐 Authentication

JWT-based authentication is implemented using access tokens.

After successful login:
- Backend generates JWT token
- Frontend stores token
- Token is sent in Authorization header for protected APIs

---

# 🗄️ Database

Database: MySQL

Main Tables:
- Users
- Employees
- Departments
- Designations
- Attendance
- LeaveRequests
- CorrectionRequests

---

# 📦 Key Concepts Implemented

- JWT Authentication
- Role-Based Authorization
- Pagination
- Search Filtering
- Transactions
- Global Error Handling
- DTO Mapping
- Attendance Workflow
- Leave Workflow
- Correction Approval Workflow

---

# ⚙️ Setup Instructions

## 1. Clone Repository

```bash
git clone <repo-url>
```

---

## 2. Navigate to Project

```bash
cd SmartEMS.API
```

---

## 3. Update Database Connection

Update:

```json
appsettings.json
```

with your MySQL connection string.

---

## 4. Install Dependencies

```bash
dotnet restore
```

---

## 5. Run Migrations

```bash
dotnet ef database update
```

---

## 6. Run Project

```bash
dotnet run
```

---

# 🌐 API Base URL

```text
http://localhost:5000/api
```

---

# 📸 Screenshots

Add screenshots here later:

- Login
- Dashboard
- Employee Management
- Attendance
- Leave Management

---

# 🎯 Learning Outcome

This project helped me understand:
- Backend Architecture
- REST API Design
- Authentication & Authorization
- Entity Framework Core
- Database Relationships
- Clean Architecture
- Workflow-Based Business Logic

---

# 👨‍💻 Author

Karthick K
