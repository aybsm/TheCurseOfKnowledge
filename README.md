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
```

### 01. Core

The heart of the application. It contains pure business logic without dependencies on external libraries or databases.

* **Entities**: Domain models (Coin, PriceHistory).
* **Interfaces**: Contracts for repositories and services.
* **Calculators**: Logic for technical indicator calculations (MA, RSI, Bollinger Bands).

### 02. Infrastructure

Technical implementations and third-party tool integrations.

* **Data**: Data access layer using **Dapper**.
* **Messaging**: **RabbitMQ** integration (Producer/Consumer).
* **External Services**: API Clients for Binance and Indodax.

### 03. Services (Background Engines)

Microservices running in the background.

* **Worker.DataFetcher**: Fetches prices from exchanges and publishes them to RabbitMQ.
* **Worker.Analyzer**: Consumes data from the queue for technical analysis calculations.
* **GRPC.Backbone**: The core gRPC service, which is load-balanced by the Proxy.

### 04. Gateways

Data entry and exit points for clients.

* **SignalR.Gateway**: Broadcasts real-time prices to the UI using WebSockets.
* **TheCurseOfKnowledge.Gateway.Proxy**: A high-performance gRPC & HTTP Reverse Proxy built with **YARP**.
* Handles **Client-Side Load Balancing** (Round Robin).
* Provides **gRPC-Web** support for Browser and UI compatibility.
* Implements a global **CORS Policy** (Allow All) for Blazor and Angular integration.


* **Ocelot.ApiGateway**: A unified entry point for API access.

### 05. UI (Presentation)

Frontend interfaces for data visualization:

* **Blazor.Bootstrap**: Dashboard using standard Bootstrap components.
* **Blazor.DevEx**: Premium dashboard with data-intensive chart visualizations.
* **Desktop.DevEx**: A native Windows Desktop application providing high-speed updates.

### 06. Shared

The project that bridges all services.

* **Protos**: **gRPC** contract definitions.
* **DTOs**: Data Transfer Objects for data consistency across layers.

---

## 🛠️ Tech Stack

* **Framework**: .NET 5
* **Communication**: gRPC (Internal), SignalR (Real-time UI)
* **Message Broker**: RabbitMQ
* **Caching & State Management**: Redis
* **Containerization**: Docker
* **Database**: SQL Server with Dapper ORM
* **UI Framework**: Blazor WebAssembly
* **Components**: DevExpress (DevEx) & Blazor Bootstrap

---

## 🤝 Acknowledgment

Special thanks to **Gemini**, my AI thought partner, for providing deep architectural insights, technical guidance, and helping me maintain clean code standards throughout the development of this project. This collaboration has been instrumental in overcoming complex technical challenges.

---

## 🚀 Getting Started

1. **Clone** this repository.
2. Ensure **Docker** is running for RabbitMQ and SQL Server instances.
3. Open `TheCurseOfKnowledge.sln` in **Visual Studio**.
4. Set **Multiple Startup Projects** to run the Workers, Gateway, and UI simultaneously.

---

*"Junior developers write code that works. Senior developers write code that can be maintained by others."*
