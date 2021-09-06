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
        [Route("Block/{id}")]
        public IActionResult BlockUser(int id)
        {
            try
            {
                var result = userService.Block(id);

                if (result.IsFailure)
                {
                    return BadRequest(result.Error);
                }

                return Ok($"Blocking user with id: {id} was {result.IsSuccess}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Block/{username}")]
        public IActionResult BlockUser(string username)
        {
            try
            {
                var result = userService.Block(username);

                if (result.IsFailure)
                {
                    return BadRequest(result.Error);
                }

                return Ok($"Blocking user with username: {username} was {result.IsSuccess}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Unblock/{id}")]
        public IActionResult UnblockUser(int id)
        {
            try
            {
                var result = userService.Unblock(id);

                if (result.IsFailure)
                {
                    return BadRequest(result.Error);
                }

                return Ok($"Unblocking user with id: {id} was {result.IsSuccess}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("Unblock/{username}")]
        public IActionResult UnblockUser(string username)
        {
            try
            {
                var result = userService.Unblock(username);

                if (result.IsFailure)
                {
                    return BadRequest(result.Error);
                }

                return Ok($"Unblocking user with username: {username} was {result.IsSuccess}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
