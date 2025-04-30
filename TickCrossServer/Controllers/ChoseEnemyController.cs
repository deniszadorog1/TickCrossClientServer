using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TickCrossLib.Models;
using TickCrossLib.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860


namespace TickCrossServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChoseEnemyController : ControllerBase
    {
        // GET: api/<ChoseEnemyController>
        [HttpGet("GetPossibleEnemies")]
        public IEnumerable<User> Get(string login)
        {
            var enemies = DBService.GetUserEnemies(login);
            return enemies;
        }

        // GET api/<ChoseEnemyController>/5
        [HttpGet("GetEnemy")]
        public User GetEnemy(string login)
        {
            return DBService.GetUserModelByLogin(login);
        }

        [HttpGet("GetAllSigns")]
        public List<char> GetAllSigns()
        {
            return DBService.GetAllSigns();
        }

    }
}
