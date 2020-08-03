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
    public class UsersService : IUsersService
    {
        private readonly GamesDbContext _context;

        public UsersService(GamesDbContext context)
        {
            _context = context;
        }

        public Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken)
        {
            return _context.Users.ToListAsync(cancellationToken);
        }

        public Task<User> GetUserAsync(string clientId)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.ConnectionId == clientId);
        }

        public Task<User> GetUserByNameAsync(string name)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Name == name);
        }

        public Task<User> GetUserBySubjectAsync(string subId)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.UserSubId == subId);
        }

        public async Task AddUserAsync(string connectionId, string subId, string userName)
        {
            if (connectionId == null)
            {
                throw new ArgumentNullException(nameof(connectionId));
            }

            if (!_context.Users.Any(u => u.UserSubId == subId))
            {
                await _context.Users.AddAsync(new User
                {
                    ConnectionId = connectionId,
                    UserSubId = subId,
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