using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TickCrossServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameReqController : ControllerBase
    {
        // GET: api/<GameReqController>
        [HttpGet("GetGameRequest")]
        public TickCrossLib.Models.User GetGameRequester(string login)
        {
            TickCrossLib.Models.User result = null;


            return result;
        }

        // GET api/<GameReqController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<GameReqController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GameReqController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GameReqController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
