# Music Distribution Platform

A full-stack application for managing music distribution across Digital Service Providers (DSPs). Built with **.NET 8 API** and **Angular** frontend.

---

## 📋 Prerequisites

### Backend Requirements
- **.NET 8 SDK** or later
- **SQL Server** (LocalDB or full instance)
- Visual Studio or Visual Studio Code with C# extensions

### Frontend Requirements
- **Node.js** v18+ and **npm** v9+
- Angular CLI (installed globally or via npx)

---

## 🚀 Installation & Setup

### Backend Setup

#### 1. Configure Database Connection
Edit `MusicDistribution.API/appsettings.json` and update the connection string if needed:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=MusicDistributionDb;Trusted_Connection=True;"
  }
}
```

#### 2. Configure JWT Secret
Update the JWT configuration in `appsettings.json`:

```json
{
  "Jwt": {
    "Key": "YOUR_LONG_RANDOM_SECRET_AT_LEAST_32_CHARS_HERE",
    "Issuer": "MusicDistributionApi",
    "Audience": "MusicDistributionClient"
  }
}
```

> ⚠️ **Important**: Replace the `Key` with a strong, random secret of at least 32 characters for production. The app will throw `InvalidOperationException` on startup if this isn't configured.

#### 3. Build & Run Backend
```bash
cd MusicDistribution.API
dotnet restore
dotnet build
dotnet run
```

The API will start at **http://localhost:5011**

> Migrations are applied automatically on startup, and seed data is loaded into an empty database.

To run migrations manually instead:
```bash
dotnet ef database update --project MusicDistribution.DAL --startup-project MusicDistribution.API
```

---

### Frontend Setup

```bash
cd track-distribution-web
npm install
npm start
```

The frontend will be available at **http://localhost:4200**

---

## 🔐 Authentication

The API uses JWT for authentication.

**Default test credentials**: `admin` / `admin123`
(Test-only — see Security Notes below.)

#### Get a token
```bash
curl -X POST http://localhost:5011/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{ "username": "admin", "password": "admin123" }'
```

Response:
```json
{ "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." }
```

#### Use the token on protected endpoints
```bash
curl -X POST http://localhost:5011/api/tracks/1/distribute \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{ "dspIds": [1, 2, 3] }'
```

Tokens expire after 2 hours.

---

## 📡 API Endpoints

### Auth

| Method | Endpoint | Protected | Description |
|--------|----------|-----------|-------------|
| POST | `/api/auth/login` | No | Login and receive JWT token |

### Artists

| Method | Endpoint | Protected | Description |
|--------|----------|-----------|-------------|
| GET | `/api/artists` | No | Get all artists |
| POST | `/api/artists` | No | Create a new artist |

### Tracks

| Method | Endpoint | Protected | Description |
|--------|----------|-----------|-------------|
| GET | `/api/tracks` | No | List tracks (filter by `artistId`, `genre`, `status`) |
| GET | `/api/tracks/{id}` | No | Get track detail incl. DSP distribution statuses |
| POST | `/api/tracks` | No | Create a new track |
| POST | `/api/tracks/{id}/distribute` | **Yes** | Submit track to one or more DSPs |
| PATCH | `/api/tracks/{id}/status` | **Yes** | Update track status |

### DSPs

| Method | Endpoint | Protected | Description |
|--------|----------|-----------|-------------|
| GET | `/api/dsps` | No | Get all DSPs |
| POST | `/api/dsps` | No | Create a new DSP |

---

## 🧪 Testing

### Swagger UI
Navigate to **http://localhost:5011/swagger**. Use the "Authorize" button to add your JWT token, then test endpoints directly.

### cURL examples

**Create a track:**
```bash
curl -X POST http://localhost:5011/api/tracks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Song Title",
    "artistId": 1,
    "isrc": "USRC17607839",
    "genre": "Pop",
    "releaseDate": "2026-08-15"
  }'
```

**Distribute a track** (requires auth):
```bash
curl -X POST http://localhost:5011/api/tracks/1/distribute \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{ "dspIds": [1, 2] }'
```

**Update track status** (requires auth — valid values: `Draft`, `Submitted`, `Distributed`):
```bash
curl -X PATCH http://localhost:5011/api/tracks/1/status \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -H "Content-Type: application/json" \
  -d '{ "status": "Distributed" }'
```

### Frontend
```bash
cd track-distribution-web
npm test        # unit tests via Karma/Jasmine
npm run build   # production build → dist/
```
Or manually: open **http://localhost:4200**, browse the track list, filter by status, click into a track's detail view.

---

## 📊 Project Structure

```
MusicDistribution/
├── MusicDistribution.API/           # Presentation layer
│   ├── Controllers/
│   ├── Auth/                        # JWT token generation
│   └── appsettings.json
├── MusicDistribution.BLL/           # Business logic layer
│   ├── Services/
│   ├── DTOs/
│   └── Exceptions/
├── MusicDistribution.DAL/           # Data access layer
│   ├── AppDbContext.cs
│   ├── Entities/
│   ├── Repositories/
│   └── Migrations/
└── track-distribution-web/          # Angular frontend
    └── src/app/
```

---

## 🚨 Troubleshooting

| Issue | Solution |
|---|---|
| `Jwt:Key is not configured` | Set `Jwt:Key` in `appsettings.json`, minimum 32 characters |
| Database connection error | Confirm SQL Server/LocalDB is running and the connection string matches your environment |
| Migrations not applied | Run manually: `dotnet ef database update --project MusicDistribution.DAL --startup-project MusicDistribution.API` |
| Swagger says a required field is missing even though it's filled in | This is a Swagger UI validation-timing quirk — click outside the field to blur it before hitting Execute, or test via Postman/cURL instead |
| CORS errors from the frontend | Confirm the API is running on `http://localhost:5011` and the frontend origin (`http://localhost:4200`) matches the CORS policy in `Program.cs` |
| `npm install` fails | `npm cache clean --force && npm install` |

---

## 📝 Notes

- Default test user: `admin` / `admin123` (development only — see DECISIONS.md for the security note on this)
- Tokens expire after 2 hours
- Database runs on LocalDB by default — no separate install needed for local development
- CORS is scoped to `http://localhost:4200`

---

**Last Updated**: August 15, 2026