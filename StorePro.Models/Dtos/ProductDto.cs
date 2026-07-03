using System;
using System.Collections.Generic;

namespace StorePro.Models.Dtos
{
    public class ProductDto
    {
        public Guid ProductID { get; set; }
        public long ITm_ID { get; set; }
        public string ITm_Name { get; set; }
        public string PrivateCode { get; set; }
        public string CategoryName { get; set; }
        public string CategorysTopMenuName { get; set; }
        public string Size_Info { get; set; }
        public string Remark { get; set; }
        public int? StorMsgMin { get; set; }
        public int? StorMsgMax { get; set; }
        public bool? IsBlocked { get; set; }
        public DateTime? Ins_Date { get; set; }
        public DateTime? Upd_Date { get; set; }

        public List<ProductUnitDto> Units { get; set; } = new();
    }

    public class ProductUnitDto
    {
        public Guid Unit_GUID { get; set; }
        public string Unit_Name { get; set; }
        public decimal? Unit_Pack { get; set; }
        public decimal? Price_Cost { get; set; }
        public decimal? Price_Sales { get; set; }
        public decimal? WholeSale { get; set; }
        public decimal? MiniSale { get; set; }
        public bool? IsDefaultSelected { get; set; }
        public bool? IsSales { get; set; }
    }

    public class CreateProductDto
    {
        public string ITm_Name { get; set; }
        public string PrivateCode { get; set; }
        public string CategoryName { get; set; }
        public string CategorysTopMenuName { get; set; }
        public string Size_Info { get; set; }
        public string Remark { get; set; }
        public int? StorMsgMin { get; set; }
        public int? StorMsgMax { get; set; }
    }

    public class UpdateProductDto
    {
        public string ITm_Name { get; set; }
        public string PrivateCode { get; set; }
        public string Remark { get; set; }
        public int? StorMsgMin { get; set; }
        public int? StorMsgMax { get; set; }
    }

    public class SearchProductDto
    {
        public Guid ProductID { get; set; }
        public string ITm_Name { get; set; }
        public string PrivateCode { get; set; }
        public List<ProductUnitDto> Units { get; set; }
    }

    public class LowStockReportDto
    {
        public Guid ProductID { get; set; }
        public string ITm_Name { get; set; }
        public int? CurrentStock { get; set; }
        public int? MinimumStock { get; set; }
        public int? MaximumStock { get; set; }
        public string Status { get; set; }
    }
}