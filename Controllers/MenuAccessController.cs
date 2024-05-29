using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using restaurant.Interfaces;
using restaurant.Models;
using restaurant.Repository;

namespace restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuAccessController : Controller
    {
        private readonly IMenuAccessRepository _menuAccessRepository;
        private readonly IMapper _mapper;
        public MenuAccessController(IMenuAccessRepository menuAccessRepository, IMapper mapper)
        {
            _menuAccessRepository = menuAccessRepository;
            _mapper = mapper;
        }
        [Route("GetAllMenuAccessData")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MonuAccess>))]
        public IActionResult GetAllMenuAccessData()
        {
            var TaskList = _mapper.Map<List<MonuAccess>>(_menuAccessRepository.GetAllMenuAccessData());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }

        [Route("EditMenuAccessDatas")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult EditMenuAccessDatas([FromBody] MonuAccess UpdateList)
        {
            if (UpdateList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<MonuAccess>(UpdateList);
            if (_menuAccessRepository.EditMenuAccessDatas(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Role Name is Already Exist");
        }
    }
}
