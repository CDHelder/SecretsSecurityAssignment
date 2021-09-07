using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SecretsSecurityAssignment.Core;
using SecretsSecurityAssignment.Core.UserEntities;
using SecretsSecurityAssignment.Service;
using SecretsSecurityAssignment.Service.CustomAttributes;
using SecretsSecurityAssignment.Service.Interfaces;
using SecretsSecurityAssignment.WebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecretsSecurityAssignment.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AuthorizeUser(UserType.Civilian)]
    public class SensitiveSecretController : ControllerBase
    {
        private readonly ISensitiveSecretService service;
        private readonly IMapper mapper;

        public SensitiveSecretController(ISensitiveSecretService service, IMapper mapper)
        {
            this.service = service;
            this.mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = service.GetAll();

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var viewModel = mapper.Map<List<Secret>>(result.Value);
            return Ok(new { all = viewModel });
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var result = service.Get(id);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            var viewModel = mapper.Map<Secret>(result.Value);
            return Ok(new { sensitivesecret = viewModel });
        }

        [HttpPost]
        public IActionResult Post([FromBody] Secret secret)
        {
            var result = service.Create(secret.Content, secret.Name);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok($"Secret with name: {secret.Name} was created is {result.IsSuccess}");
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromBody] Secret secret)
        {
            var sensitiveSecret = mapper.Map<SensitiveSecret>(secret);
            var result = service.Update(sensitiveSecret);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok($"Secret with username: {secret.Name} was updated is {result.IsSuccess}");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = service.Delete(id);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return Ok($"Secret with id: {id} was deleted is {result.IsSuccess}");
        }
    }
}
