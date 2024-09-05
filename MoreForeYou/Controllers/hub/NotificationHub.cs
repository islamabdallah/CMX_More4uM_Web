
using Microsoft.AspNetCore.SignalR;
using MoreForYou.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace MoreForYou.Controllers.hub
{
    public class NotificationHub : Hub
    {
        private readonly IUserConnectionManager _userConnectionManager;
        public NotificationHub(IUserConnectionManager userConnectionManager)
        {
            _userConnectionManager = userConnectionManager;
        }
        public string GetConnectionId()
        {
            var httpContext = this.Context.GetHttpContext();
            var userId = httpContext.Request.Query["userId"];
            _userConnectionManager.KeepUserConnection(userId, Context.ConnectionId); return Context.ConnectionId;
        }
        //Called when a connection with the hub is terminated.
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            //get the connectionId
            var connectionId = Context.ConnectionId;
            _userConnectionManager.RemoveUserConnection(connectionId);
            var value = await Task.FromResult(0);
        }
    }
}
