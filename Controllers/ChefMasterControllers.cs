using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using restaurant.Interfaces;
using restaurant.Models;
using restaurant.Repository;

namespace restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChefMasterControllers  : Controller
    {
        private readonly IChefMaster _chefMaster;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public ChefMasterControllers(IChefMaster chefMaster, IMapper mapper, IConfiguration configuration)
        {
            _chefMaster = chefMaster;
            _mapper = mapper;
            _configuration = configuration;
        }
        [Route("AddedChefList")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddedRole([FromBody] ChefMaster newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var CreateList = _mapper.Map<ChefMaster>(newList);
            if (_chefMaster.AddedChefMasterList(CreateList))
            {
                return Ok(new { Message = "Successfully Created", status = 1 });
            }
            return BadRequest("TaskList Already Exist");
        }
        [Route("EditChef")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateRole([FromBody] ChefMaster newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<ChefMaster>(newList);
            if (_chefMaster.EditChefMaster(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Chef Name is Already Exist");
        }
        [Route("GetChefList")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ChefMaster>))]
        public IActionResult GetRole()
        {
            var TaskList = _mapper.Map<List<ChefMaster>>(_chefMaster.GetAllChefMasterData());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [HttpDelete("DeleteChef/{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRole(int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            if (!_chefMaster.DeleteChefMaster(Id))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
            return Ok(new { status = 1 });
        }
    }
}
