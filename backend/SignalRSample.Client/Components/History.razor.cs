using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using SignalRSample.Client.Models;
using SignalRSample.Client.Services;

namespace SignalRSample.Client.Components
{
    public partial class History
    {
        [Inject] private HttpClient HttpClient { get; set; }
        [Inject] private IAccessTokenProvider AccessTokenProvider { get; set; }

        [Inject] private SignalRService SignalRService { get; set; }

        private GameHistory[] _history;

        protected override async Task OnInitializedAsync()
        {
            var accessTokenValue = await AccessTokenProvider.RequestAccessToken();
            if (accessTokenValue.Status == AccessTokenResultStatus.Success)
            {
                _history = await HttpClient.GetFromJsonAsync<GameHistory[]>("GamesHistory");
                SignalRService.GameOver += async (sender, data) => await OnGameOver(sender, data);
            }
        }

        private async Task OnGameOver(object sender, GameOverEventArgs data)
        {
            Console.WriteLine("Game over is thrown", data.WinnerId);
            if (data.WinnerId != "Lost")
            {
                _history = await HttpClient.GetFromJsonAsync<GameHistory[]>("GamesHistory");
                StateHasChanged();
            }
        }
    }
}