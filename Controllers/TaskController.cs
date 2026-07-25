using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SystemDesignTask1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class TaskController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TaskController (IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("")]
        public IActionResult GetRequest()
        {
            var servername = _configuration["ServerName"];
            return Ok($"hello from {servername}");
        }

    }
}
