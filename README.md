# Library Management System

A WPF application for managing books, members, and loan transactions with a clean, multi-layer architecture.

---

## Prerequisites

* **.NET 6 SDK** (or later) installed.
* **SQLite** support via `Microsoft.Data.Sqlite` (bundled).
* Visual Studio, JetBrains Rider, or another C# IDE.

---

## Configuration

1. **Clone the repo**:

   ```bash
   git clone https://github.com/RaresMarta/Library-Management-System.git
   cd LibrarySystem
   ```

2. **appsettings.json** (copied to output on build):

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Data Source=Data/LibraryDB.sqlite"
     }
   }
   ```

   * On first launch, the app creates `LibraryDB.sqlite` and the required tables.
   * Subsequent runs will reuse the existing database and apply any migrations.

3. **DatabaseInitializer**:

   * Automatically creates or migrates **Books**, **Members**, and **Loans** tables.

---

## Building & Running

From the **root** of the repository (where `LibrarySystem.csproj` lives):

1. **Restore dependencies**:

   ```bash
   dotnet restore
   ```

2. **Build**:

   ```bash
   dotnet build
   ```

3. **Run**:

   ```bash
   dotnet run --project LibrarySystem.csproj
   ```

   Or simply:

   ```bash
   dotnet run
   ```

   if your working directory is the project root, and then select `LibrarySystem` as the startup project if prompted by your IDE.

---

## Core Features

* **CRUD** for **Books** and **Members** with validations.
* **Search** books by **Title** and **Author** filters.
* **Lending** and **Return** flows:

   * Prevent loans when stock is depleted or member already has an open loan.
   * Prevent returns when loan is already returned.
* **Renewals**: extend due dates for active loans.
* **Automatic Overdue**: loans past due are marked **Overdue** on each refresh or startup.
* **Loan History Tracking**: view history per book and per member in dedicated windows.

---

## Analytics Dashboard (New Feature)

The **Analytics Dashboard** provides real-time insights on library usage:

1. **Genre Distribution**

   * Shows a table of each **Genre** and the number of books in that category.

2. **Loan Status Distribution**

   * Displays counts of **Active** and **Overdue** loans.

3. **Top 5 Borrowed Books**

   * Lists the five most frequently borrowed books with borrow counts.

4. **Accessing the Dashboard**

   * Click **Analytics Dashboard** in the main window to open.
   * Data updates automatically on open.

This dashboard helps librarians spot popular genres, track overdue load, and identify high-demand titles for ordering.

---

## Notes

* All business logic resides in the **Service** layer; UI only handles display and user interaction.
* The app uses **MVVM-inspired** patterns, with dependency injection of services into windows.
* No manual DB setup required: clone, build, and run!
