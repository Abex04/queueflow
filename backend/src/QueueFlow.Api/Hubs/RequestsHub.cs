using Microsoft.AspNetCore.SignalR;

namespace QueueFlow.Api.Hubs;

// Route: /hubs/requests
public class RequestsHub : Hub
{
    // Server pushes "RequestStatusChanged" events from the command handlers
    // (inject IHubContext<RequestsHub> where status changes happen).
}
