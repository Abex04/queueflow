# QueueFlow — Digital Service Queue System

Practice hackathon project (not CivicFix). Citizens create service requests;
staff claim/assign and update status; citizens see status changes live via SignalR.

## Stack
Angular → ASP.NET Core (Clean Architecture) → EF Core → PostgreSQL → SignalR → Docker

## Repo layout
```
queueflow/
├── backend/
│   └── src/
│       ├── QueueFlow.Api/            (Dev 4 - controllers, hub, DI, CORS)
│       ├── QueueFlow.Application/    (Dev 1 - commands, queries, DTOs)
│       ├── QueueFlow.Domain/         (Dev 1 - entities, enums, rules)
│       └── QueueFlow.Infrastructure/ (Dev 2 - EF Core, Postgres, migrations)
├── frontend/
│   └── queueflow-web/                (Dev 3 - Angular app)
├── docs/                             (API contract, domain model - see docs/plan.md)
├── docker-compose.yml
└── README.md
```

## Getting started

### 1. Start PostgreSQL
```bash
docker compose up -d
```

### 2. Backend
```bash
cd backend
dotnet sln add src/QueueFlow.Domain src/QueueFlow.Application src/QueueFlow.Infrastructure src/QueueFlow.Api
dotnet restore
dotnet ef migrations add InitialCreate -p src/QueueFlow.Infrastructure -s src/QueueFlow.Api
dotnet ef database update -p src/QueueFlow.Infrastructure -s src/QueueFlow.Api
dotnet run --project src/QueueFlow.Api
```
API runs at `http://localhost:5000`, Swagger at `/swagger`.

### 3. Frontend
```bash
cd frontend/queueflow-web
npm install
npm start
```
Angular runs at `http://localhost:4200`.

## API contract & domain model
See `docs/plan.md` for the full entity model, endpoint contract, and SignalR event shape.

## Team ownership
| Dev | Owns |
|---|---|
| Dev 1 | Domain + Application |
| Dev 2 | Infrastructure + PostgreSQL + Docker |
| Dev 3 | Angular frontend |
| Dev 4 | API + SignalR + integration |

## Status
- [ ] Docker Postgres running
- [ ] First migration applied
- [ ] Vertical slice: create request end-to-end
- [ ] List / status-change / assign wired
- [ ] SignalR live update working
- [ ] Demo script ready
