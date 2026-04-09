# 🚀 Customer Management System (CRM)

A full-stack **Customer Management System** built using **ASP.NET Core Web API** and **React.js (Vite)**.
This application enables efficient management of customers, authentication, and business workflows with a clean architecture approach.

---

## 📌 Project Overview

This system provides:

* Secure authentication using JWT
* Customer data management (CRUD operations)
* Scalable backend architecture (Clean Architecture)
* Modern frontend UI with React
* Containerized deployment using Docker

---

## 🛠️ Tech Stack

### 🔹 Backend

* C#
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* Redis (Caching)
* JWT Authentication

### 🔹 Frontend

* React.js (Vite)
* JavaScript (ES6+)
* CSS

### 🔹 DevOps & Tools

* Docker & Docker Compose
* Swagger (API Testing)
* Postman
* xUnit (Unit Testing)
* Git & GitHub

---

## 📂 Project Structure

```
Project2_CRM/
│
├── backend/
│   ├── src/
│   │   ├── CustomerManagement.API
│   │   ├── CustomerManagement.Application
│   │   ├── CustomerManagement.Domain
│   │   └── CustomerManagement.Infrastructure
│   │
│   └── tests/
│
├── frontend/
│   └── customer-management-ui/
│
└── docker-compose.yml
```

---


---

### 🔹 1. Backend Setup

```bash
cd backend/src/CustomerManagement.API
dotnet restore
dotnet build
dotnet run
```

👉 Backend will run at:

```
http://localhost:5000
```

👉 Swagger UI:

```
http://localhost:5000/swagger
```

---

### 🔹 2. Frontend Setup

```bash
cd frontend/customer-management-ui
npm install
npm run dev
```

👉 Frontend will run at:

```
http://localhost:5173
```

---

## 🐳 Docker Setup (Optional but Recommended)

### 🔹 Build & Run Containers

```bash
docker-compose up --build
```

---

### 🔹 Run in Background

```bash
docker-compose up -d
```

---

### 🔹 Stop Containers

```bash
docker-compose down
```

---

## 🔐 Features

* 🔑 JWT Authentication & Authorization
* 👤 Customer CRUD Operations
* 🏗️ Clean Architecture (API, Application, Domain, Infrastructure)
* ⚡ Redis Caching for performance
* 📄 Swagger API Documentation
* 🐳 Dockerized Full Stack Application

---

## 🔗 API Endpoints (Sample)

| Method | Endpoint            | Description        |
| ------ | ------------------- | ------------------ |
| GET    | /api/customers      | Get all customers  |
| GET    | /api/customers/{id} | Get customer by ID |
| POST   | /api/customers      | Create customer    |
| PUT    | /api/customers/{id} | Update customer    |
| DELETE | /api/customers/{id} | Delete customer    |

---

## 🧪 Running Tests

```bash
cd backend/tests
dotnet test
```

---

## ⚠️ Common Issues & Solutions

### ❌ 1. `vite not recognized`

✔ Fix:

```bash
npm install
```

---

### ❌ 2. `.csproj not found`

✔ Run commands inside:

```
backend/src/CustomerManagement.API
```

---

### ❌ 3. Docker Error: `no configuration file`

✔ Ensure:

```
docker-compose.yml is in root folder
```

---

### ❌ 4. Backend build errors (Handlers not found)

✔ Run:

```bash
cd backend
dotnet restore
dotnet build
```

---

## 🚀 Deployment Ideas

* 🌐 Frontend → Netlify / Vercel
* ⚙️ Backend → Azure App Service / AWS
* 🐳 Full Stack → Docker + Cloud VM

---

## 👨‍💻 Author

**Omkar Pandilwar**
Project Contributor: Yogesh Gohane

---

## 📈 Future Enhancements

* 📊 Dashboard analytics
* 🔔 Notification system
* 💳 Payment integration
* 🌍 Multi-user roles & permissions

---

## 📄 License

This project is created for educational and development purposes.
