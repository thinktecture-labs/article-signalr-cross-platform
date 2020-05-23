using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorSignalRSample.Client.Models;
using BlazorSignalRSample.Client.Services;
using Sotsera.Blazor.Oidc;

namespace BlazorSignalRSample.Client.Services
{
    public class UserService 
    {
        public EventHandler UserConnected;
        private readonly SignalRService _signalRService;
        private readonly IUserManager _userManager;


        public UserService(SignalRService signalRService, IUserManager userManager)
        {
            _signalRService = signalRService ?? throw new ArgumentNullException(nameof(signalRService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signalRService.UserConnected += OnUserConnected;
        }

        public async Task<bool> HasUsers() 
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userManager.UserState.AccessToken);
            var userString = await httpClient.GetStringAsync("http://localhost:5002/users");
            var currentUsers = JsonSerializer.Deserialize<User[]>(userString);
            var result = currentUsers.Length > 0 && currentUsers.Count(u => u.connectionId != _signalRService.ConnectionId) > 0;
            Console.WriteLine($"{JsonSerializer.Serialize(currentUsers)}, {_signalRService.ConnectionId}, {result}, {currentUsers.Count(u => u.connectionId != _signalRService.ConnectionId)}");
            return currentUsers.Length > 0 && currentUsers.Count(u => u.connectionId != _signalRService.ConnectionId) > 0;
        }

        private void OnUserConnected(object sender, UserEventArgs data)
        {
            Console.WriteLine($"User connected, {data}");
            UserConnected?.Invoke(this, null);
        }
    }
}