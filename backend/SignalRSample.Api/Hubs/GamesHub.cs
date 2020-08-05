using System;
using System.Text.Json;
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
        private readonly IUsersService _usersService;
        private readonly GameSessionManager _manager;

        public GamesHub(IUsersService usersService, GameSessionManager manager)
        {
            _usersService = usersService;
            _manager = manager;
        }

        // TODO: Check if the current User is already in a session btw. logged in
        public async Task JoinSession()
        {
            var user = await _usersService.GetUserBySubjectAsync(Context.User.SubId());
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
            await _usersService.AddUserAsync(Context.ConnectionId, Context.User.SubId(), Context.User.UserName());
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var user = await _usersService.GetUserBySubjectAsync(Context.User.SubId());
            await _manager.RemoveUserAsync(user.Id);
            await _usersService.RemoveUserAsync(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}