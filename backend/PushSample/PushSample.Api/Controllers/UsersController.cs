using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PushSample.Api.Services;

namespace PushSample.Api.Controllers
{
    [Route("[Controller]")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UsersService _usersService;

        public UsersController(UsersService usersService)
        {
            _usersService = usersService ?? throw new ArgumentNullException(nameof(usersService));
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(_usersService.GetAllUsers());
        }
    }
}