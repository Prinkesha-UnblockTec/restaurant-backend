using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using restaurant.Interfaces;
using restaurant.Models;
using restaurant.Repository;

namespace restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryAddressController : Controller
    {
        private readonly IDeliveryAddressRepository _dliveryAddressRepository;
        private readonly IMapper _mapper;
        public DeliveryAddressController(IDeliveryAddressRepository deliveryAddressRepository, IMapper mapper)
        {
            _dliveryAddressRepository = deliveryAddressRepository;
            _mapper = mapper;
        }
        [Route("GetDeliveryAddressList/{loginId}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DeliveryAddresses>))]
        public IActionResult GetDeliveryAddresses(int loginId)
        {
            var TaskList = _mapper.Map<List<DeliveryAddresses>>(_dliveryAddressRepository.GetAllDeliveryAddressesData(loginId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("AddedDeliveryAddressList")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddedDeliveryAddresses([FromBody] DeliveryAddresses newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var CreateList = _mapper.Map<DeliveryAddresses>(newList);
            if (_dliveryAddressRepository.AddedDeliveryAddressesList(CreateList))
            {
                return Ok(new { Message = "Successfully Created", status = 1 });
            }
            return BadRequest("TaskList Already Exist");
        }

        [Route("EditDeliveryAddress")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateRole([FromBody] DeliveryAddresses newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<DeliveryAddresses>(newList);
            if (_dliveryAddressRepository.EditDeliveryAddresses(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Delivery Addresses is Already Exist");
        }
        [Route("SetDefaultAddress")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult SetDefaultAddress([FromBody] SetDefult newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<SetDefult>(newList);
            if (_dliveryAddressRepository.SetDefaultAddress(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Delivery Addresses is Already Exist");
        }
        [Route("GetDefaultAddress/{loginId}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<DeliveryAddresses>))]
        public IActionResult GetDataBaseOnRoleIDs(int loginId)
        {
            var TaskList = _mapper.Map<List<DeliveryAddresses>>(_dliveryAddressRepository.GetDefaultAddress(loginId));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [HttpDelete("DeleteDeliveryAddress/{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteDeliveryAddresses(int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            if (!_dliveryAddressRepository.DeleteDeliveryAddresses(Id))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
            return Ok(new { status = 1 });
        }
    }
}
