using Microsoft.AspNetCore.Mvc;
using SecretsSecurityAssignment.Core.UserEntities;
using SecretsSecurityAssignment.Service;

namespace SecretsSecurityAssignment.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        //TODO: Maak UserController

        //TODO: Per type secret een controller??

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string username, string password)
        {
            return Ok();
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateUser(string username, string password, UserType userType)
        {
            return Ok();
        }

        [HttpPost]
        [Route("Block")]
        public IActionResult BlockUser(string username = null, int id = 0)
        {
            return Ok();
        }

        [HttpPost]
        [Route("Unblock")]
        public IActionResult UnblockUser(string username = null, int id = 0)
        {
            return Ok();
        }
    }
}
