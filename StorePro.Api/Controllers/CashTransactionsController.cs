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
    public class CashTransactionsController : ControllerBase
    {
        private readonly ICashTransactionService _cashService;

        public CashTransactionsController(ICashTransactionService cashService)
        {
            _cashService = cashService;
        }

        /// <summary>
        /// الحصول على جميع الإيصالات
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VoucherDto>>> GetAll()
        {
            var vouchers = await _cashService.GetAllVouchersAsync();
            return Ok(vouchers);
        }

        /// <summary>
        /// الحصول على إيصال برقم المعرف
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<VoucherDto>> GetById(long id)
        {
            var voucher = await _cashService.GetVoucherByIdAsync(id);
            if (voucher == null)
                return NotFound(new { message = "الإيصال غير موجود" });

            return Ok(voucher);
        }

        /// <summary>
        /// الحصول على الإيصالات في نطاق تاريخي
        /// </summary>
        [HttpGet("date-range")]
        public async Task<ActionResult<IEnumerable<VoucherDto>>> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var vouchers = await _cashService.GetVouchersByDateRangeAsync(startDate, endDate);
            return Ok(vouchers);
        }

        /// <summary>
        /// الحصول على رصيد الصندوق اليومي
        /// </summary>
        [HttpGet("daily-balance")]
        public async Task<ActionResult<CashBalanceDto>> GetDailyBalance([FromQuery] DateTime date)
        {
            var balance = await _cashService.GetDailyCashBalanceAsync(date);
            return Ok(balance);
        }

        /// <summary>
        /// إنشاء إيصال جديد
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<VoucherDto>> Create([FromBody] CreateVoucherDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var voucher = await _cashService.CreateVoucherAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = voucher.VoucherID }, voucher);
        }
    }
}