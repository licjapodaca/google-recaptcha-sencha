using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace backend_recaptcha.Controllers
{
	[ApiController]
    [Route("subscription")]
    public class SubscriptionController : ControllerBase
    {
        private readonly IConfiguration _config;

        public SubscriptionController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterSubscription()
        {
            return await Task.Run<IActionResult>(() =>
			{
				return Ok(new { success = true, message = "Test" });
			});
        }
    }
}
