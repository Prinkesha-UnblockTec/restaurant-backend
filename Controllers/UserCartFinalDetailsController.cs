using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using restaurant.Interfaces;
using restaurant.Models;
using restaurant.Repository;

namespace restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCartFinalDetailsController : Controller
    {
        private readonly IUserCartFinalDetails _userCartFinalDetails;
        private readonly IMapper _mapper;
        public UserCartFinalDetailsController(IUserCartFinalDetails userCartFinalDetails, IMapper mapper)
        {
            _userCartFinalDetails = userCartFinalDetails;
            _mapper = mapper;
        }
        [Route("GetOrdersInAdminData/{Id}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrdersAdmin>))]
        public IActionResult GetOrdersInAdminData(int id)
        {
            var TaskList = _mapper.Map<List<OrdersAdmin>>(_userCartFinalDetails.GetOrdersInAdminData(id));
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("AddedUserCartData")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddNewTask([FromBody] UserCartFinalDetails.CartDetails newList)
        {

            var CreateList = _mapper.Map<UserCartFinalDetails.CartDetails>(newList);
            if (_userCartFinalDetails.AddedUserCartList(CreateList))
            {
                return Ok(new { Message = "Successfully Created", status = 1 });
            }
            return BadRequest("TaskList Already Exist");
        }
              [HttpPost("GetItemDataBseOnUser")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ItemDataBseOnUser>))]
        public IActionResult GetUserCartProductsByTableName([FromBody]  ItemDataBseOnUser user)
        {

            if (user == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var products = _userCartFinalDetails.GetUserCartProductsByTableName(user);
            return Ok(products);
        }
        [HttpGet("GetAllCartItems")]
        public IActionResult GetAllCartItems()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var products = _userCartFinalDetails.GetAllCartItems();
            return Ok(products);
        }
        [Route("UpdateParticularUserStatus")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateRole([FromBody] UpdateStatusUserCart ItemsList)
        {
            if (ItemsList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<UpdateStatusUserCart>(ItemsList);
            if (_userCartFinalDetails.UpdateParticularUserStatus(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Role Name is Already Exist");
        }
        [Route("GetAddressByUsernamePassword")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult GetAddressByUsernamePassword([FromBody] Address request)
        {
            if (request == null || string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Invalid request. Username and password are required.");
            }
            var address = _userCartFinalDetails.GetAddressByUsernamePassword(request.Username, request.Password);

            return Ok(new { Address = address, Message = "Successfully retrieved address.", Status = 1 });
        }
        [Route("GetAddressByOrderId")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult GetAddressByOrderId([FromBody] Address request)
        {
            if (request == null || request.ID == null)
            {
                return BadRequest("Invalid request. Username and password are required.");
            }
            var address = _userCartFinalDetails.GetAddressByOrderId(request.ID);

            return Ok(new { Address = address, Message = "Successfully retrieved address.", Status = 1 });
        }
        [Route("GetStatusUser/{Id}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult GetLoginId(int Id)
        {
            if (Id == null)
            {
                return BadRequest(ModelState);
            }

            var TaskList = _userCartFinalDetails.GetStatusUser(Id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("AddSatausData")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddSatausData([FromBody] StoreSatausData newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var CreateList = _mapper.Map<StoreSatausData>(newList);
            if (_userCartFinalDetails.StoreSatausData(CreateList))
            {
                return Ok(new { Message = "Successfully Created", status = 1 });
            }
            return BadRequest("TaskList Already Exist");
        }
        [Route("GetSatausDataByCartId/{Id}")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StoreSatausData>))]
        public IActionResult GetStoreSatausData(int Id)
        {
            if (Id == null)
            {
                return BadRequest(ModelState);
            }

            var TaskList = _userCartFinalDetails.GetStoreSatausData(Id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("GetLastSelectedOrderType")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(string))]
        public IActionResult GetLastSelectedOrderTypes()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = _userCartFinalDetails.GetLastSelectedOrderType();
            return Ok(new { result });

        }
        [Route("AddedDataBaseCartId")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddedDataBaseCartId([FromBody] CartModel newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);  
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var CreateList = _mapper.Map<CartModel>(newList);
            if (_userCartFinalDetails.AddedDataBaseCartId(CreateList))
            {
                return Ok(new { Message = "Successfully Created", status = 1 });
            }
            return BadRequest("TaskList Already Exist");
        }
        [Route("GetNotificationData")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Notification>))]
        public IActionResult GetRole()
        {
            var TaskList = _mapper.Map<List<Notification>>(_userCartFinalDetails.GetAllNotificationData());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("EditIsReadInNotification")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateRole([FromBody] UpdateRoleNameForNotification RoleList)
        {
            if (RoleList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<UpdateRoleNameForNotification>(RoleList);
            if (_userCartFinalDetails.UpdateNotificationIsRead(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Role Name is Already Exist");
        }
        [Route("EditNotification")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult EditNotification([FromBody] Notification RoleList)
        {
            if (RoleList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<Notification>(RoleList);
            if (_userCartFinalDetails.UpdateNotification(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Role Name is Already Exist");
        }
        [Route("UpdateCheckedItems")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateChefItems([FromBody] UpdateCheckedItems List)
        {
            if (List == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<UpdateCheckedItems>(List);
            if (_userCartFinalDetails.UpdateCheckedItems(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Role Name is Already Exist");
        }
        [HttpDelete("DeleteCompleteOrderforNotification/{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRole(int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            if (!_userCartFinalDetails.DeleteCompleteOrderforNotification(Id))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
            return Ok(new { status = 1 });
        }

    }
}
