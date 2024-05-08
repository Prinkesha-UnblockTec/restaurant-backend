using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using restaurant.Interfaces;
using restaurant.Models;

namespace restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuAceessController : Controller
    {
        private readonly IMenuAceessRepository _menuAceessRepository;
        private readonly IMapper _mapper;
        public MenuAceessController(IMenuAceessRepository menuAceessRepository, IMapper mapper)
        {
            _menuAceessRepository = menuAceessRepository;
            _mapper = mapper;
        }
        [Route("GetAllMenuAccessData")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MenuAccess>))]
        public IActionResult GetAllMenuAccessDatas()
        {
            var TaskList = _mapper.Map<List<MenuAccess>>(_menuAceessRepository.GetAllMenuAccessData());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("SaveMenuAccessData")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddedRoleData([FromBody] List<MenuAccess> newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var CreateList = _mapper.Map<List<MenuAccess>>(newList);
            if (_menuAceessRepository.SaveMenuAccessData(CreateList))
            {
                return Ok(new { Message = "Successfully Created", status = 1 });
            }
            return BadRequest("TaskList Already Exist");
        }

        [Route("GetMenuListByRoleID/{RoleId}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MenuAccess>))]
        public IActionResult GetMenuByRoleId(int RoleId)
        {
            var TaskList = _mapper.Map<List<MenuAccess>>(_menuAceessRepository.GetMenuListByRoleID(RoleId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("GetActiveMenus")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Menu>))]
        public IActionResult GetActiveRole()
        {
            var TaskList = _mapper.Map<List<Menu>>(_menuAceessRepository.GetActiveMenu());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("GetDataBaseOnRoleID/{RoleId}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActiveMenuAccess>))]
        public IActionResult GetDataBaseOnRoleIDs(int RoleId)
        {
            var TaskList = _mapper.Map<List<ActiveMenuAccess>>(_menuAceessRepository.GetDatasBaseOnRoleID(RoleId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
    }
}
