using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TickCrossLib.Services;
using TickCrossServer.Timers;

namespace TickCrossServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimerController : Controller
    {
        private int _moveTime = JsonService.GetNumByName("TimeToMakeMove"); //++-

        [HttpPost("StartTimer")]
        public IActionResult StartTurn([FromBody] GameTimeParam game)
        {
            TimerStorage.StartTimerForGame(game.GameId, TimeSpan.FromSeconds(_moveTime));
            return Ok("Started!");
        }

        public class GameTimeParam()
        {
            public int GameId { get; set; }
        }

        [HttpGet("GetTimeLeft")]
        public IActionResult GetTimeLeft(int gameId)
        {
            var timeLeft = TimerStorage.GetTimeLeft(gameId);
            if (timeLeft == null) return NotFound("Not Found");

            return Ok(timeLeft.Value.TotalSeconds);
        }
    }
}
