using System;
using System.Text.Json;
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

        public async Task JoinSession()
        {
            var user = await _usersService.GetUserAsync(Context.ConnectionId);
            Console.WriteLine(JsonSerializer.Serialize(user));
            if (user != null)
            {
                await _manager.AddUserAsync(user);
            }
        }

        public async Task PlayRound(int data)
        {
            Console.WriteLine(
                $"PlayRound: User {Context.User.UserName()}; ConnectionId: {Context.ConnectionId} Wert {data}");
            await _manager.PlayRoundAsync(Context.ConnectionId, data);
        }

        public override async Task OnConnectedAsync()
        {
            await _usersService.AddUserAsync(Context.ConnectionId, Context.User.UserName());
            await base.OnConnectedAsync();
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