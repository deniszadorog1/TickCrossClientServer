using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using TickCrossLib.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TickCrossServer.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        // GET: api/<RegistrationController_cs>
        [HttpPost("CheckLogin")]
        public IActionResult Get([FromBody] TempDTO login)
        {
            bool result = TickCrossLib.Services.DBService.IsUserLoginIsExist(login.SimpleReq);
            return Ok(result ? "1" : "0");
        }

        public class TempDTO
        {
            public string? SimpleReq { get; set; }
        }

        // POST api/<RegistrationController_cs>
        [HttpPost("AddUser")]
        public IActionResult Post([FromBody] UserRegistrationModel model)
        {
            if (!RegexService.RegistrationPasswordValid(model.Password) ||
                !RegexService.RegistrationPasswordValid(model.Login)) return NoContent();


            DBService.AddNewUser(model.Login, model.Password);
            return Ok();
        }

        public class UserRegistrationModel
        {
            public string? Login { get; set; }
            public string? Password { get; set; }
        }


    }
}
