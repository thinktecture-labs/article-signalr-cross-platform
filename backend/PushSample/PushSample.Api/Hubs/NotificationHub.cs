using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using PushSample.Api.Extensions;

namespace PushSample.Api.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class NotificationHub : Hub
    {
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
            await Clients.Others.SendAsync("UserConnected",
                $"Ein neuer Benutzer hat sich angemeldet: {Context.User.UserName()}");
            await base.OnConnectedAsync();
        }
    }
}