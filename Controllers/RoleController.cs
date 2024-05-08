using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurant.Interfaces;
using restaurant.Models;

namespace restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public RoleController(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }
        [Route("GetRoleList")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Role>))]
        public IActionResult GetRole()
        {
            var TaskList = _mapper.Map<List<Role>>(_roleRepository.GetAllRoleData());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("AddedRoletList")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddedRole([FromBody] Role newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var CreateList = _mapper.Map<Role>(newList);
            if (_roleRepository.AddedRoleList(CreateList))
            {
                return Ok(new { Message = "Successfully Created", status = 1 });
            }
            return BadRequest("TaskList Already Exist");
        }

        [Route("EditRole")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateRole([FromBody] Role RoleList)
        {
            if (RoleList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<Role>(RoleList);
            if (_roleRepository.EditRole(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Role Name is Already Exist");
        }
        [HttpDelete("DeleteRole/{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRole(int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            if (!_roleRepository.DeleteRole(Id))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
            return Ok(new { status = 1 });
        }
        [Route("GetActiveRole")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActiveRole>))]
        public IActionResult GetActiveRole()
        {
            var TaskList = _mapper.Map<List<ActiveRole>>(_roleRepository.GetActiveRole());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
    }
}
