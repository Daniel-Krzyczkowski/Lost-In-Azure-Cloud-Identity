using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TMF.Shared.API.Dto;

namespace TMF.Shared.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetGreeting()
        {
            string userDisplayName = User.FindFirst(c => c.Type == "name")?.Value;
            var apiResponse = new ApiResponse
            {
                GreetingFromApi = $"Hello {userDisplayName}!"
            };
            return Ok(apiResponse);
        }
    }
}
