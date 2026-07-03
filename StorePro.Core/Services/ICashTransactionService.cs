using StorePro.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StorePro.Core.Services
{
    public interface ICashTransactionService
    {
        Task<VoucherDto> GetVoucherByIdAsync(long voucherId);
        Task<IEnumerable<VoucherDto>> GetAllVouchersAsync();
        Task<IEnumerable<VoucherDto>> GetVouchersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<CashBalanceDto> GetDailyCashBalanceAsync(DateTime date);
        Task<VoucherDto> CreateVoucherAsync(CreateVoucherDto dto);
    }
}