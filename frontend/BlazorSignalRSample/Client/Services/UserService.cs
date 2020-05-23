using System;
using System.Collections.Generic;
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
        private readonly SignalRService _signalRService;
        private readonly IUserManager _userManager;
        private List<User> _users = new List<User>();
        public EventHandler<User[]> CurrentUsers;


        public UserService(SignalRService signalRService, IUserManager userManager)
        {
            _signalRService = signalRService ?? throw new ArgumentNullException(nameof(signalRService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _signalRService.UserConnected += OnUserConnected;
            _signalRService.UserDisconnected += OnUserDisconnected;
        }

        public async Task Initialize() 
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _userManager.UserState.AccessToken);
            var userString = await httpClient.GetStringAsync("http://localhost:5002/users");
            var currentUsers = JsonSerializer.Deserialize<User[]>(userString);
            _users = currentUsers.Where(u => u.connectionId != _signalRService.ConnectionId).ToList();
            Console.WriteLine(JsonSerializer.Serialize(_users));
            CurrentUsers?.Invoke(this, _users.ToArray());
        }

        private void OnUserConnected(object sender, UserEventArgs data)
        {
            Console.WriteLine($"User connected, {data}");
            if (!_users.Any(u => u.connectionId == data.User.connectionId)) 
            {
                _users.Add(data.User);
                CurrentUsers?.Invoke(this, _users.ToArray());
            }
        }

        private void OnUserDisconnected(object sender, UserEventArgs data)
        {
            Console.WriteLine($"User disconnected, {data}");
            var currentUser = _users.FirstOrDefault(u => u.connectionId == data.User.connectionId);
            if (currentUser != null) 
            {
                _users.Remove(currentUser);
                CurrentUsers?.Invoke(this, _users.ToArray());
            }
        }
    }
}