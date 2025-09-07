# 🔹 CSharp.Essentials.Extensions (Demo)

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

> **Official demo repository** for the `CSharpEssentials.*` packages.  
> This project contains practical examples that showcase how to integrate:
> - **HttpHelper** → simplified HTTP requests with configuration from `appsettings.json`
> - **LoggerHelper** → structured logging with multiple sinks (Telegram, xUnit, Console, File, etc.)

---

## 🌐 HttpHelper

This demo uses `CSharpEssentials.HttpHelper` with `appsettings.json` to show how to easily configure and test GitHub API calls.

📖 Full documentation here 👉 [HttpHelper README](https://github.com/alexbypa/CSharp.Essentials/blob/main/CSharpEssentials.HttpHelper/README.md)

---

## 📊 LoggerHelper

This demo also demonstrates how **HttpHelper** and **LoggerHelper** work together:

- Trace API requests and responses.  
- Send logs to multiple sinks (Console, File, Telegram, xUnit).  
- Prepare for future integration with metrics and traces (OpenTelemetry).  

📖 Full documentation here 👉 [LoggerHelper on NuGet](https://www.nuget.org/packages/CSharpEssentials.LoggerHelper/)

---

## 🧩 CSharpEssentials Ecosystem

- `CSharpEssentials.HttpHelper` → HttpClient wrapper with configuration support  
- `CSharpEssentials.LoggerHelper` → structured logging + multi-sink support  
- `CSharpEssentials.HangFireHelper` → Hangfire extensions  
- `CSharp.Essentials.Extensions` → **this demo repository**

---

## 🚀 Purpose

This repository serves as a **unified demo** to show how to:

- Configure the packages.  
- Integrate them into a .NET 8 Web API.  
- Use available sinks and extensions.  

👉 Future updates will also include a **premium dashboard** for metrics and traces.
