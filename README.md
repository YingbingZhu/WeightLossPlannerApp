# 🥗 WeightLossPlannerApp

A full-stack web app for tracking meals, workouts, and calories using:

- **ASP.NET Core Web API** (`WeightLossPlannerAPI`)
- **Blazor WebAssembly** frontend (`WeightLossPlannerUI`)
- **Azure SQL** as the database

---

## 🚀 Features

- 🍽️ Log daily meals and calories
- 🏃 Track workouts and burned calories
- 📊 View net calorie balance
- 🔐 Connected to Azure SQL securely

---

## 🔧 Configuration

This app uses **Azure SQL Database**. The connection string is not committed to GitHub for security.

To run locally, you'll need to configure your connection string securely using the following:
```bash
cd WeightLossPlannerAPI
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<Your Azure SQL connection string here>"

## 🧪 Getting Started

```bash
# Run backend API
cd WeightLossPlannerAPI
dotnet run

# Run Blazor frontend
cd ../WeightLossPlannerUI
dotnet run


Then open the app at:
http://localhost:5179 (UI)
https://localhost:5038/api (API)