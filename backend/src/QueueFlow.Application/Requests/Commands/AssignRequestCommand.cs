namespace QueueFlow.Application.Requests.Commands;

public record AssignRequestCommand(Guid RequestId, Guid StaffUserId);
