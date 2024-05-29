using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
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
        [Route("Register")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult UserRegister([FromBody] Login user)
        {
            if (user == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var CreateList = _mapper.Map<Login>(user);
            if (_accountRepository.AddedRegisterList(CreateList))
            {
                return Ok(new { Message = "Successfully Created", status = 1 });
            }
            return BadRequest("TaskList Already Exist");
            //int isValidUser = _accountRepository.Login(user);
            //    var token = new TokenGenerationRequest
            //    {
            //        UserName = user.UserName,
            //        Password = user.Password
            //    };
            //    var result = new JWTService(_configuration).GenerateToken(token);
        }
        [Route("Login")]
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult UserLogin([FromBody] Login user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginResult = _accountRepository.UserLogin(user);
            if (loginResult.IsValidUser == false)
            {
                return BadRequest("Invalid username or password.");
            }
            var token = new TokenGenerationRequest
            {
                UserName = user.UserName,
                Password = user.Password
            };

            var result = new JWTService(_configuration).GenerateToken(token);

            return Ok(new
            {
                token = result,
                User = user.UserName,
                isUserOrNot = loginResult.IsValidUser,
                UserId = loginResult.NewUserId,
                status = 1
            });
        }

        [Route("GetLoginUserDataBaseOnID/{Id}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetLoginId(int Id)
        {
            if (Id == null)
            {
                return BadRequest(ModelState);
            }

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}
            //var result =  _accountRepository.GetLoginUserDataBaseOnID(Id);
            //return Ok(new { result });


            var TaskList = _mapper.Map<List<User>>(_accountRepository.GetLoginUserDataBaseOnID(Id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
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
        [HttpPost("UpdatCurrency")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult UpdateCurrecyAdmin([FromBody] UpdateCurrency model)
        {
            if (model == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _accountRepository.UpdateCurrecyAdmin(model);
            return Ok(new { message = "updated successfully." });

        }
        [Route("GetCurrency")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult GetCurrecyAdmin ()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _accountRepository.GetCurrecyAdmin();
            return Ok(new { result });

        }
        [HttpPost("UpdatDefaultRouting")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        public IActionResult UpdateCurrecyAdmin([FromBody] UpdateRouting model)
        {
            if (model == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _accountRepository.SetDefaultRouting(model);
            return Ok(new {message = "updated successfully." });

        }
        [Route("GetDefaultRouting")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UpdateRouting>))]
        public IActionResult GetRole()
        {
            var TaskList = _mapper.Map<List<UpdateRouting>>(_accountRepository.GetDefaultRouting());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
    }
}






//CREATE TABLE StatusLog (
//    LogId INT IDENTITY(1,1) PRIMARY KEY,
//    CartId INT NOT NULL,
//    Status NVARCHAR(50) NOT NULL,
//    ChangeTime DATETIME NOT NULL,
//    FOREIGN KEY (CartId) REFERENCES cart_final_deatils(ID)
//);
