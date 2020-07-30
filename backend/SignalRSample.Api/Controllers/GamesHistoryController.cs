using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SignalRSample.Api.Extensions;
using SignalRSample.Api.Services;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SignalRSample.Api.Controllers
{
    [Route("[Controller]")]
    [Authorize]
    public class GamesHistoryController : Controller
    {
        private readonly GamesHistoryService _gamesHistoryService;
        private readonly IUsersService _usersService;

        public GamesHistoryController(GamesHistoryService gamesHistoryService, IUsersService usersService)
        {
            _gamesHistoryService = gamesHistoryService;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> LoadGamesHistoryByUserId(CancellationToken cancellationToken)
        {
            var user = await _usersService.GetUserByNameAsync(User.UserName());
            if (user == null)
            {
                return BadRequest("User does not exists");
            }
            var result = await _gamesHistoryService.LoadUserHistoryAsync(user.Id, cancellationToken);
            return Ok(result);
        }
    }
}