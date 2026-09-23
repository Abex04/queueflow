namespace QueueFlow.Domain.Entities;

public class Assignment
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ServiceRequestId { get; private set; }
    public Guid StaffUserId { get; private set; }
    public DateTime AssignedAt { get; private set; } = DateTime.UtcNow;

    private Assignment() { } // EF Core

    public Assignment(Guid serviceRequestId, Guid staffUserId)
    {
        ServiceRequestId = serviceRequestId;
        StaffUserId = staffUserId;
    }
}
