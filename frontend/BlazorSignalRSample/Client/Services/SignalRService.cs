using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using BlazorSignalRSample.Client.Models;
using MatBlazor;
using Microsoft.AspNetCore.SignalR.Client;
using Sotsera.Blazor.Oidc;

namespace BlazorSignalRSample.Client.Services
{
    public class SignalRService 
    {
        private HubConnection _hubConnection;
        private IMatToaster _toaster;
        private IUserManager _manager;
        public EventHandler<GameEventArgs> RoundPlayed;
        public EventHandler ResetGame;
        public EventHandler<UserEventArgs> UserConnected;
        public EventHandler<UserEventArgs> UserDisconnected;

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;
        public string ConnectionId => _hubConnection.ConnectionId;

        public SignalRService(IUserManager manager, IMatToaster toaster)
        {
            _manager = manager ?? throw new ArgumentNullException();
            _toaster = toaster ?? throw new ArgumentNullException();
        }

        public async Task InitConnectionAsync()
        {
            if (IsConnected) {
                return;
            }
            var accessToken = _manager.UserState.AccessToken;
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"https://pj-tt-signalr.azurewebsites.net/tictactoe?access_token={accessToken}", options =>
                { 
                    options.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10) })
                .Build();

            _hubConnection.On<User>("UserConnected", (data) =>
            {
                Console.WriteLine("SignalRService: User Disconnected");
                UserConnected?.Invoke(this, new UserEventArgs() { User = data });
            });
            _hubConnection.On<User>("UserDisconnected", (data) =>
            {
                Console.WriteLine("SignalRService: User Disconnected");
                UserDisconnected?.Invoke(this, new UserEventArgs() { User = data });
            });

            _hubConnection.On<string>("Play", (data) =>
            {
                Console.WriteLine($"Round Played. Data is {data}");
                RoundPlayed?.Invoke(this, new GameEventArgs() { Value = Int32.Parse(data) });
            });

            _hubConnection.On("Reset", () =>
            {
                ResetGame?.Invoke(this, null);
            });

            await _hubConnection.StartAsync();
            _toaster.Add("Erfolgreich am Hub angemeldet!", MatToastType.Success);
        }

        public async Task PlayRoundAsync(int value) 
        {
            Console.WriteLine("Send play round", value);
            await _hubConnection.SendAsync("PlayRound", $"{value}");
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
        public User User { get; set; }
    }
}