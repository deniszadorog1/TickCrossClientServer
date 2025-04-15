using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using TickCrossLib.Models;
using TickCrossLib.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TickCrossServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        // GET: api/<LoginController>
        [HttpPost("GetLoggedUser")]
        public IActionResult GetLoggedUser([FromBody] DTOUser userParams)
        {
            //return Ok("User received: " + userParams.UserLogin);

            var user = DBService.GetLoggedUser(userParams.UserLogin, userParams.UserPassword);

            //string? user = null;

            if (user is null)
                return NotFound();

            return Ok(user);
        }

        public class DTOUser
        {
            public string? UserLogin { get; set; }
            public string? UserPassword { get; set; }
        }
    }
}
