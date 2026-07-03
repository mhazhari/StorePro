using AutoMapper;
using StorePro.Data.Repositories;
using StorePro.Models.Dtos;
using StorePro.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StorePro.Core.Services
{
    public class CashTransactionService : ICashTransactionService
    {
        private readonly IRepository<VoucherMain> _voucherRepository;
        private readonly IRepository<ChartAccMain> _accountRepository;
        private readonly IMapper _mapper;

        public CashTransactionService(
            IRepository<VoucherMain> voucherRepository,
            IRepository<ChartAccMain> accountRepository,
            IMapper mapper)
        {
            _voucherRepository = voucherRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<VoucherDto> GetVoucherByIdAsync(long voucherId)
        {
            var voucher = await _voucherRepository.FindAsync(v => v.VoucherID == voucherId);
            var voucherMain = voucher.FirstOrDefault();

            if (voucherMain == null) return null;

            var dto = _mapper.Map<VoucherDto>(voucherMain);
            var account = await _accountRepository.GetByIdAsync(voucherMain.Account_Guid);
            if (account != null)
                dto.AccountName = account.Acc_Name;

            return dto;
        }

        public async Task<IEnumerable<VoucherDto>> GetAllVouchersAsync()
        {
            var vouchers = await _voucherRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<VoucherDto>>(vouchers);
        }

        public async Task<IEnumerable<VoucherDto>> GetVouchersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var vouchers = await _voucherRepository.FindAsync(v =>
                v.VoucherDate >= startDate && v.VoucherDate <= endDate
            );

            return _mapper.Map<IEnumerable<VoucherDto>>(vouchers);
        }

        public async Task<CashBalanceDto> GetDailyCashBalanceAsync(DateTime date)
        {
            var startOfDay = date.Date;
            var endOfDay = startOfDay.AddDays(1).AddTicks(-1);

            var vouchers = await _voucherRepository.FindAsync(v =>
                v.VoucherDate >= startOfDay && v.VoucherDate <= endOfDay
            );

            decimal totalIncome = vouchers
                .Where(v => v.VoucherType == 2)
                .Sum(v => v.VoucherAmount);

            decimal totalExpense = vouchers
                .Where(v => v.VoucherType == 1)
                .Sum(v => v.VoucherAmount);

            return new CashBalanceDto
            {
                Date = date,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = totalIncome - totalExpense,
                VoucherAmount = vouchers.Sum(v => v.VoucherAmount)
            };
        }

        public async Task<VoucherDto> CreateVoucherAsync(CreateVoucherDto dto)
        {
            var voucher = new VoucherMain
            {
                Voucher_Guid = Guid.NewGuid(),
                VoucherType = dto.VoucherType,
                VoucherDate = DateTime.Now,
                PaymentType = dto.PaymentType,
                VoucherAmount = dto.VoucherAmount,
                Account_Guid = dto.Account_Guid,
                Box_Account = dto.Box_Account,
                Remark = dto.Remark
            };

            await _voucherRepository.AddAsync(voucher);
            await _voucherRepository.SaveChangesAsync();

            return _mapper.Map<VoucherDto>(voucher);
        }
    }
}