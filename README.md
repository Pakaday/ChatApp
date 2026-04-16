# ChatApp — Backend API

The ASP.NET Core backend for a real-time chat application. Handles user management, message persistence, and real-time delivery via SignalR WebSockets. Pairs with the [chat-frontend](https://github.com/Pakaday/chat-frontend) React application.

## What This Is

A learning project built to understand real-time bidirectional communication using SignalR — how WebSocket connections are established, how the server pushes events to connected clients, and how persistent chat history and live messaging coexist in the same system.

## Stack

- .NET 8
- ASP.NET Core Web API
- SignalR (real-time WebSocket hub)
- Entity Framework Core + PostgreSQL (Npgsql)

## Architecture

```
React Frontend (SignalR client)
        │
        │  WebSocket connection
        ▼
    ChatHub  ──────────────────────────────▶  PostgreSQL
  (SignalR Hub)                              (message history)
        │
        │  Broadcasts to all connected clients
        ▼
All connected React clients receive the message
```

When a message is sent:
1. Frontend invokes `SendMessage` on the hub over the WebSocket connection
2. Hub looks up the sender by email, validates input
3. Message is persisted to PostgreSQL with sender, recipient, content, and timestamp
4. Hub broadcasts `ReceiveMessage` to all connected clients in real time

## Key Files

| File | Purpose |
|------|---------|
| `Hubs/ChatHub.cs` | SignalR hub — handles connections and message routing |
| `Models/Message.cs` | Message entity with sender, recipient, content, soft delete flag |
| `Models/User.cs` | User entity synced from Supabase auth on login |
| `Data/ChatDbContext.cs` | EF Core context |
| `Controllers/UsersController.cs` | REST endpoint for user creation and retrieval |
| `Controllers/MessagesController.cs` | REST endpoint for loading message history |

## Key Design Decisions

**SignalR for real-time delivery** — Rather than polling, the frontend maintains a persistent WebSocket connection. The server pushes messages to all clients the instant they're sent.

**Message persistence** — Messages are written to PostgreSQL before being broadcast. This means history survives reconnects and page refreshes.

**Soft delete on messages** — `IsDeleted` flag is included on the Message model to support future message deletion without destroying history.

**GUID string as message ID** — IDs are generated as `Guid.NewGuid().ToString()` server-side rather than relying on auto-increment, making them safe to reference before they're confirmed persisted.

**CORS configured for local dev** — Allows credentials from `http://localhost:5173` (the Vite dev server), required for SignalR's WebSocket handshake.

## Running Locally

**Prerequisites:** .NET 8 SDK, PostgreSQL

**1. Update the connection string** in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=chatapp;Username=postgres;Password=yourpassword"
  }
}
```

**2. Apply migrations and run:**

```bash
dotnet ef database update
dotnet run
```

The API runs on `https://localhost:5180` by default. Swagger UI available at `/swagger`.

## What's Next

- Scope message broadcast to conversation participants rather than all connected clients
- Load message history filtered to a specific conversation on connect
- Add typing indicators via SignalR hub events
- Authentication middleware to validate Supabase JWT tokens on the backend
