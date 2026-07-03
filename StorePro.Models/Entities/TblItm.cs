using System;
using System.Collections.Generic;

namespace StorePro.Models.Entities
{
    public class TblItm
    {
        public Guid ProductID { get; set; }
        public long ITm_ID { get; set; }
        public string CategorysTopMenuName { get; set; }
        public string CategoryName { get; set; }
        public string PrivateCode { get; set; }
        public string ITm_Name { get; set; }
        public bool? IsBlocked { get; set; }
        public bool? IsUnits_AutoCalc { get; set; }
        public int? Supp_ID { get; set; }
        public int? MakerID { get; set; }
        public string GeneralFactoryCode { get; set; }
        public string Size_Info { get; set; }
        public decimal? Style_Width { get; set; }
        public decimal? Style_Height { get; set; }
        public string Remark { get; set; }
        public Guid? UnitID_DefBuy { get; set; }
        public Guid? UnitID_DefSale { get; set; }
        public DateTime? Ins_Date { get; set; }
        public DateTime? Upd_Date { get; set; }
        public bool? IsExpired { get; set; }
        public int? StorExpirDay { get; set; }
        public int? StorSalesStatic { get; set; }
        public int? StorMsgMin { get; set; }
        public int? StorMsgMax { get; set; }
        public int? StorGetOrder { get; set; }
        public decimal? ProfitRate_FromPurchases { get; set; }
        public Guid? VATRateGUID { get; set; }
        public bool? Purchase_discountTYPE { get; set; }
        public decimal? Purchase_discount { get; set; }
        public bool? Sale_discountTYPE { get; set; }
        public decimal? Sale_discount { get; set; }
        public string InfoPlace { get; set; }
        public string StorMsgMin_UnitType { get; set; }
        public string StorMsgMax_UnitType { get; set; }
        public string StorGetOrder_UnitType { get; set; }
        public decimal? Discount_WholeSaleHalf { get; set; }
        public decimal? Discount_WholeSale { get; set; }
        public decimal? Discount_MiniSale { get; set; }
        public bool? IsAllowWholeSaleHalf { get; set; }
        public bool? IsAllowWholeSale { get; set; }
        public bool? IsAllowMiniSale { get; set; }
        public int? _CategorysTopMenuId { get; set; }
        public int? _CategoryId { get; set; }

        // Navigation Properties
        public virtual ICollection<TblItmOnUnit> TblItmOnUnits { get; set; } = new List<TblItmOnUnit>();
        public virtual ICollection<OrderProducts> OrderProducts { get; set; } = new List<OrderProducts>();
    }
}