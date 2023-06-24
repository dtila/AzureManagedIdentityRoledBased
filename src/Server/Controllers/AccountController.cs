using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Server.Controllers
{
    [Authorize(Roles = "Administrators")]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        [HttpGet("status")]
        public AccountStatus Status()
        {
            return new AccountStatus
            {
                Date = DateTime.Now,
                Name = "SecretValueFromServer"
            };
        }
    }
}