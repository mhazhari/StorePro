using Microsoft.AspNetCore.Mvc;
using StorePro.Core.Services;
using StorePro.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorePro.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// الحصول على جميع الفواتير
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// الحصول على فاتورة برقم المعرف
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound(new { message = "الفاتورة غير موجودة" });

            return Ok(order);
        }

        /// <summary>
        /// الحصول على الفواتير في نطاق تاريخي
        /// </summary>
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var orders = await _orderService.GetOrdersByDateRangeAsync(startDate, endDate);
            return Ok(orders);
        }

        /// <summary>
        /// إنشاء فاتورة جديدة
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.CreateOrderAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = order.OMainGuID }, order);
        }

        /// <summary>
        /// تحديث فاتورة
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<OrderDto>> Update(Guid id, [FromBody] UpdateOrderDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var order = await _orderService.UpdateOrderAsync(id, dto);
            if (order == null)
                return NotFound(new { message = "الفاتورة غير موجودة" });

            return Ok(order);
        }

        /// <summary>
        /// إلغاء فاتورة
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var result = await _orderService.CancelOrderAsync(id);
            if (!result)
                return NotFound(new { message = "الفاتورة غير موجودة" });

            return NoContent();
        }

        /// <summary>
        /// الحصول على الفاتورة للطباعة
        /// </summary>
        [HttpGet("{id}/print")]
        public async Task<ActionResult<OrderPrintDto>> GetForPrint(Guid id)
        {
            var order = await _orderService.GetOrderForPrintAsync(id);
            if (order == null)
                return NotFound(new { message = "الفاتورة غير موجودة" });

            return Ok(order);
        }
    }
}