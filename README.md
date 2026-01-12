# 🪙 TheCurseOfKnowledge

**TheCurseOfKnowledge** adalah platform monitoring harga cryptocurrency real-time yang dibangun dengan arsitektur modern (Clean Architecture) untuk menjamin skalabilitas dan kemudahan pemeliharaan. Proyek ini menangani streaming data harga dalam volume tinggi menggunakan gRPC dan SignalR dengan RabbitMQ sebagai message broker.

---

## 🏗️ Architecture & Structure

Proyek ini menerapkan **Clean Architecture** (Onion Architecture) untuk memisahkan logika bisnis dari detail infrastruktur. Struktur Solution dibagi menjadi beberapa layer logis:

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

### 04. Gateways
Pintu masuk dan keluar data untuk sisi client.
- **SignalR.Gateway**: Melakukan broadcast harga real-time ke UI menggunakan WebSockets.
- **Ocelot.ApiGateway**: Unified entry point untuk akses API.

### 05. UI (Presentation)
Frontend berbasis **Blazor** untuk visualisasi data:
- **Blazor.Bootstrap**: Dashboard menggunakan komponen Bootstrap.
- **Blazor.DevExtreme**: Dashboard premium dengan visualisasi chart data-intensive.

### 06. Shared
Project penghubung antar service.
- **Protos**: Definisi kontrak **gRPC**.
- **DTOs**: Data Transfer Objects untuk konsistensi data antar layer.

---

## 🛠️ Tech Stack

- **Framework**: .NET 8
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