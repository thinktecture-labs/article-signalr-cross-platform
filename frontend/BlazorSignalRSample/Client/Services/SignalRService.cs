using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Sotsera.Blazor.Oidc;

namespace BlazorSignalRSample.Client.Services
{
    public class SignalRService 
    {
        private static HubConnection _hubConnection;
        private IUserManager _manager;
        public EventHandler<GameEventArgs> RoundPlayed;
        public EventHandler ResetGame;
        public EventHandler<UserEventArgs> UserConnected;

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public SignalRService(IUserManager manager)
        {
            _manager = manager ?? throw new ArgumentNullException();
        }

        public async Task InitConnectionAsync()
        {
            var accessToken = _manager.UserState.AccessToken;
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://localhost:5002/notifications?access_token={accessToken}")
                .Build();


            await _hubConnection.StartAsync();

            _hubConnection.On<string>("UserConnected", (data) =>
            {
                UserConnected?.Invoke(this, new UserEventArgs() { UserName = data });
            });

            _hubConnection.On<object>("Play", (data) =>
            {
                Console.WriteLine("Round Played", data);
                // RoundPlayed?.Invoke(this, new GameEventArgs() { Value = data });
            });

            _hubConnection.On("Reset", () =>
            {
                ResetGame?.Invoke(this, null);
            });
            
        }

        public async Task PlayRoundAsync(int value) 
        {
            Console.WriteLine("Send play round", value);
            await _hubConnection.InvokeAsync("PlayRound", $"{value}");
        }

        public async Task ResetGameAsync() 
        {
            await _hubConnection.SendAsync("ResetGame");
        }
    }

    public class GameEventArgs : EventArgs 
    {
        public int Value { get; set; }
    }

    public class UserEventArgs : EventArgs 
    {
        public string UserName { get; set; }
    }
}