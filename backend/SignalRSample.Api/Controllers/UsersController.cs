using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SignalRSample.Api.Services;

namespace SignalRSample.Api.Controllers
{
    [Route("[Controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            // REVIEW: Könnte man auch weglassen, da der Controller über DI aktiviert wird und es dann in der DI schon knallt
            // wenn der UsersService nicht existiert.
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_usersService.GetAllUsers());
        }
    }
}
