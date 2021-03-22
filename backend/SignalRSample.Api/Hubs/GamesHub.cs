using System;
using SignalRSample.Api.Services;

namespace SignalRSample.Api.Hubs
{
    // Add Authorization
    // Add Hub Base Class
    public class GamesHub
    {
        private readonly IUsersService _usersService;

        public GamesHub(IUsersService usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }
    }
}