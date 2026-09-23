# QueueFlow — Domain Model, API Contract & Task Ownership

## 1. Domain Model

### Enums

```csharp
public enum RequestStatus
{
    Waiting,
    Assigned,
    InProgress,
    Completed,
    Cancelled // optional, add later if time allows
}
```

### Entities

```csharp
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Role { get; set; } // "Citizen" | "Staff"
}

public class ServiceRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string ServiceType { get; set; }
    public RequestStatus Status { get; set; } = RequestStatus.Waiting;
    public Guid CreatedByUserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Assignment? Assignment { get; set; }
    public ICollection<StatusHistory> StatusHistory { get; set; } = new List<StatusHistory>();
}

public class Assignment
{
    public Guid Id { get; set; }
    public Guid ServiceRequestId { get; set; }
    public Guid StaffUserId { get; set; }
    public DateTime AssignedAt { get; set; }
}

public class StatusHistory
{
    public Guid Id { get; set; }
    public Guid ServiceRequestId { get; set; }
    public RequestStatus FromStatus { get; set; }
    public RequestStatus ToStatus { get; set; }
    public DateTime ChangedAt { get; set; }
    public Guid ChangedByUserId { get; set; }
}
```

### Valid Status Transitions (enforce in domain, not just UI)

```
Waiting     -> Assigned
Assigned    -> InProgress
InProgress  -> Completed
InProgress  -> Cancelled   (optional)
```

Any other transition should be rejected by the domain layer (throw a domain exception, return 400).

---

## 2. API Contract

Agree on this before Dev 3 and the backend devs split off. Treat changes to it as something that gets announced in the team sync, not silently pushed.

```http
POST   /api/requests                 → create a request
GET    /api/requests                 → list requests (optionally ?status=Waiting)
GET    /api/requests/{id}            → get one request with history
PATCH  /api/requests/{id}/status     → change status
POST   /api/requests/{id}/assign     → assign to staff
```

### POST /api/requests

Request:
```json
{
  "title": "Passport service request",
  "description": "Need document verification",
  "serviceType": "DocumentVerification",
  "createdByUserId": "user-guid"
}
```

Response `201`:
```json
{
  "id": "request-guid",
  "title": "Passport service request",
  "description": "Need document verification",
  "serviceType": "DocumentVerification",
  "status": "Waiting",
  "assignedTo": null,
  "createdAt": "2026-09-23T10:00:00Z",
  "updatedAt": "2026-09-23T10:00:00Z"
}
```

### GET /api/requests

Response `200`:
```json
[
  {
    "id": "request-guid",
    "title": "Passport service request",
    "status": "Waiting",
    "assignedTo": null,
    "createdAt": "2026-09-23T10:00:00Z"
  }
]
```

### GET /api/requests/{id}

Response `200`:
```json
{
  "id": "request-guid",
  "title": "Passport service request",
  "description": "Need document verification",
  "serviceType": "DocumentVerification",
  "status": "InProgress",
  "assignedTo": "staff-guid",
  "createdAt": "2026-09-23T10:00:00Z",
  "updatedAt": "2026-09-23T10:15:00Z",
  "statusHistory": [
    { "fromStatus": "Waiting", "toStatus": "Assigned", "changedAt": "2026-09-23T10:10:00Z" },
    { "fromStatus": "Assigned", "toStatus": "InProgress", "changedAt": "2026-09-23T10:15:00Z" }
  ]
}
```

### PATCH /api/requests/{id}/status

Request:
```json
{ "newStatus": "InProgress", "changedByUserId": "staff-guid" }
```
Response `200`: updated request object (same shape as GET one). `400` if transition is invalid.

### POST /api/requests/{id}/assign

Request:
```json
{ "staffUserId": "staff-guid" }
```
Response `200`: updated request object with `assignedTo` set and status moved to `Assigned`.

### Error shape (agree on this too — Dev 3 needs to know what to expect)

```json
{
  "error": "InvalidStatusTransition",
  "message": "Cannot move from Waiting directly to Completed."
}
```

---

## 3. SignalR Contract

Hub route: `/hubs/requests`

Server → client event:
```
Event name:  "RequestStatusChanged"
Payload:     { "requestId": "...", "status": "InProgress", "assignedTo": "staff-guid" }
```

Angular subscribes on the request list and request-detail pages, and patches local state on receipt rather than re-fetching.

---

## 4. Task Ownership (unchanged from the original split — it's the right shape)

| Dev | Owns | Branch prefix |
|---|---|---|
| Dev 1 | Domain entities/enums, Application commands/queries/DTOs, validation | `feature/domain`, `feature/application` |
| Dev 2 | EF Core, `AppDbContext`, configurations, migrations, Docker Compose for Postgres | `feature/database` |
| Dev 3 | Angular structure, pages, services, mocked-data-first UI | `feature/frontend-*` |
| Dev 4 | Controllers, DI, CORS, Swagger, SignalR hub, integration checks | `feature/api-signalr` |

**Rule to keep:** Dev 3 builds against the mocked API contract above and swaps in real calls once Dev 4's endpoints exist — no blocking on backend being "done."

## 5. Milestones (no clock — sequence, not time slots)

1. Domain model + API contract agreed (this doc) → everyone starts in parallel
2. `docker compose up` gets Postgres running + first migration applied (Dev 2)
3. First vertical slice works end-to-end: Angular form → POST → DB → 201 → UI shows it
4. List, status-change, assign all wired end-to-end
5. One SignalR event flowing (status change → live UI update)
6. README + docker-compose + demo script

Want me to also scaffold the actual repo structure (backend solution + Angular workspace skeleton) as a starting point, or draft the README next?
