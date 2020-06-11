using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SignalRSample.Api.Controllers
{
    [Route("identity")]
    [Authorize]
    public class IdentityController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            // REVIEW: Du nutzt sonst überall die Methodenschreibweise und hier plötzlich "normales" LINQ. Wieso?
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
