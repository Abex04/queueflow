using QueueFlow.Domain.Enums;

namespace QueueFlow.Domain.Entities;

public class StatusHistory
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ServiceRequestId { get; private set; }
    public RequestStatus FromStatus { get; private set; }
    public RequestStatus ToStatus { get; private set; }
    public DateTime ChangedAt { get; private set; } = DateTime.UtcNow;
    public Guid ChangedByUserId { get; private set; }

    private StatusHistory() { } // EF Core

    public StatusHistory(Guid serviceRequestId, RequestStatus from, RequestStatus to, Guid changedByUserId)
    {
        ServiceRequestId = serviceRequestId;
        FromStatus = from;
        ToStatus = to;
        ChangedByUserId = changedByUserId;
    }
}
