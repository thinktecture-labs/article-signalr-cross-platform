using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Api.Hubs;
using SignalRSample.Api.Models;

namespace SignalRSample.Api.Services
{
    public class GameSessionManager
    {
        private static int[][] _winningOptions =
        {
            new int[] {0, 1, 2},
            new int[] {3, 4, 5},
            new int[] {6, 7, 8},
            new int[] {0, 3, 6},
            new int[] {1, 4, 7},
            new int[] {2, 5, 8},
            new int[] {0, 4, 8},
            new int[] {2, 4, 6},
        };

        private readonly IHubContext<GamesHub> _hubContext;
        private List<GameSession> _sessions = new List<GameSession>();

        public GameSessionManager(IHubContext<GamesHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task AddUserAsync(string clientId)
        {
            var session = _sessions.FirstOrDefault(s => String.IsNullOrWhiteSpace(s.UserTwo));
            if (session != null)
            {
                session.UserTwo = clientId;
                session.ActiveUser = session.UserOne;
            }
            else
            {
                session = new GameSession
                {
                    SessionId = $"Game{_sessions.Count}",
                    UserOne = clientId
                };
                _sessions.Add(session);
            }

            await _hubContext.Groups.AddToGroupAsync(clientId, session.SessionId);
            if (!String.IsNullOrWhiteSpace(session.ActiveUser))
            {
                Console.WriteLine(
                    $"New Game will start now. Session: {session.SessionId}, User One: {session.UserOne}, User Two: {session.UserTwo}, Active User: {session.ActiveUser}");
                await _hubContext.Clients.Group(session.SessionId).SendAsync("StartGame", session);
            }
        }

        public async Task RemoveUserAsync(string contextId)
        {
            var session = _sessions.FirstOrDefault(s => String.IsNullOrWhiteSpace(s.UserTwo));
            if (session != null)
            {
                Console.WriteLine($"Remove user from Group. Session: {session.SessionId}, User: {contextId}");
                await _hubContext.Groups.RemoveFromGroupAsync(contextId, session.SessionId);
                await _hubContext.Clients.Group(session.SessionId).SendAsync("GameOver", "Lost");
                session.Moves = new List<KeyValuePair<string, int>>();
                session.ActiveUser = String.Empty;
            }
        }

        public async Task PlayRoundAsync(string contextId, int value)
        {
            var session = _sessions.FirstOrDefault(s => s.UserOne == contextId || s.UserTwo == contextId);
            if (session?.ActiveUser == contextId)
            {
                session?.Moves.Add(new KeyValuePair<string, int>(contextId, value));
                if (CheckSessionState(session, out var winner))
                {
                    await _hubContext.Clients.Group(session.SessionId).SendAsync("GameOver", winner);
                    await _hubContext.Groups.RemoveFromGroupAsync(session.UserOne, session.SessionId);
                    await _hubContext.Groups.RemoveFromGroupAsync(session.UserTwo, session.SessionId);
                    _sessions.Remove(session);
                }
                else
                {
                    session.ActiveUser = session.UserOne == contextId ? session.UserTwo : session.UserOne;
                    await _hubContext.Clients.GroupExcept(session.SessionId, contextId).SendAsync("Play", value);
                }
            }
            else
            {
                await _hubContext.Clients.User(contextId).SendAsync("NotAllowed");
            }
        }

        private bool CheckSessionState(GameSession gameSession, out string winner)
        {
            winner = String.Empty;
            var firstUser = gameSession.Moves.Where(m => m.Key == gameSession.UserOne).Select(m => m.Value);
            var secondUser = gameSession.Moves.Where(m => m.Key == gameSession.UserTwo).Select(m => m.Value);

            var gameOver = false;
            foreach (var line in _winningOptions)
            {
                if (line.Intersect(firstUser).Count() == line.Count())
                {
                    gameOver = true;
                    winner = gameSession.UserOne;
                }

                if (line.Intersect(secondUser).Count() == line.Count())
                {
                    gameOver = true;
                    winner = gameSession.UserOne;
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