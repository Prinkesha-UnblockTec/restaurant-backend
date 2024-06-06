using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using restaurant.Interfaces;
using restaurant.Models;
using restaurant.Repository;

namespace restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMapper _mapper;
        public DashboardController(IDashboardRepository dashboardRepository, IMapper mapper)
        {
            _dashboardRepository = dashboardRepository;
            _mapper = mapper;
        }
        [Route("GetStatusWiseOrderCount")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<StausWiseShowOrder>))]
        public IActionResult StausWiseShowOrder()
        {
            var TaskList =_dashboardRepository.GetStatusOrderDetails();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("GetTotalQuaAndPriceInItemRecord")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TotalItemRecord>))]
        public IActionResult TotalItemRecord()
        {
            var TaskList = _mapper.Map<List<TotalItemRecord>>(_dashboardRepository.GetTotalItemRecord());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("GetTotalCategorywithItemSale")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<TotalCategorywithItemSale>))]
        public IActionResult TotalCategorywithItemSale()
        {
            var TaskList = _mapper.Map<List<TotalCategorywithItemSale>>(_dashboardRepository.GetTotalCategorywithItemSale());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new { List = TaskList, status = 1 });
        }
        [Route("OrderSummary")]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<OrderSummary>))]
        public IActionResult OrderSummary([FromQuery] int? year = null)
        {
            var orderSummaries = _dashboardRepository.GetOrderSummaries(year);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(new {  orderSummaries, status = 1 });
        }
        [Route("FilterDataChart")]
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult GetFilteredOrderDetails([FromBody] OrderFilterModel newList)
        {
            if (newList == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

           var FilterData =  _dashboardRepository.GetFilteredOrderDetails(newList);
                return Ok(new { List = FilterData,  status = 1 });
        }

    }
}
