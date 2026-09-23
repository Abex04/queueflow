using QueueFlow.Application.Requests.DTOs;

namespace QueueFlow.Application.Requests.Commands;

public record CreateServiceRequestCommand(
    string Title,
    string Description,
    string ServiceType,
    Guid CreatedByUserId
);

// Handler: load repository, call ServiceRequest.Create, save, map to ServiceRequestDto.
// Wire with MediatR or a plain application service - team's choice, keep it consistent.
public interface ICreateServiceRequestHandler
{
    Task<ServiceRequestDto> Handle(CreateServiceRequestCommand command, CancellationToken ct);
}
