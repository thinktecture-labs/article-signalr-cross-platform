using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalRSample.Api.Services;

namespace SignalRSample.Api.Controllers
{
    [Route("[Controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var result = await _usersService.GetAllUsersAsync(cancellationToken);
            return Ok(result);
        }
    }
}