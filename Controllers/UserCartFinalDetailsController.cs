﻿using AutoMapper;
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
        [Route("AddedUserCartData")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddNewTask([FromBody] UserCartFinalDetails.CartDetails newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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

            string address = _userCartFinalDetails.GetAddressByUsernamePassword(request.Username, request.Password);

            if (string.IsNullOrEmpty(address))
            {
                return NotFound("Address not found for the provided username and password.");
            }

            return Ok(new { Address = address, Message = "Successfully retrieved address.", Status = 1 });
        }
    }
}
