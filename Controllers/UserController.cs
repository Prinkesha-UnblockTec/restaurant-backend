using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurant.Interfaces;
using restaurant.Models;

namespace restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }
        [Route("GetUserList")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public IActionResult GetUser()
        {
            var TaskList = _mapper.Map<List<User>>(_userRepository.GetAllUserData());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("AddedUsertList")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddedUser([FromBody] User newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var CreateList = _mapper.Map<User>(newList);
            if (_userRepository.AddedUserList(CreateList))
            {
                return Ok(new { Message = "Successfully Created", status = 1 });
            }
            return BadRequest("TaskList Already Exist");
        }

        [Route("EditUser")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateUser([FromBody] User UpdateList)
        {
            if (UpdateList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<User>(UpdateList);
            if (_userRepository.EditUser(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Role Name is Already Exist");
        }
        
            [Route("EditChangePassword")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult EditChangePassword([FromBody] changePasswordforusercrud UpdateList)
        {
            if (UpdateList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<changePasswordforusercrud>(UpdateList);
            if (_userRepository.EditChangePassword(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Role Name is Already Exist");
        }
        [HttpDelete("DeleteUser/{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteUser(int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            if (!_userRepository.DeleteUser(Id))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
            return Ok(new { status = 1 });
        }
    }
}
