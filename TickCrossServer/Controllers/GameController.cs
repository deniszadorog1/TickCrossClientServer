using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TickCrossLib.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TickCrossServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        // GET: api/<GameController>
        [HttpGet("IsGameBeenCanceled")]
        public bool IsGameBeenCanceled(int gameId)
        {
            return DBService.IsTempGameIsBeenCanceled(gameId);
        }

        [HttpPost("SetGameCanceledStatus")]
        public void SetGameCanceledStatus([FromBody] GameIdParam gameParam)
        {
            DBService.SetClosedStatusToGame(gameParam.GameId);
        }

        [HttpGet("IsTempGameIsExist")]
        public bool IsTempGameIsExist(int gameId)
        {
            return DBService.IsTempGameIsExist(gameId);
        }

        public class GameIdParam
        {
            public int GameId { get; set; }
        }

    }
}
