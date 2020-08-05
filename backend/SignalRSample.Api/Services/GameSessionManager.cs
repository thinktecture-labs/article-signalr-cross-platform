using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalRSample.Api.Database;
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
        private readonly GamesDbContext _context;
        private readonly GamesHistoryService _historyService;

        public GameSessionManager(IHubContext<GamesHub> hubContext, GamesDbContext context,
            GamesHistoryService historyService)
        {
            _hubContext = hubContext;
            _context = context;
            _historyService = historyService;
        }

        // TODO: Split in two Methods
        public async Task AddUserAsync(User client)
        {
            // await using var tx = await _context.Database.BeginTransactionAsync();
            var session = await _context.Sessions
                .Include(s => s.UserOne)
                .Include(s => s.UserTwo)
                .Where(s => s.UserTwo == null && s.UserOne != null)
                .FirstOrDefaultAsync(s => s.UserOne.Id != client.Id);
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
                    SessionId = $"Game{_context.Sessions.Count()}",
                    UserOne = client
                };
                await _context.Sessions.AddAsync(session);
            }

            await _context.SaveChangesAsync();
            // await tx.CommitAsync();

            await _hubContext.Groups.AddToGroupAsync(client.ConnectionId, session.SessionId);
            if (!String.IsNullOrWhiteSpace(session.ActiveUser))
            {
                Console.WriteLine(
                    $"New Game will start now. Session: {session.SessionId}, User One: {session.UserOne}, User Two: {session.UserTwo}, Active User: {session.ActiveUser}");
                await _hubContext.Clients.Group(session.SessionId).SendAsync("StartGame", session);
            }
        }

        public async Task RemoveUserAsync(Guid userId)
        {
            // await using var tx = await _context.Database.BeginTransactionAsync();
            var session = await _context.Sessions.FirstOrDefaultAsync(s =>
                s.UserOne.Id == userId || s.UserTwo.Id == userId);
            if (session != null)
            {
                Console.WriteLine($"Remove user from Group. Session: {session.SessionId}, User: {userId}");
                await _hubContext.Clients.Group(session.SessionId).SendAsync("GameOver", "Lost");
                var user = session.UserOne.Id == userId ? session.UserOne : session.UserTwo;
                await _hubContext.Clients.Group(session.SessionId).SendAsync("UserDisconnected", user);
                await _hubContext.Groups.RemoveFromGroupAsync(session.UserTwo.ConnectionId, session.SessionId);
                await _hubContext.Groups.RemoveFromGroupAsync(session.UserOne.ConnectionId, session.SessionId);
                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();
            }

            // await tx.CommitAsync();
        }

        public async Task PlayRoundAsync(string clientId, int value)
        {
            // await using var tx = await _context.Database.BeginTransactionAsync();
            var session = await _context.Sessions
                .Include(s => s.UserOne)
                .Include(s => s.UserTwo)
                .Include(s => s.Moves)
                .FirstOrDefaultAsync(s =>
                    (s.UserOne.ConnectionId == clientId || s.UserTwo.ConnectionId == clientId) &&
                    s.ActiveUser == clientId);
            if (session != null)
            {
                session?.Moves.Add(new GameSessionMove {SessionId = session.Id, ClientId = clientId, Move = value});
                if (CheckSessionState(session, out var winner))
                {
                    await _historyService.AddGameAsync(session, winner);
                    await _hubContext.Clients.Group(session.SessionId).SendAsync("GameOver", winner);
                    await _hubContext.Groups.RemoveFromGroupAsync(session.UserOne.ConnectionId, session.SessionId);
                    await _hubContext.Groups.RemoveFromGroupAsync(session.UserTwo.ConnectionId, session.SessionId);
                    _context.Sessions.Remove(session);
                }
                else
                {
                    session.ActiveUser = session.UserOne.ConnectionId == clientId
                        ? session.UserTwo.ConnectionId
                        : session.UserOne.ConnectionId;
                    await _hubContext.Clients.GroupExcept(session.SessionId, clientId).SendAsync("Play", value);
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                await _hubContext.Clients.User(clientId).SendAsync("NotAllowed");
            }

            // await tx.CommitAsync();
        }

        private bool CheckSessionState(GameSession gameSession, out string winner)
        {
            winner = String.Empty;
            var firstUser = gameSession.Moves.Where(m => m.ClientId == gameSession.UserOne.ConnectionId)
                .Select(m => m.Move).ToArray();
            var secondUser = gameSession.Moves.Where(m => m.ClientId == gameSession.UserTwo.ConnectionId)
                .Select(m => m.Move).ToArray();

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