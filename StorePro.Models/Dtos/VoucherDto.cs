using System;

namespace StorePro.Models.Dtos
{
    public class VoucherDto
    {
        public long VoucherID { get; set; }
        public Guid Voucher_Guid { get; set; }
        public int VoucherType { get; set; }
        public DateTime VoucherDate { get; set; }
        public int PaymentType { get; set; }
        public decimal VoucherAmount { get; set; }
        public string AccountName { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
    }

    public class CreateVoucherDto
    {
        public int VoucherType { get; set; }
        public int PaymentType { get; set; }
        public decimal VoucherAmount { get; set; }
        public Guid Account_Guid { get; set; }
        public Guid Box_Account { get; set; }
        public string Remark { get; set; }
    }

    public class CashBalanceDto
    {
        public DateTime Date { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal Balance { get; set; }
        public decimal VoucherAmount { get; set; }
    }
}