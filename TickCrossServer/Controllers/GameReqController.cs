using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Runtime;
using System.Runtime.CompilerServices;
using TickCrossLib.EntityModels;
using TickCrossLib.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TickCrossServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameReqController : ControllerBase
    {
        // GET: api/<GameReqController>
        [HttpGet("GetGameRequest")]
        public TickCrossLib.Models.GameRequest GetGameRequester(string login)
        {
            TickCrossLib.Models.GameRequest result = DBService.GetFirstUnwatchedUserRequest(login);
            return result;
        }

        [HttpPost("AddGame")]
        public void AddGame([FromBody] GameParam game)
        {
            DBService.AddGame(game.Game);
        }
        public class GameParam
        {
            public TickCrossLib.Models.Game Game { get; set; }
        }


        [HttpPost("AddGameRequest")]
        public IActionResult AddGameRequest([FromBody] RequestParams addReq)
        {
            int? enemySignIndex = DBService.GetSignId(addReq.EnemySign);
            int? userSignIndex = DBService.GetSignId(addReq.UserSign);

            if (enemySignIndex is null || userSignIndex is null) return NotFound("How it it?");

            int reqStatId = (int)DBService.GetRequestStatusId(addReq.Status.ToString());


            bool isAdded = DBService.AddGameRequest(addReq.SenderLogin, addReq.ReceiverLogin,
                (int)enemySignIndex, (int)userSignIndex, reqStatId);

            return isAdded ? Ok() : NotFound();
        }
        public class RequestParams()
        {
            public string SenderLogin { get; set; }
            public string ReceiverLogin { get; set; }
            public char EnemySign { get; set; }
            public char UserSign { get; set; }
            public TickCrossLib.Enums.RequestStatus Status { get; set; }
        }

        // GET api/<GameReqController>/5
        [HttpGet("IsUserIsLoggedIn")]
        public bool IsUserIsLogged(string login)
        {
            return DBService.IsUserIsLogged(login);
        }

        [HttpGet("GetUserByLogin")]
        public TickCrossLib.Models.User GetUserByLogin(string login)
        {
            return DBService.GetUserByLogin(login);
        }

        [HttpGet("GetUserById")]
        public TickCrossLib.Models.User GetUserById(int id)
        {
            return DBService.GetUserById(id);
        }

        [HttpGet("IsUserIsLoggedInById")]
        public bool IsUserLoggedById(int id)
        {
            return DBService.IsUserIsLoggedById(id);
        }

        // POST api/<GameReqController>
        [HttpPost("AddNewTempGame")]
        public void AddTempGame([FromBody] TempGame newTempGame)
        {
            DBService.AddTempGame(newTempGame.GameId);
        }
        public class TempGame()
        {
            public int GameId { get; set; }
        }

        [HttpGet("GetCordToMove")]
        public TickCrossLib.Models.HelpModels.Cord GetCordToMove(int gameId)
        {
            (int?, int?) res = DBService.GetMoveCordToMake(gameId);

            return new TickCrossLib.Models.HelpModels.Cord { X = res.Item1, Y = res.Item2 };

            /*            return (res.Item1 is null ? -1 : (int)res.Item1,
                            res.Item2 is null ? -1 : (int)res.Item2);*/
        }

        [HttpPost("SetTempGameStatus")]
        public void SetTempGameStatus([FromBody] StatusParam status)
        {
            DBService.SetTempGameStatus(status.Status, status.GameId);
        }
        public class StatusParam()
        {
            public string Status { get; set; }
            public int GameId { get; set; }
        }

        [HttpPost("ChangeTempGameMoveCord")]
        public void ChangeTempGameMoveCord([FromBody] UpdateTempGameMove newMove)
        {
            DBService.SetTempGameMoveCord((newMove.Cord.X, newMove.Cord.Y), newMove.GameId);
        }
        public class UpdateTempGameMove()
        {
            public Point Cord { get; set; }
            public int GameId { get; set; }
        }
        public class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
        }



        [HttpGet("GetTempGameStatus")]
        public string GetTempGameStatus(int gameId)
        {
            return DBService.GetTempGameStatus(gameId);
        }

        [HttpPost("SetRequestStatus")]
        public void SetRequestStatus([FromBody] NewReqStatusParams request)
        {
            int? statusId = DBService.GetRequestStatusId(request.Status.ToString());
            if (statusId is null) return;

            DBService.SetRequestStatus(request.Request.Id, (int)statusId);
        }
        public class NewReqStatusParams
        {
            public TickCrossLib.Models.GameRequest Request { get; set; }
            public TickCrossLib.Enums.RequestStatus Status { get; set; }
        };

        [HttpGet("GetRequestStatus")]
        public string GetRequestStatus(int reqId)
        {
            return DBService.GetRequestsStatus(reqId);
        }

        [HttpGet("GetLastGameId")]
        public int? GetLastGameId()
        {
            return DBService.GetLastGameId();
        }

        [HttpPost("SetStepperInTempGame")]
        public void SetStepperIdInTempGame([FromBody] SetStepperTempGame newStepperParam)
        {
            DBService.SetStepperInTempGame(newStepperParam.GameId, newStepperParam.NewStepperId);
        }
        public class SetStepperTempGame()
        {
            public int GameId { get; set; }
            public int NewStepperId { get; set; }
        }

        [HttpGet("GetTempGameStepperId")]
        public int? GetTempGameStepperId(int gameId)
        {
            return DBService.GetStepperIdInTempGame(gameId);
        }

        [HttpPost("SetGameResult")]
        public void SetGameResult([FromBody] SetGameResParams gameRes)
        {
            DBService.SetGameResult(gameRes.GameId, gameRes.WinnerId, gameRes.IsDraw);
        }
        public class SetGameResParams()
        {
            public int GameId { get; set; }
            public int? WinnerId { get; set; }
            public bool? IsDraw { get; set; }
        }

        [HttpDelete("RemoveRequests")]
        public void RemoveFriend([FromBody] UserParam userId)
        {
            DBService.RemoveUserRequests(userId.UserId);
        }
        public class UserParam()
        {
            public int UserId { get; set; }
        };

        [HttpDelete("RemoveTempGame")]
        public void RemoveFriend([FromBody] TempGameId toRemove)
        {
            DBService.RemoveTempGame(toRemove.GameId);
        }

        public class TempGameId()
        {
            public int GameId { get; set; }
        }
    }
}
