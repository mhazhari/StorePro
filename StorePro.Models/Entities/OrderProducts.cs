using System;

namespace StorePro.Models.Entities
{
    public class OrderProducts
    {
        public long Row_ID { get; set; }
        public Guid OMainGuID { get; set; }
        public Guid ProdOrderGuid { get; set; }
        public Guid StoreGuid { get; set; }
        public Guid ProductID { get; set; }
        public Guid Unit_GUID { get; set; }
        public string ITm_Name { get; set; }
        public string Unit_Header { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal? BonusQuantity { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal STax { get; set; }
        public decimal NetAmount { get; set; }
        public DateTime ExpDate { get; set; }
        public string Product_Remark { get; set; }
        public DateTime? Ins_Date { get; set; }
        public Guid? StoreGuid_ImMove { get; set; }
        public decimal? Prod_LengthMM { get; set; }
        public decimal? Prod_WidthMM { get; set; }
        public decimal? Prod_HeightMM { get; set; }
        public decimal? Prod_Cubic_Millimeter { get; set; }
        public decimal? Prod_AllQty { get; set; }
        public string Product_Record_Status { get; set; }
        public bool? Product_IsReady { get; set; }
        public bool? Record_IsDeletedByUser { get; set; }
        public decimal? Price_Cost { get; set; }
        public decimal? CostTOTAL { get; set; }
        public decimal? CostBonusTotal { get; set; }
        public decimal? Profit { get; set; }
        public decimal? ProfitRate { get; set; }
        public decimal? CostOfGoodSold { get; set; }
        public string OnBill_Full_UnitName { get; set; }
        public decimal? OnBill_Unit_Pack2 { get; set; }
        public decimal? OnBill_Unit_Pack3 { get; set; }
        public decimal? QtyToSmallUnitCalculation { get; set; }
        public Guid? From_CheckActual_GUID { get; set; }
        public string VATRateName { get; set; }
        public decimal? VATRate { get; set; }
        public decimal? Edit_PriceSALE { get; set; }
        public DateTime? deleted_DateTime { get; set; }
        public string deleted_SessionID { get; set; }
        public string deleted_ByComputerName { get; set; }
        public string deleted_ByComputerCPUID { get; set; }
        public Guid? deleted_UserId { get; set; }
        public string deleted_ByUserName { get; set; }
        public string deleted_userComments { get; set; }

        // Navigation Properties
        public virtual OrderMain OrderMain { get; set; }
        public virtual TblItm TblItm { get; set; }
    }
}