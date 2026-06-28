# 🤖 AIIMS Appointment Agent

An automated appointment monitoring bot for the Government of India ORS Portal built using **.NET 8**, **Microsoft Playwright**, and **Telegram Bot API**.

The application continuously monitors AIIMS appointment availability and instantly sends Telegram notifications whenever new appointment slots become available.

---

## 🚀 Features

- ✅ Automated ORS website navigation
- ✅ Microsoft Playwright browser automation
- ✅ AIIMS New Delhi appointment monitoring
- ✅ Department specific monitoring (Orthopedics)
- ✅ Multi-month calendar scanning
- ✅ Automatic stop when unopened dates are reached
- ✅ Telegram instant notifications
- ✅ Duplicate notification prevention
- ✅ Background Worker Service
- ✅ Configurable architecture for future hospitals/departments

---

## 🛠️ Tech Stack

- .NET 8 Worker Service
- Microsoft Playwright
- Telegram Bot API
- Dependency Injection
- Background Services

---

## 📁 Project Structure

```
AppointmentAgent.Worker
│
├── Models
│   ├── OrsLocators.cs
│   └── SlotInfo.cs
│
├── Services
│   ├── PlaywrightService.cs
│   ├── TelegramService.cs
│   ├── NotificationCacheService.cs
│   └── OrsMonitorService.cs
│
├── Worker.cs
├── Program.cs
└── appsettings.json
```

---

## ⚙️ Workflow

```
Start Worker
      │
      ▼
Open ORS Portal
      │
      ▼
Select State
      │
      ▼
Select Hospital
      │
      ▼
Select Appointment Type
      │
      ▼
Select Department
      │
      ▼
Read Appointment Calendar
      │
      ▼
Available Slot Found?
      │
   Yes ▼ No
      │
Send Telegram Notification
      │
      ▼
Close Browser
```

---

## 📲 Telegram Notification

Whenever appointment slots become available, the bot sends a notification containing:

- Hospital Name
- Department
- Available Appointment Dates
- Detection Time
- ORS Portal Link

---

## 🔒 Security

Sensitive information such as Bot Tokens and Chat IDs are **not stored in the repository** and should be configured locally using application settings.

---

## 📌 Future Enhancements

- Multi Hospital Support
- Multi Department Monitoring
- Configuration Driven Monitoring
- Docker Support
- Windows Service Deployment
- Retry Policies
- Structured Logging
- Screenshot Capture on Failure

---

## ⚠️ Disclaimer

This project is intended for educational and automation learning purposes. Please ensure that your usage complies with the ORS portal's terms of service.

---

## 👨‍💻 Author

**Sushant Verma**

Senior .NET & Azure Developer

GitHub: https://github.com/SushantVerma247
