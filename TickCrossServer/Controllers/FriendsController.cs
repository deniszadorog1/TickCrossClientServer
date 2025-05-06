using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Runtime.CompilerServices;
using TickCrossLib.EntityModels;
using TickCrossLib.Models;
using TickCrossLib.Models.NonePlayable;
using TickCrossLib.Services;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TickCrossServer.Controllers
{
    [Authorize]
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

        [HttpGet("GetFriendsLogins")]
        public List<string> GetFriendsLogins(int userId)
        {
            return DBService.GetFriendLogins(userId);
        }

        [HttpGet("GetUserLoginsToAddInFriends")]
        public List<string> GetUserLoginsToAddInFriends(int userId)
        {
            return DBService.GetUserLoginsToAddInFriends(userId);
        }

        //user - sender; 
        [HttpPost("AddFriendOffer")]
        public void AddFriendOffer([FromBody] FriendOfferDTO offer)
        {
            int receiverId = DBService.GetUserByLogin(offer.ReceiverLogin).Id;
            DBService.AddFriendOffer(offer.UserId, receiverId);
        }
        public class FriendOfferDTO
        {
            public int UserId { get; set; }
            public string ReceiverLogin { get; set; }
        };

        [HttpGet("GetReqsSentByUser")]
        public List<FriendRequestModel> GetReqsSentByUser(int userId)
        {
            return DBService.GetFriendReqsSentByUser(userId);
        }

        [HttpGet("GetReqsSentToUser")]
        public List<FriendRequestModel> GetReqsSentToUser(int userId)
        {
            return DBService.GetFriendReqsSentToUser(userId);
        }

        // POST api/<FriendsController>
        [HttpPost("AddFriend")]
        public void AddFriend([FromBody] DtoUser toAddFriend)
        {
            DBService.AddFriend(toAddFriend.UserLogin, toAddFriend.ToAddLogin);
        }

        [HttpDelete("RemoveReqBySenderLogin")]
        public void RemoveReqBySenderLogin([FromBody] RemoveReqWithSenderLogin model)
        {
            DBService.RemoveFriendOfferBySenderLogin(model.ReceiverId, model.SenderLogin);
        }
        public class RemoveReqWithSenderLogin
        {
            public int ReceiverId { get; set; }
            public string SenderLogin { get; set; }
        }

        [HttpDelete("RemoveReqByReceiverLogin")]
        public void RemoveReqByReceiverLogin([FromBody] RemoveReqWithReceiverLogin model)
        {
            DBService.RemoveFriendOfferByReceiverLogin(model.SenderId, model.ReceiverLogin);
        }
        public class RemoveReqWithReceiverLogin
        {
            public int SenderId { get; set; }
            public string ReceiverLogin { get; set; }
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
