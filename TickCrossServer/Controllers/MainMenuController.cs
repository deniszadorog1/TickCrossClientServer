using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class MainMenuController : ControllerBase
    {
        // GET: api/<MainMenuController>
        [HttpGet("GetUserWinsAmount")]
        public int GetUserWinsAmount(int userId)
        {
            return DBService.GetWinsAmount(userId);
        }

        [HttpGet("GetUserLosesAmount")]
        public int GetUserLosesAmount(int userId)
        {
            return DBService.GetLosesAmount(userId);
        }

        [HttpGet("GetUserDrawsAmount")]
        public int GetUserDrawsAmount(int userId)
        {
            return DBService.GetDrawsAmount(userId);
        }


        [HttpGet("GetUserGamesAmount")]
        public int GetUserGamesAmount(int userId)
        {
            return DBService.GetGamesAmount(userId);
        }

        [HttpGet("GetUserReqsSentToUser")]
        public List<GameRequestModel> GetAllUserRequests(int userId)
        {
            List<TickCrossLib.EntityModels.GameRequest> reqs = DBService.GetAllGotUserRequests(userId);

            return GetConvertedGameReqs(reqs);
        }

        [HttpGet("GetGameReqsSentByUser")]
        public List<GameRequestModel> GetGameReqsSentByUser(int userId)
        {
            List<TickCrossLib.EntityModels.GameRequest> reqs = DBService.GetGameRequestSentByUser(userId);

            return GetConvertedGameReqs(reqs);
        }

        private static List<GameRequestModel> GetConvertedGameReqs(List<TickCrossLib.EntityModels.GameRequest> reqs) //++-
        {
            List<GameRequestModel> res = new List<GameRequestModel>();

            for (int i = 0; i < reqs.Count; i++)
            {
/*                string senderLogin = DBService.GetUserLoginById((int)reqs[i].SenderId);
                string receiverLogin = DBService.GetUserLoginById((int)reqs[i].ReciverId);

                char? senderSign = DBService.GetSignById((int)reqs[i].SenderSignId);
                char? receiverSign = DBService.GetSignById((int)reqs[i].ReciverSignId);

                TickCrossLib.Enums.RequestStatus status = DBService.GetGameReqStatusById((int)reqs[i].StatusId);
*/
                res.Add(new GameRequestModel(
                    DBService.GetUserLoginById((int)reqs[i].SenderId),
                    DBService.GetUserLoginById((int)reqs[i].ReciverId),
                    DBService.GetSignById((int)reqs[i].SenderSignId),
                    DBService.GetSignById((int)reqs[i].ReciverSignId),
                    DBService.GetGameReqStatusById((int)reqs[i].StatusId)));
            }
            return res;
        }

        [AllowAnonymous]
        [HttpGet("GetGameRequest")]
        public TickCrossLib.Models.GameRequest GetGameRequest(int receiverId, int senderId)
        {
            TickCrossLib.Models.GameRequest req = DBService.GetGameRequest(receiverId, senderId);
            return req;
        }

        [HttpGet("GetGameHistory")]
        public List<GameResult> GetGameHistory(int userId)
        {
            return DBService.GetGamesWithUser(userId);
        }
    }
}
