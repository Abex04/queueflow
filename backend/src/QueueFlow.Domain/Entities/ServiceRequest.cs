using QueueFlow.Domain.Common;
using QueueFlow.Domain.Enums;

namespace QueueFlow.Domain.Entities;

public class ServiceRequest
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string ServiceType { get; private set; } = default!;
    public RequestStatus Status { get; private set; } = RequestStatus.Waiting;
    public Guid CreatedByUserId { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    public Assignment? Assignment { get; private set; }
    private readonly List<StatusHistory> _statusHistory = new();
    public IReadOnlyCollection<StatusHistory> StatusHistory => _statusHistory.AsReadOnly();

    private ServiceRequest() { } // EF Core

    public static ServiceRequest Create(string title, string description, string serviceType, Guid createdByUserId)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("InvalidTitle", "Title is required.");

        return new ServiceRequest
        {
            Title = title,
            Description = description,
            ServiceType = serviceType,
            CreatedByUserId = createdByUserId
        };
    }

    private static readonly Dictionary<RequestStatus, RequestStatus[]> AllowedTransitions = new()
    {
        [RequestStatus.Waiting] = new[] { RequestStatus.Assigned },
        [RequestStatus.Assigned] = new[] { RequestStatus.InProgress },
        [RequestStatus.InProgress] = new[] { RequestStatus.Completed, RequestStatus.Cancelled },
    };

    public void ChangeStatus(RequestStatus newStatus, Guid changedByUserId)
    {
        if (!AllowedTransitions.TryGetValue(Status, out var allowed) || !allowed.Contains(newStatus))
            throw new DomainException("InvalidStatusTransition", $"Cannot move from {Status} to {newStatus}.");

        _statusHistory.Add(new StatusHistory(Id, Status, newStatus, changedByUserId));
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignTo(Guid staffUserId)
    {
        if (Status != RequestStatus.Waiting)
            throw new DomainException("AlreadyAssigned", "Request is not in Waiting status.");

        Assignment = new Assignment(Id, staffUserId);
        ChangeStatus(RequestStatus.Assigned, staffUserId);
    }
}
