using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using restaurant.Interfaces;
using restaurant.Models;
using restaurant.Repository;

namespace restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController  : Controller
    {
        private readonly IMasterRepository _masterRepository;
        private readonly IMapper _mapper;
        public MasterController(IMasterRepository masterRepository, IMapper mapper)
        {
            _masterRepository = masterRepository;
            _mapper = mapper;
        }
        [Route("GetAllMasterData")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Master>))]
        public IActionResult GetAllMasterData()
        {
            var TaskList = _mapper.Map<List<Master>>(_masterRepository.GetAllMasterData());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("AddedMasterList")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddedRole([FromBody] Master newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var CreateList = _mapper.Map<Master>(newList);
            if (_masterRepository.AddedMasterList(CreateList))
            {
                return Ok(new { Message = "Successfully Created", status = 1 });
            }
            return BadRequest("TaskList Already Exist");
        }

        [Route("EditMaster")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult EditMaster([FromBody] Master RoleList)
        {
            if (RoleList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<Master>(RoleList);
            if (_masterRepository.EditMaster(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Master Name is Already Exist");
        }
        [HttpDelete("DeleteMaster/{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteMaster(int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            if (!_masterRepository.DeleteMaster(Id))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
            return Ok(new { status = 1 });
        }
        [Route("GetActiveMaster")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActiveMaster>))]
        public IActionResult GetActiveRole()
        {
            var TaskList = _mapper.Map<List<ActiveMaster>>(_masterRepository.GetActiveMaster());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("GetTotalAmountTableNo")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult GetTotalAmountTableNo()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _masterRepository.GetTotalAmountTableNo();
            return Ok(new { List = result });

        }
        [Route("GetLastRecordByTableNo/{tableNo}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CartIDAndUserName>))] 
        public IActionResult GetLastRecordByTableNo(string tableNo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _masterRepository.GetLastRecordByTableNo(tableNo);
            return Ok(new { List = result });

        }
    }
}
