using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using restaurant.Interfaces;
using restaurant.Models;
using restaurant.Repository;

namespace restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public AccountController(IAccountRepository accountRepository, IMapper mapper, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
        [Route("Login")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult UserLogin([FromBody] Login user)
        {
            if (user == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int isValidUser = _accountRepository.Login(user);
                var token = new TokenGenerationRequest
                {
                    UserName = user.UserName,
                    Password = user.Password
                };
                var result = new JWTService(_configuration).GenerateToken(token);
                return Ok(new { Token = result, User = user , isValidUser });
        }
        [Route("GetIdBaseOnUserandPassword")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult GetLoginId([FromBody] Login user)
        {
            if (user == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result =  _accountRepository.GetLoginIdByUsernameAndPasswordAsync(user.UserName, user.Password);
            return Ok(new { result });
        }
        [HttpPost("UpdateLogin")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult UpdateLogin([FromBody] loginwithallDetails model)
        {
            if (model == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _accountRepository.UpdateLogin(model);
            return Ok("Login information updated successfully.");

        }
    }
}