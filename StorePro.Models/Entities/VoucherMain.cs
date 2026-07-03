using System;

namespace StorePro.Models.Entities
{
    public class VoucherMain
    {
        public long VoucherID { get; set; }
        public Guid Voucher_Guid { get; set; }
        public int VoucherType { get; set; }
        public DateTime VoucherDate { get; set; }
        public int PaymentType { get; set; }
        public decimal VoucherAmount { get; set; }
        public string Remark { get; set; }
        public int? Account_Type { get; set; }
        public Guid Account_Guid { get; set; }
        public Guid Box_Account { get; set; }
        public Guid? Withholding_Account { get; set; }
        public Guid? Bank_Account { get; set; }
        public bool? InsertByOrder { get; set; }
        public int? Bill_OrderType { get; set; }
        public Guid? Bill_OrderID { get; set; }
        public bool? Voucher_IsReady { get; set; }
        public string Voucher_Record_Status { get; set; }
        public decimal? AccBalanceBeforeOrder { get; set; }
        public decimal? AccBalanceAfterOrder { get; set; }
        public string IsTransferredEntries { get; set; }
        public Guid? IsBindToContractGUID { get; set; }
        public decimal? drawerBalanceBeforeVoucher { get; set; }
        public decimal? drawerBalanceAfterVoucher { get; set; }

        // Navigation Properties
        public virtual ChartAccMain ChartAccMain { get; set; }
    }
}