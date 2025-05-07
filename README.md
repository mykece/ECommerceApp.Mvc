# ECommerceApp.Mvc

A clean and maintainable online shopping platform built with ASP.NET Core MVC, supporting product listings, cart operations, and campaign management.

## 📌 Project Features

- User and Role Management (ASP.NET Core Identity)
- Product Listing and Filtering
- Shopping Cart Operations
- Discount and Campaign Management
- Authorization and Authentication
- Database Integration with **SQL Server**

---

## 🛠 Technologies Used

| Technology                | Description                    |
|---------------------------|--------------------------------|
| **.NET 7.0**              | Main development platform      |
| **ASP.NET Core MVC**      | Web application framework      |
| **Entity Framework Core** | ORM layer                      |
| **Identity Framework**    | User management                |
| **Bootstrap 5**           | Frontend design                |
| **Toastr.js**             | Notification management        |
| **MSSQL**                 | Database management            |

---

## 📂 Project Folder Structure

```plaintext
ECommerceApp.Mvc
├── ECommerceApp.Application
│   ├── DTOs
│   ├── Extensions
│   ├── Services
├── ECommerceApp.Domain
│   ├── Core
│   ├── Entities
│   ├── Enums
│   ├── Utilities
├── ECommerceApp.Infrastructure
│   ├── AppContext
│   ├── Configurations
│   ├── DataAccess
│   ├── Migrations
│   ├── Repositories
├── ECommerceApp.UI
│   ├── Controllers
│   ├── Models
│   ├── Views
│   ├── wwwroot
│   ├── Areas
└── README.md
```

---

## 📂 Managing API Keys

This project uses AI services that require API keys. These keys are stored in specific script files. To ensure security and functionality, update these placeholders with your own API keys:

1. **Locate the Files**: Search for files containing the text `"API_KEY_PLACEHOLDER"`.
2. **Replace the Keys**: Update `"API_KEY_PLACEHOLDER"` with your actual API key.
3. **Keep Your Keys Secure**: Avoid sharing your keys or uploading them to public repositories.

---

## 🗂 Database Schema

This project includes core components such as **user management**, **product catalogs**, **shopping cart**, and **campaign management**.

---

## 📅 Requirements

- **.NET 7.0** or newer version
- **SQL Server** or **SQL Server Express**
- **Visual Studio** or **Visual Studio Code** (optional)

---

## 🚀 Getting Started

### 1. Clone the Project

First, clone the project from GitHub to your computer:

```bash
git clone https://github.com/mykece/ECommerceApp.Mvc.git
```

### 2. Edit the Connection String

Edit the content of the `appsettings.Development.json` file as follows:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "AppConnectionDev": "Server=YOUR_SERVER_NAME;Database=ECommerceApp;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Apply Database Migrations

Before running the project for the first time, you need to apply database migrations to create the database schema.

a. Open the Package Manager Console (Visual Studio)

Go to Tools > NuGet Package Manager > Package Manager Console.

b. Apply the Migrations

Run the following commands to apply the migrations:

```bash
Add-Migration InitialCreate
Update-Database
```

---

### Now you can run the project...
