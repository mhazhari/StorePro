using System;
using System.Collections.Generic;

namespace StorePro.Models.Entities
{
    public class ChartAccMain
    {
        public long Acc_MainID { get; set; }
        public Guid Account_Guid { get; set; }
        public string Acc_Name { get; set; }
        public string Acc_No { get; set; }
        public bool Acc_IsMain { get; set; }
        public long Parent_Acc_ID { get; set; }
        public string parentName { get; set; }
        public bool Kind_ID { get; set; }
        public bool IsEnabled { get; set; }
        public int? Acc_Lvl { get; set; }
        public bool? imIsDefault { get; set; }
        public bool? Active_Trade { get; set; }
        public bool? Active_Money { get; set; }
        public bool? Active_CustSupp { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string GroupName { get; set; }
        public decimal? AllowMaxDebit { get; set; }
        public decimal? AllowMaxCredit { get; set; }
        public bool? Allow_Payments_AsDelayed { get; set; }

        // Navigation Properties
        public virtual ICollection<VoucherMain> VoucherMains { get; set; } = new List<VoucherMain>();
    }
}