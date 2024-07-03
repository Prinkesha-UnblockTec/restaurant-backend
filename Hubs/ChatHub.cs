using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
namespace restaurant.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string roleName, string message)
        {
            await Clients.Group(roleName).SendAsync("ReceiveNotification", message);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var roleName = httpContext.Request.Query["roleName"];
            if (!string.IsNullOrEmpty(roleName))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roleName);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var roleName = httpContext.Request.Query["roleName"];
            if (!string.IsNullOrEmpty(roleName))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, roleName);
            }
            await base.OnDisconnectedAsync(exception);
        }
}
}