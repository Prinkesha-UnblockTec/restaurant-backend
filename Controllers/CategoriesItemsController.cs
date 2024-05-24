using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using restaurant.Interfaces;
using restaurant.Models;

namespace restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesItemsController : Controller
    {
        private readonly ICategoriesItemsRepository _categoriesItemsRepository;
        private readonly IMapper _mapper;
        public CategoriesItemsController(ICategoriesItemsRepository categoriesItemsRepository, IMapper mapper)
        {
            _categoriesItemsRepository = categoriesItemsRepository;
            _mapper = mapper;
        }
        [Route("GetAllCategoriesItemsData")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoriesItems>))]
        public IActionResult GetRole()
        {
            var TaskList = _mapper.Map<List<CategoriesItems>>(_categoriesItemsRepository.GetAllCategoriesItemsData());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("GetAllActiveCategoriesItemsData")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActiveCategoriesItems>))]
        public IActionResult GetActiveActiveCategories()
        {
            var TaskList = _mapper.Map<List<ActiveCategoriesItems>>(_categoriesItemsRepository.GetAllActiveCategoriesItemsData());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("AddCategoriesItemsList")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddCategory([FromBody] CategoriesItems newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_categoriesItemsRepository.AddCategoriesItemsList(newList))
            {
                return Ok(new { Message = "Successfully Created", status = 1 });
            }
            return BadRequest("TaskList Already Exist");
        }
        [Route("UpdateCalculationItems")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateCalculationItems([FromBody] UpdateCalculation newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_categoriesItemsRepository.UpdateCalculationItems(newList))
            {
                return Ok(new { Message = "Successfully Created", status = 1 });
            }
            return BadRequest("TaskList Already Exist");
        }
        [Route("EditCategoriesItemsList")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult EditCategoriesList([FromBody] CategoriesItems Categories)
        {
            if (Categories == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<CategoriesItems>(Categories);
            if (_categoriesItemsRepository.EditCategoriesItemsList(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Categories Name is Already Exist");
        }
        [Route("EditUpdateBalanceQuantityList")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult EditUpdateBalanceQuantityList([FromBody] List<UpdateBalanceQuantity> Categories)
        {
            if (Categories == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<List<UpdateBalanceQuantity>>(Categories);
            if (_categoriesItemsRepository.EditUpdateBalanceQuantityList(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This UpdateBalance Quantity is Already Exist");
        }
        [HttpDelete("DeleteCategoriesItem/{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRole(int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            if (!_categoriesItemsRepository.DeleteCategoriesItem(Id))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
            return Ok(new { status = 1 });
        }
    }
}