namespace QueueFlow.Application.Requests.Commands;

public record ChangeStatusCommand(Guid RequestId, string NewStatus, Guid ChangedByUserId);
