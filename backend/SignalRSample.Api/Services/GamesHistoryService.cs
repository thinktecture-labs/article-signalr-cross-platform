using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SignalRSample.Api.Database;
using SignalRSample.Api.Models;

namespace SignalRSample.Api.Services
{
    public class GamesHistoryService
    {
        private readonly GamesDbContext _context;

        public GamesHistoryService(GamesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GameHistoryEntryDto>> LoadUserHistoryAsync(Guid userId,
            CancellationToken cancellationToken)
        {
            var games = await _context.Games
                .Include(g => g.UserOne)
                .Include(g => g.UserTwo)
                .Where(g => g.UserOne.Id == userId || g.UserTwo.Id == userId)
                .OrderBy(game => game.SessionDate)
                .ToListAsync(cancellationToken: cancellationToken);

            return games.Select(game => ConvertGameSessionToHistoryEntryDto(userId, game));
        }

        public async Task AddGameAsync(GameSession session, string winner)
        {
            await _context.Games.AddAsync(new Game
            {
                Id = Guid.NewGuid(),
                SessionDate = DateTime.Now,
                UserOne = session.UserOne,
                UserTwo = session.UserTwo,
                Winner = winner
            });
        }

        private GameHistoryEntryDto ConvertGameSessionToHistoryEntryDto(Guid userId, Game game)
        {
            string gameResult;
            var winGame = (game.UserOne.ConnectionId == game.Winner && game.UserOne.Id == userId) ||
                          (game.UserOne.ConnectionId == game.Winner && game.UserOne.Id == userId);
            if (game.Winner == "Tie")
            {
                gameResult = game.Winner;
            }
            else
            {
                gameResult = winGame ? "Win" : "Loose";
            }

            return new GameHistoryEntryDto
            {
                Id = game.Id,
                Opponent = game.UserOne.Id == userId ? game.UserTwo.Name : game.UserOne.Name,
                GameResult = gameResult,
                SessionDate = game.SessionDate
            };
        }
    }
}