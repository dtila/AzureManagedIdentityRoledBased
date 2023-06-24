using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly ServerClient _client;

        public AccountController(ILogger<AccountController> logger, ServerClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        [HttpGet("RetrieveStatusFromRemoteServer")]
        public Task<string> Get() => _client.GetStatus();
    }
}