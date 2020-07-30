using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SignalRSample.Api.Models;

namespace SignalRSample.Api.Services
{
    public interface IUsersService
    {
        Task<List<User>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task AddUserAsync(string connectionId, string userName);
        Task RemoveUserAsync(string connectionId);
        Task<User> GetUserAsync(string clientId);
        Task<User> GetUserByNameAsync(string name);
    }
}