namespace QueueFlow.Application.Requests.Queries;

public record GetRequestsQuery(string? StatusFilter);

public record GetRequestByIdQuery(Guid Id);
