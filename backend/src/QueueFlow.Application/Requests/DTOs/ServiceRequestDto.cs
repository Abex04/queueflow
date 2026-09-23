namespace QueueFlow.Application.Requests.DTOs;

public record ServiceRequestDto(
    Guid Id,
    string Title,
    string Description,
    string ServiceType,
    string Status,
    Guid? AssignedTo,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record StatusHistoryDto(
    string FromStatus,
    string ToStatus,
    DateTime ChangedAt
);

public record ServiceRequestDetailDto(
    Guid Id,
    string Title,
    string Description,
    string ServiceType,
    string Status,
    Guid? AssignedTo,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyCollection<StatusHistoryDto> StatusHistory
);
