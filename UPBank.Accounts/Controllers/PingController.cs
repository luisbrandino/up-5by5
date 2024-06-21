using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UPBank.Accounts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {

        [HttpGet]
        public async Task<string> Get()
        {
            return "Pong";
        }

    }
}
