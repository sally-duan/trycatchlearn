
using Microsoft.AspNetCore.SignalR;
using api.Extensions;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Authentication;
using api.SignalR;

namespace api.SignalR
{
    [Authorize]
    public class PresenceHub:Hub
    {
        private readonly PresenceTracker _tracker;
        public PresenceHub( PresenceTracker tracker)
        {
            _tracker = tracker;
        }
        public override async Task OnConnectedAsync()
        {
            await _tracker.UserConnected(Context.User.GetUsername(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());
            var currentUsers= await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

          public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _tracker.UserDisonnected(Context.User.GetUsername(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());
            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers",currentUsers); 
            
            await base.OnDisconnectedAsync(exception);
        }
    }
}