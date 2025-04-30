using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("GetUserGamesAmount")]
        public int GetUserGamesAmount(int userId)
        {
            return DBService.GetGamesAmount(userId);
        }
    }
}
