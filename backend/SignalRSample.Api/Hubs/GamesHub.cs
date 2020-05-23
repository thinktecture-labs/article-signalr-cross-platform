using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Api.Extensions;
using SignalRSample.Api.Services;

namespace SignalRSample.Api.Hubs
{
    [Authorize]
    public class GamesHub : Hub
    {
        private readonly UsersService _usersService;

        public GamesHub(UsersService usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        public async Task SendNotification(string message)
        {
            await Clients.Others.SendAsync("Notifications", message);
        }

        public Task PlayRound(string data)
        {
            return Clients.Others.SendAsync("Play", data);
        }

        public Task ResetGame()
        {
            return Clients.Others.SendAsync("Reset");
        }

        public override async Task OnConnectedAsync()
        {
            _usersService.AddUser(Context.ConnectionId, Context.User.UserName());
            await Clients.Others.SendAsync("UserConnected",
                $"Ein neuer Benutzer hat sich angemeldet: {Context.User.UserName()}");
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            _usersService.RemoveUser(Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}