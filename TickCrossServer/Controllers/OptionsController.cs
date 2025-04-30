using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TickCrossLib.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TickCrossServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OptionsController : ControllerBase
    {
        // PUT api/<OptionsController>/5
        [HttpPut("ChangeUserParams")]
        public IActionResult Put([FromBody] UserUpdateDto change)
        {
            bool isExist = DBService.IsUserLoginIsExist(change.NewLogin);
            if (isExist) return Conflict("User with such login is exist!");

            DBService.ChangeUserParams(change.OldLogin, change.NewLogin, change.NewPassword);
            return Ok("Updated");
        }

        public class UserUpdateDto()
        {
            public string? OldLogin { get; set; }
            public string? OldPassword { get; set; }

            public string? NewLogin { get; set; }
            public string? NewPassword { get; set; }
        }

    }
}
