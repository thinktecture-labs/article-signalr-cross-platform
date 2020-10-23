using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalRSample.Api.Extensions;
using SignalRSample.Api.Models;
using SignalRSample.Api.Services;

namespace SignalRSample.Api.Controllers
{
    [Authorize(Roles = "ProUser")]
    [ApiController]
    [Route("[Controller]")]
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
            var user = await _usersService.GetUserBySubjectAsync(User.SubId());
            if (user == null)
            {
                return Ok(new List<GameHistoryEntryDto>());
            }

            var result = await _gamesHistoryService.LoadUserHistoryAsync(user.Id, cancellationToken);
            return Ok(result);
        }
    }
}