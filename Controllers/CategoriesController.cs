using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using restaurant.Interfaces;
using restaurant.Models;

namespace restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoriesRepository _categoriesRepository;
        private readonly IMapper _mapper;
        public CategoriesController(ICategoriesRepository roleRepository, IMapper mapper)
        {
            _categoriesRepository = roleRepository;
            _mapper = mapper;
        }
        [Route("GetCategoriesList")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Categories>))]
        public IActionResult GetRole()
        {
            var TaskList = _mapper.Map<List<Categories>>(_categoriesRepository.GetAllCategoriesData());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("GetActiveCategories")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ActiveCategories>))]
        public IActionResult GetActiveActiveCategories()
        {
            var TaskList = _mapper.Map<List<ActiveCategories>>(_categoriesRepository.GetAllActiveCategoriesData());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("AddCategoriestList")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult AddCategory([FromBody] NewCategories newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_categoriesRepository.AddCategoriesList(newList))
            {
                return Ok(new { Message = "Successfully Created", status = 1 });
            }
            return BadRequest("TaskList Already Exist");
        }
        [Route("EditCategories")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult EditCategoriesList([FromBody] NewCategories Categories)
        {
            if (Categories == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var UpdateListData = _mapper.Map<NewCategories>(Categories);
            if (_categoriesRepository.EditCategoriesList(UpdateListData))
            {
                return Ok(new { Message = "Successfully Updated", status = 1 });
            }
            return BadRequest("This Categories Name is Already Exist");
        }
        [HttpDelete("DeleteCategories/{Id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRole(int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();


            if (!_categoriesRepository.DeleteCategories(Id))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }
            return Ok(new { status = 1 });
        }
    }
}