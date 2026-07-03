using System;
using System.Collections.Generic;

namespace StorePro.Models.Entities
{
    public class TblItmOnUnit
    {
        public Guid ProductID { get; set; }
        public Guid Unit_GUID { get; set; }
        public int Unit_Key { get; set; }
        public decimal? Unit_Pack { get; set; }
        public decimal? Price_Cost { get; set; }
        public decimal? Price_Sales { get; set; }
        public decimal? WholeSale { get; set; }
        public decimal? WholeSaleHalf { get; set; }
        public bool? IsSales { get; set; }
        public bool? IsBuy { get; set; }
        public string Unit_Name { get; set; }
        public bool? IsDefaultSelected { get; set; }
        public decimal? MiniSale { get; set; }

        // Navigation Properties
        public virtual TblItm TblItm { get; set; }
    }
}