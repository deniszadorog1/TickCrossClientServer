using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using TickCrossLib.Enums;
using TickCrossLib.Models;
using TickCrossLib.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TickCrossServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        // GET: api/<LoginController>
        [AllowAnonymous]
        [HttpPost("GetLoggedUser")]
        public TickCrossLib.Models.User? GetLoggedUser([FromBody] DTOUser userParams)
        {
            //if (!RegexService.LoginValidation(userParams.UserLogin)) return null;

            var user = DBService.GetLoggedUser(userParams.UserLogin, userParams.UserPassword);

            return user;
        }

        public class DTOUser
        {
            public string? UserLogin { get; set; }
            public string? UserPassword { get; set; }
        }
        [Authorize]
        [HttpDelete("RemoveClosedGames")]
        public void RemoveClosedGames([FromBody] ToRemoveGamesPram userId)
        {
            DBService.RemoveClosedGames(userId.UserId);
        }
        public class ToRemoveGamesPram()
        {
            public int UserId { get; set; }
        };

        [HttpPost("SetUserLoginStatus")]
        public void SetUserLoginStatus([FromBody] UserLoginStatus status)
        {
            DBService.SetUserLoginStatus(status.UserId, status.Stat);
        }

        public class UserLoginStatus()
        {
            public int UserId { get; set; }
            public UserStat Stat { get; set; }
        };


        [HttpGet("IsUserLogged")]
        public bool IsUserLogged(int userId)
        {
            return DBService.IsUserLogged(userId);
        }

    }
}
