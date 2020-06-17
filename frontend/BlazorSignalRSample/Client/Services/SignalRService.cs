using System;
using System.Reactive.Subjects;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorSignalRSample.Client.Models;
using MatBlazor;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Sotsera.Blazor.Oidc;

namespace BlazorSignalRSample.Client.Services
{
    public class SignalRService 
    {
        private HubConnection _hubConnection;
        private IMatToaster _toaster;
        private IUserManager _manager;
        private IConfiguration _configuration;

        public event EventHandler<GameRunningEventArgs> GameRunning;
        public event EventHandler<GameOverEventArgs> GameOver;
        public event EventHandler<ActiveSessionEventArgs> ActiveSession;
        public event EventHandler<GameEventArgs> RoundPlayed;

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;
        public string ConnectionId => _hubConnection.ConnectionId;

        public SignalRService(IUserManager manager, IMatToaster toaster, IConfiguration configuartion)
        {
            _manager = manager ?? throw new ArgumentNullException();
            _toaster = toaster ?? throw new ArgumentNullException();
            _configuration = configuartion ?? throw new ArgumentNullException();
        }

        public async Task InitConnectionAsync()
        {
            if (IsConnected) {
                return;
            }
            var accessToken = _manager.UserState.AccessToken;
            var apiBaseUrl = _configuration["api:baseUrl"];
            Console.WriteLine(JsonSerializer.Serialize(_configuration));
            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{apiBaseUrl}tictactoe?access_token={accessToken}", options =>
                { 
                    options.AccessTokenProvider = () => Task.FromResult(accessToken);
                })
                .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10) })
                .Build();


            

            // NEW ARCHITECTURE
            _hubConnection.On("StartGame", (GameSession data) =>
            {
                GameRunning?.Invoke(this, new GameRunningEventArgs { Running = true });
                ActiveSession?.Invoke(this, new ActiveSessionEventArgs { Session = data });
            });

            _hubConnection.On("GameOver", (string data) =>
            {
                GameRunning?.Invoke(this, new GameRunningEventArgs { Running = false });
                ActiveSession?.Invoke(this, new ActiveSessionEventArgs { Session = null });
                GameOver?.Invoke(this, new GameOverEventArgs { WinnerId = data});
            });

            _hubConnection.On<int>("Play", (data) =>
            {
                Console.WriteLine($"Round Played. Data is {data}");
                RoundPlayed?.Invoke(this, new GameEventArgs() { Value = data });
            });

            await _hubConnection.StartAsync();
            _toaster.Add("Erfolgreich am Hub angemeldet!", MatToastType.Success);
            await JoinNewSession();
        }

        public async Task JoinNewSession() {
            await _hubConnection.SendAsync("JoinSession");
            GameOver?.Invoke(this, new GameOverEventArgs { WinnerId = String.Empty });
        }

        public async Task PlayRoundAsync(int value) 
        {
            Console.WriteLine("Send play round", value);
            await _hubConnection.SendAsync("PlayRound", value);
        }
    }

    public class GameRunningEventArgs : EventArgs 
    {
        public bool Running { get; set; }
    }

    public class GameOverEventArgs : EventArgs 
    {
        public string WinnerId { get; set; }
    }

    public class ActiveSessionEventArgs : EventArgs 
    {
        public GameSession Session { get; set; }
    }

    public class GameEventArgs : EventArgs 
    {
        public int Value { get; set; }
    }
}
