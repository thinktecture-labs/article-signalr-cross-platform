using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Api.Extensions;
using SignalRSample.Api.Models;
using SignalRSample.Api.Services;

namespace SignalRSample.Api.Hubs
{
    [Authorize]
    public class GamesHub : Hub
    {
        private readonly IUsersService _usersService;
        private readonly GameSessionManager _manager;

        public GamesHub(IUsersService usersService, GameSessionManager manager)
        {
            _usersService = usersService;
            _manager = manager;
        }

        public async Task SendNotification(string message)
        {
            await Clients.Others.SendAsync("Notifications", message);
        }

        public async Task JoinSession()
        {
            await _manager.AddUserAsync(Context.ConnectionId);
        }

        public async Task PlayRound(int data)
        {
            await _manager.PlayRoundAsync(Context.ConnectionId, data);
        }

        public Task ResetGame()
        {
            return Clients.Others.SendAsync("Reset");
        }

        public User OwnConnectionId()
        {
            return new User
            {
                ConnectionId = Context.ConnectionId,
                Name = Context.User.UserName()
            };
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _manager.RemoveUserAsync(Context.ConnectionId);
            await _usersService.RemoveUserAsync(Context.ConnectionId);
            await Clients.Others.SendAsync("UserDisconnected", new User
            {
                ConnectionId = Context.ConnectionId,
                Name = Context.User.UserName()
            });
            await base.OnDisconnectedAsync(exception);
        }
    }
}