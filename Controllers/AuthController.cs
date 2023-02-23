using Microsoft.AspNetCore.Mvc;
using session_storage_to_redis.Attributes;

namespace board_dotnet.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpGet]
        [Authorize(role = "user")]
        public async Task<IActionResult> GetSession()
        {
            await HttpContext.Session.LoadAsync();

            return Ok(HttpContext.Session.GetString("id"));
        }

        [HttpPost]
        public async Task<IActionResult> SetSession(string id)
        {
            HttpContext.Session.SetString("id", id);
            HttpContext.Session.SetString("role", "user");

            await HttpContext.Session.CommitAsync();

            return Ok();
        }

        [HttpDelete]
        [Authorize(role = "user")]
        public async Task<IActionResult> DeleteSession()
        {
            await HttpContext.Session.LoadAsync();

            HttpContext.Session.Clear();

            return NoContent();
        }
    }
}