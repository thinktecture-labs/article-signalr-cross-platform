using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Api.Hubs;
using SignalRSample.Api.Models;

namespace SignalRSample.Api.Services
{
    public class GameSessionManager
    {
        private static readonly int[][] WinningOptions =
        {
            new[] {0, 1, 2},
            new[] {3, 4, 5},
            new[] {6, 7, 8},
            new[] {0, 3, 6},
            new[] {1, 4, 7},
            new[] {2, 5, 8},
            new[] {0, 4, 8},
            new[] {2, 4, 6},
        };

        private readonly IHubContext<GamesHub> _hubContext;
        private readonly List<GameSession> _sessions = new List<GameSession>();

        public GameSessionManager(IHubContext<GamesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task AddUserAsync(User client)
        {
            var session = _sessions.FirstOrDefault(s => s.UserTwo == null && s.UserOne != null);
            if (session != null)
            {
                Console.WriteLine(
                    $"Session found which is open and not started. {JsonSerializer.Serialize(session)}");
                session.UserTwo = client;
                session.ActiveUser = session.UserOne.ConnectionId;
            }
            else
            {
                session = new GameSession
                {
                    SessionId = $"Game{_sessions.Count}",
                    UserOne = client
                };
                _sessions.Add(session);
            }

            await _hubContext.Groups.AddToGroupAsync(client.ConnectionId, session.SessionId);
            if (!String.IsNullOrWhiteSpace(session.ActiveUser))
            {
                Console.WriteLine(
                    $"New Game will start now. Session: {session.SessionId}, User One: {session.UserOne}, User Two: {session.UserTwo}, Active User: {session.ActiveUser}");
                await _hubContext.Clients.Group(session.SessionId).SendAsync("StartGame", session);
            }
        }

        public async Task RemoveUserAsync(string clientId)
        {
            var session = _sessions.FirstOrDefault(s =>
                s.UserOne?.ConnectionId == clientId || s.UserTwo?.ConnectionId == clientId);
            if (session != null)
            {
                Console.WriteLine($"Remove user from Group. Session: {session.SessionId}, User: {clientId}");
                await _hubContext.Clients.Group(session.SessionId).SendAsync("GameOver", "Lost");
                await _hubContext.Groups.RemoveFromGroupAsync(session.UserTwo.ConnectionId, session.SessionId);
                await _hubContext.Groups.RemoveFromGroupAsync(session.UserOne.ConnectionId, session.SessionId);
                session.Moves = new List<KeyValuePair<string, int>>();
                session.ActiveUser = String.Empty;
            }
        }

        public async Task PlayRoundAsync(string clientId, int value)
        {
            var session = _sessions.FirstOrDefault(s =>
                (s.UserOne?.ConnectionId == clientId || s.UserTwo?.ConnectionId == clientId) &&
                s.ActiveUser == clientId);
            if (session != null)
            {
                session?.Moves.Add(new KeyValuePair<string, int>(clientId, value));
                if (CheckSessionState(session, out var winner))
                {
                    await _hubContext.Clients.Group(session.SessionId).SendAsync("GameOver", winner);
                    await _hubContext.Groups.RemoveFromGroupAsync(session.UserOne.ConnectionId, session.SessionId);
                    await _hubContext.Groups.RemoveFromGroupAsync(session.UserTwo.ConnectionId, session.SessionId);
                    _sessions.Remove(session);
                }
                else
                {
                    session.ActiveUser = session.UserOne.ConnectionId == clientId
                        ? session.UserTwo.ConnectionId
                        : session.UserOne.ConnectionId;
                    await _hubContext.Clients.GroupExcept(session.SessionId, clientId).SendAsync("Play", value);
                }
            }
            else
            {
                await _hubContext.Clients.User(clientId).SendAsync("NotAllowed");
            }
        }

        private bool CheckSessionState(GameSession gameSession, out string winner)
        {
            winner = String.Empty;
            var firstUser = gameSession.Moves.Where(m => m.Key == gameSession.UserOne.ConnectionId)
                .Select(m => m.Value).ToArray();
            var secondUser = gameSession.Moves.Where(m => m.Key == gameSession.UserTwo.ConnectionId)
                .Select(m => m.Value).ToArray();

            var gameOver = false;
            foreach (var line in WinningOptions)
            {
                if (line.Intersect(firstUser).Count() == line.Count())
                {
                    gameOver = true;
                    winner = gameSession.UserOne.ConnectionId;
                }
                else if (line.Intersect(secondUser).Count() == line.Count())
                {
                    gameOver = true;
                    winner = gameSession.UserTwo.ConnectionId;
                }
            }

            if (!gameOver)
            {
                var occupy = firstUser.Count() + secondUser.Count();
                if (occupy == 9)
                {
                    gameOver = true;
                    winner = "Tie";
                }
            }


            return gameOver;
        }
    }
}