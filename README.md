# Feeding Campaigns Management System

A real-world full-stack system designed for NGOs to manage feeding campaigns, donations, beneficiary families, inventory, and meal distributions.

This project includes:

- **Backend:** ASP.NET Core 8 Web API + Entity Framework Core + SQL Server + JWT Authentication  
- **Frontend:** React 18 + Vite + TailwindCSS (Admin Dashboard)  
- **Documentation:** SRS, ERD, UML, and System Architecture  
- **Database:** Code-First + Auto Seeding  

---

## 🚀 Overview

The Feeding Campaigns system digitizes the daily operations of NGOs:

- Create & manage feeding campaigns (Ramadan, Winter Relief, Emergency Aid…)
- Track monetary & food donations
- Manage beneficiary families and vulnerability scoring
- Record meal distribution batches across branches
- Maintain accurate food inventory
- Provide a modern admin dashboard for analytics

This system is suitable for:
- Graduation projects  
- NGO prototype systems  
- Real-world training  
- Portfolio and job interviews  

---

## 🏗 System Architecture

### 🔹 Backend Architecture (ASP.NET Core 8)
- RESTful API  
- JWT Authentication  
- Role-based Authorization  
- EF Core (Code First)  
- Repository/Service-like structure  
- Seeding initial data (NGO, Branch, Users, Campaigns)

### 🔹 Frontend Architecture (React)
- React 18 SPA  
- Vite build system  
- TailwindCSS styling  
- React Router  
- Axios + JWT Interceptor  
- Protected Routes  
- Dashboard Components  

---

## 📂 Project Structure

### 🟦 Backend: `FeedingCampaigns.Api`
```

FeedingCampaigns.Api/
├── Models/
├── Dtos/
├── Controllers/
├── Services/
├── Config/
├── Data/
├── appsettings.json
└── Program.cs

```

### 🟩 Frontend: `FeedingCampaignsDashboardReact`
```

FeedingCampaignsDashboardReact/
├── src/
│   ├── api/
│   ├── auth/
│   ├── components/
│   ├── pages/
│   ├── App.jsx
│   └── main.jsx
├── index.html
├── package.json
└── tailwind.config.js

````

---

## 🔐 Authentication & Roles

System uses **JWT authentication + role-based authorization**.

### Roles:
- **SystemAdmin**  
- **NgoAdmin**  
- **BranchManager**  
- **Volunteer**  
- **Donor**  

### Auth Endpoints:
- `POST /api/auth/login`  
- `POST /api/auth/register`  

Token is stored in `localStorage` and injected in API requests.

---

## 🧩 Main Features

### ✔ Campaigns
- Create campaigns  
- Change campaign status  
- Search & filter campaigns  
- View campaign details + progress stats  

### ✔ Donations
- Monetary donations  
- Food donations with item quantities  
- Automatic inventory updates  

### ✔ Beneficiary Families
- Register families  
- Vulnerability scoring  
- Sorting & filtering  

### ✔ Distributions
- Create distribution batches  
- Meals delivered per family  
- Update campaign progress automatically  

### ✔ Inventory Tracking
- Inbound (from donations)  
- Outbound (to distributions)  
- Adjustments  

### ✔ Dashboard Analytics
- Total campaigns  
- Total meals prepared/distributed  
- Beneficiary count  
- Recent campaigns  
- Top vulnerable families  

---

## 🔧 Backend Setup

### 1️⃣ Restore packages
```bash
dotnet restore
````

### 2️⃣ Configure SQL Server

Modify `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=192.168.1.1\\SQLEXPRESS;Database=FeedingCampaignsRealDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 3️⃣ Run API

```bash
dotnet run
```



---

## 💻 Frontend Setup

### 1️⃣ Install dependencies

```bash
npm install
```

### 2️⃣ Run dev server

```bash
npm run dev
```



---

## 🔑 Demo Login Credentials

| Role          | Email                                                     | Password    |
| ------------- | --------------------------------------------------------- | ----------- |
| SystemAdmin   | [admin@hopefeeding.org](mailto:admin@hopefeeding.org)     | Admin@123   |
| BranchManager | [manager@hopefeeding.org](mailto:manager@hopefeeding.org) | Manager@123 |
| Donor         | [donor@hopefeeding.org](mailto:donor@hopefeeding.org)     | Donor@123   |

---

## 📚 Documentation (Included)

Located in `docs/`:

* **SRS.md** – Software Requirements Specification
* **Architecture.md** – System Architecture
* **erd.dbml** – Database Diagram
* **class-diagram.puml** – UML Diagram
* **README.md** – (this file)

---

## 📌 Future Improvements

* Volunteer Mobile App (React Native)
* Notifications (Email, SMS)
* File uploads (IDs, receipts)
* Full reporting module
* Audit Trail
* Multi-NGO support

---

## © 2025 Feeding Campaigns System

Professional full-stack implementation for NGO digital transformation.
