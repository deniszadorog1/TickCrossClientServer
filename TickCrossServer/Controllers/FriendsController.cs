using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using TickCrossLib.Models;
using TickCrossLib.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TickCrossServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        // GET: api/<FriendsController>
        [HttpGet("GetFriends")]
        public IEnumerable<TickCrossLib.Models.User> GetFriends(string login)
        {
            return DBService.GetAllUserFriends(login);
        }

        // GET: api/<FriendsController>
        [HttpGet("GetUserToAddInFriends")]
        public IEnumerable<TickCrossLib.Models.User> GetUserToAddInFriends(string login)
        {
            return DBService.GetUsersThatNotFriend(login);
        }

        // POST api/<FriendsController>
        [HttpPost("AddFriend")]
        public void AddFriend([FromBody] DtoUser toAddFriend)
        {
            DBService.AddFriend(toAddFriend.UserLogin, toAddFriend.ToAddLogin);
        }

        // DELETE api/<FriendsController>/5
        [HttpDelete("RemoveFriend")]
        public IActionResult RemoveFriend([FromBody] DtoUser toRemove)
        {
            if (string.IsNullOrEmpty(toRemove.UserLogin) || string.IsNullOrEmpty(toRemove.ToAddLogin))
                return BadRequest("Logins are empty");

            bool result = DBService.RemoveFriend(toRemove.UserLogin, toRemove.ToAddLogin);

            return result ? Ok("Removes") : NotFound("Something went wrong!");
        }

        public class DtoUser()
        {
            public string? UserLogin { get; set; }
            public string? ToAddLogin { get; set; }
        };
    }
}
