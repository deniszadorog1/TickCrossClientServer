using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TickCrossServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainMenuController : ControllerBase
    {
        // GET: api/<MainMenuController>
        [HttpGet("GetAllPlayers")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<MainMenuController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<MainMenuController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<MainMenuController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<MainMenuController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
