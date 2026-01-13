# 🪙 TheCurseOfKnowledge

**TheCurseOfKnowledge** is a high-performance cryptocurrency real-time monitoring platform built with **Clean Architecture** (Onion Architecture). The system is designed for high scalability and low-latency data streaming, utilizing **gRPC**, **SignalR**, and **RabbitMQ**.

---

## 🏗️ Architecture & Structure

The solution is organized into logical layers to ensure a strict **Separation of Concerns**.

```text
TheCurseOfKnowledge.sln
├── 01. Core
│   └── TheCurseOfKnowledge.Core (Business Logic, Entities, & Technical Calculators)
├── 02. Infrastructure
│   └── TheCurseOfKnowledge.Infrastructure (Dapper ORM, RabbitMQ Integration, DB Access)
├── 03. Services (Background Engines)
│   ├── TheCurseOfKnowledge.Worker.DataFetcher (Binance API Ingestion)
│   ├── TheCurseOfKnowledge.Worker.Analyzer (Technical Indicators Engine)
│   └── TheCurseOfKnowledge.GRPC.Backbone (Core gRPC Service - Load Balanced)
├── 04. Gateways
│   ├── TheCurseOfKnowledge.SignalR.Gateway (Real-time Broadcast to WebSockets)
│   ├── TheCurseOfKnowledge.Ocelot.ApiGateway (Unified REST Entry Point)
│   └── TheCurseOfKnowledge.Gateway.Proxy (YARP gRPC & HTTP Reverse Proxy)
├── 05. UI (Presentation Layer)
│   ├── TheCurseOfKnowledge.Blazor.Bootstrap (Light-weight Web UI)
│   ├── TheCurseOfKnowledge.Blazor.DevEx (Premium Web Dashboard via DevExpress)
│   └── TheCurseOfKnowledge.Desktop.DevEx (Native WinForms .NET 5 High-Speed App)
└── 06. Shared
    └── TheCurseOfKnowledge.Shared (Protocol Buffers / Proto Files & Common DTOs)

### 01. Core
Jantung dari aplikasi. Berisi logika bisnis murni tanpa ketergantungan pada library eksternal atau database.
- **Entities**: Domain model (Coin, PriceHistory).
- **Interfaces**: Kontrak untuk repository dan service.
- **Calculators**: Logika perhitungan indikator teknikal (MA, RSI, Bollinger Bands).

### 02. Infrastructure
Implementasi teknis dan integrasi alat pihak ketiga.
- **Data**: Data access layer menggunakan **Dapper**.
- **Messaging**: Integrasi **RabbitMQ** (Producer/Consumer).
- **ExternalServices**: API Client untuk Binance/Indodax.

### 03. Services (Background Engines)
Microservices yang berjalan di latar belakang.
- **Worker.DataFetcher**: Mengambil harga dari exchange dan mempublish ke RabbitMQ.
- **Worker.Analyzer**: Mengonsumsi data dari queue untuk perhitungan teknikal.
- **GRPC.Backbone**: The core gRPC service (Load balanced by the Proxy).

### 04. Gateways
Pintu masuk dan keluar data untuk sisi client.
- **SignalR.Gateway**: Melakukan broadcast harga real-time ke UI menggunakan WebSockets.
- **TheCurseOfKnowledge.Gateway.Proxy**: (NEW) High-performance gRPC & HTTP Reverse Proxy built with **YARP**.
    * Handles **Client-Side Load Balancing** (Round Robin).
    * Provides **gRPC-Web** support for Browser/UI compatibility.
    * Global **CORS Policy** (AllowAll) for Blazor/Angular integration.
- **Ocelot.ApiGateway**: Unified entry point untuk akses API.

### 05. UI (Presentation)
Frontend berbasis **Blazor** untuk visualisasi data:
- **Blazor.Bootstrap**: Dashboard menggunakan komponen Bootstrap.
- **Blazor.DevEx**: Dashboard premium dengan visualisasi chart data-intensive.
- **Desktop.DevEx**: (NEW) Native Windows Desktop Application.

### 06. Shared
Project penghubung antar service.
- **Protos**: Definisi kontrak **gRPC**.
- **DTOs**: Data Transfer Objects untuk konsistensi data antar layer.

---

## 🛠️ Tech Stack

- **Framework**: .NET 5
- **Communication**: gRPC (Internal), SignalR (Real-time UI)
- **Message Broker**: RabbitMQ
- **Database**: SQL Server with Dapper ORM
- **UI Framework**: Blazor WebAssembly
- **Components**: DevExtreme & Blazor Bootstrap

---

## 🚀 Getting Started

1. Clone repository ini.
2. Pastikan Docker berjalan (untuk RabbitMQ & SQL Server).
3. Jalankan `TheCurseOfKnowledge.sln` di Visual Studio.
4. Set multiple startup projects untuk menjalankan Worker, Gateway, dan UI secara bersamaan.

---
*"Junior developers write code that works. Senior developers write code that can be maintained by others."*