using Microsoft.AspNetCore.Mvc;
using SecretsSecurityAssignment.Service;
using SecretsSecurityAssignment.WebApi.ViewModels;
using System;

namespace SecretsSecurityAssignment.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        //TODO: Maak UserController

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(LoginCredentials loginCredentials)
        {
            try
            {
                return Ok(new
                {
                    token = userService.Login(loginCredentials.Username, loginCredentials.Password).Value
                });
            }
            catch(UnauthorizedAccessException uaaex)
            {
                return Problem($"Incorrect: {uaaex.Message}", statusCode: 401);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateUser(CreateCredentials createCredentials)
        {
            //TODO: ?? Captcha toevoegen 
            //TODO: ?? Niet teveel requests per ip address ~ tip: return StatusCode(429);

            try
            {
                var result = userService.Register(createCredentials.Username, createCredentials.Password, createCredentials.UserType);

                if (result.IsFailure)
                {
                    return Conflict(result.Error);
                }

                return Ok("User creation is: " + result.IsSuccess);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
