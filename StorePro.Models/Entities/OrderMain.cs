using System;
using System.Collections.Generic;

namespace StorePro.Models.Entities
{
    public class OrderMain
    {
        public Guid OMainGuID { get; set; }
        public long OrderID { get; set; }
        public int OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid StoreGUID { get; set; }
        public Guid? Account_Guid { get; set; }
        public decimal? BillDiscount { get; set; }
        public decimal? BillSTax { get; set; }
        public decimal? BillSpent { get; set; }
        public string Remark { get; set; }
        public string Ship_Info { get; set; }
        public Guid? StoreGuid_ImMove { get; set; }
        public Guid? OMainGuID_Bind { get; set; }
        public decimal? AccBalanceBeforeOrder { get; set; }
        public decimal? AccBalanceAfterOrder { get; set; }
        public bool? MainOrder_IsReady { get; set; }
        public string InvoiceRecordStatus { get; set; }
        public DateTime? LastPrintedOrder { get; set; }
        public string SOLD_TO_Info { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? PaymentDue_Date { get; set; }
        public long? BoxMoney_AccountID { get; set; }
        public int? MaxDay { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Recipt { get; set; }
        public decimal? Remainder { get; set; }
        public long? VoucherType { get; set; }
        public long? _VoucherID { get; set; }
        public Guid? _VoucherGUID { get; set; }
        public DateTime Ins_Date { get; set; }
        public Guid? UserID { get; set; }
        public Guid? Upd_UserID { get; set; }
        public DateTime? Upd_Date { get; set; }
        public string InsertByComputerID { get; set; }
        public string UpdateByComputerID { get; set; }
        public Guid? From_CheckActual_GUID { get; set; }
        public string IsTransferredEntries { get; set; }
        public Guid? IsBindToContractGUID { get; set; }
        public string PriceListTYPE { get; set; }
        public Guid? Delivery_Guid { get; set; }
        public decimal? Delivery_Cost { get; set; }
        public string isBillDiscountType { get; set; }
        public string isBillTaxType { get; set; }
        public string isBillSpentType { get; set; }
        public Guid? Mediator_GUID { get; set; }
        public bool? IsSaveCostAnalysis { get; set; }
        public DateTime? IsSaveCostAnalysisApplyDate { get; set; }
        public bool? Delivery_IsSuccess { get; set; }
        public DateTime? Delivery_IsSuccessTime { get; set; }
        public DateTime? deleted_DateTime { get; set; }
        public string deleted_SessionID { get; set; }
        public string deleted_ByComputerName { get; set; }
        public string deleted_ByComputerCPUID { get; set; }
        public Guid? deleted_UserId { get; set; }
        public string deleted_ByUserName { get; set; }
        public string deleted_userComments { get; set; }
        public decimal? CashPaidMoneyRecipt { get; set; }

        // Navigation Properties
        public virtual ICollection<OrderProducts> OrderProducts { get; set; } = new List<OrderProducts>();
    }
}