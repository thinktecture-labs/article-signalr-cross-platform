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
    // REVIEW: Es wäre hier auch denkbar, dass Du (wie es der IdSrv macht) bspw. eine InMemoryDatabase nutzt.
    // In einem "echten" Spielserver wird die Userliste höchstwahrscheinlich in einer Datenbank stecken.
    // Plus, Du könntest ein IUsersService definieren und dann eben hier den InMemoryUsersService implementieren.
    // Generell würde ich sonst die Methoden hier in der Klasse als Task auslegen und mit async/await arbeiten,
    // da das in einem echten Server eben bedingt durch den Zugriff auf eine weitere Ressource sinnvoller erscheint.
    // CHECK
    public class UsersService : IUsersService
    {
        private readonly UserDbContext _context;

        public UsersService(UserDbContext context)
        {
            _context = context;
        }

        public Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return _context.Users.ToListAsync(cancellationToken);
        }

        public async Task AddUserAsync(string connectionId, string userName)
        {
            if (connectionId == null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }

            // REVIEW: Performancetechnisch würde ich hier auf ein _users.Any(user => user.ConnectionId == connectionId) wechseln.
            // Bei .All müssen alle _users durchlaufen werden, bei .Any wird beim ersten Auffinden einer passenden connectionId sofort abgebrochen. CHECK
            if (!_context.Users.Any(u => u.ConnectionId == connectionId))
            {
                await _context.Users.AddAsync(new User
                {
                    ConnectionId = connectionId,
                    Name = String.IsNullOrWhiteSpace(userName) ? connectionId : userName
                });
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveUserAsync(string connectionId)
        {
            if (connectionId == null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.ConnectionId == connectionId);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}