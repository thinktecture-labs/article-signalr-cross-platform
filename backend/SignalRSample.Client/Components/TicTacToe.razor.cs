using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using SignalRSample.Client.Services;

namespace SignalRSample.Client.Components
{
    public partial class TicTacToe
    {
        [Inject] private SignalRService SignalRService { get; set; }
        private string[] _cells = { };
        private bool _gameOver;
        private bool _gameRunning;
        private string _turn = "O";
        private bool _waitForOtherUser;
        private string _winner = "";
        private string _opponent = "";

        protected override async Task OnInitializedAsync()
        {
            SignalRService.RoundPlayed += OtherPlayRound;
            SignalRService.GameRunning += OnGameRunning;
            SignalRService.GameOver += OnGameOver;
            SignalRService.ActiveSession += OnActiveSession;

            await SignalRService.InitConnectionAsync();
            InitBoard();
        }

        private string CellCssClass(string value)
        {
            Console.WriteLine(value);
            return value != _turn ? "cell other" : "cell";
        }

        private void InitBoard()
        {
            var result = new List<string>();
            for (var i = 0; i < 9; i++)
            {
                result.Insert(i, null);
            }

            _cells = result.ToArray();
            _turn = "O";
            _gameOver = false;
            _winner = null;
        }

        private async Task ResetGame()
        {
            InitBoard();
            StateHasChanged();
            await SignalRService.JoinNewSession();
        }

        private void OnGameRunning(object sender, GameRunningEventArgs data)
        {
            Console.WriteLine($"Game is Running: {data.Running}");
            _gameRunning = data.Running;
            _gameOver = false;
            StateHasChanged();
        }

        private void OnGameOver(object sender, GameOverEventArgs data)
        {
            if (data.WinnerId == "Tie")
            {
                _winner = "Unentschieden";
            }
            else if (data.WinnerId == "Lost")
            {
                _winner = "Der Gegner hat die Verbindung verloren oder aufgegeben!";
            }
            else if (!String.IsNullOrWhiteSpace(data.WinnerId))
            {
                _winner = data.WinnerId == SignalRService.ConnectionId
                    ? "Du hast gewonnen!"
                    : "Du hast leider verloren.";
            }

            _gameOver = !String.IsNullOrWhiteSpace(data.WinnerId);

            if (_gameOver)
            {
                _opponent = String.Empty;
            }

            StateHasChanged();
        }

        private void OnActiveSession(object sender, ActiveSessionEventArgs data)
        {
            Console.WriteLine($"Start Game: {data.Session}");
            if (data.Session != null)
            {
                _waitForOtherUser = data.Session.ActiveUser != SignalRService.ConnectionId;
                _opponent = data.Session.UserOne.ConnectionId != SignalRService.ConnectionId
                    ? data.Session.UserOne.Name
                    : data.Session.UserTwo.Name;
                _gameOver = false;
            }

            StateHasChanged();
        }

        private void OtherPlayRound(object sender, GameEventArgs data)
        {
            Console.WriteLine("Other played round", data);
            if (!_gameOver)
            {
                if (_cells[data.Value] == null)
                {
                    _cells[data.Value] = "X";
                    _waitForOtherUser = false;
                }
            }

            StateHasChanged();
        }

        private async Task PlayRound(int data)
        {
            if (!_gameOver && !_waitForOtherUser)
            {
                if (_cells[data] == null)
                {
                    _cells[data] = _turn;
                    Console.WriteLine(data);
                    await SignalRService.PlayRoundAsync(data);
                    _waitForOtherUser = true;
                }
            }

            StateHasChanged();
        }
    }
}