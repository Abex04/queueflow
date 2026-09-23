using Microsoft.AspNetCore.Mvc;
using QueueFlow.Application.Requests.Commands;
using QueueFlow.Application.Requests.DTOs;

namespace QueueFlow.Api.Controllers;

[ApiController]
[Route("api/requests")]
public class RequestsController : ControllerBase
{
    // TODO Dev 4: inject handlers/mediator, call into Application layer.
    // Keep controllers thin - no business logic here.

    [HttpPost]
    public async Task<ActionResult<ServiceRequestDto>> Create([FromBody] CreateServiceRequestCommand command)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceRequestDto>>> List([FromQuery] string? status)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceRequestDetailDto>> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<ServiceRequestDto>> ChangeStatus(Guid id, [FromBody] ChangeStatusRequestBody body)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{id:guid}/assign")]
    public async Task<ActionResult<ServiceRequestDto>> Assign(Guid id, [FromBody] AssignRequestBody body)
    {
        throw new NotImplementedException();
    }
}

public record ChangeStatusRequestBody(string NewStatus, Guid ChangedByUserId);
public record AssignRequestBody(Guid StaffUserId);
