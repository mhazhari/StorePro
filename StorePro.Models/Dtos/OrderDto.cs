using System;
using System.Collections.Generic;

namespace StorePro.Models.Dtos
{
    public class OrderDto
    {
        public Guid OMainGuID { get; set; }
        public long OrderID { get; set; }
        public int OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid StoreGUID { get; set; }
        public string SOLD_TO_Info { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? BillDiscount { get; set; }
        public decimal? BillSTax { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Remainder { get; set; }
        public string InvoiceRecordStatus { get; set; }
        public string Remark { get; set; }

        public List<OrderProductDto> OrderDetails { get; set; } = new();
    }

    public class OrderProductDto
    {
        public Guid ProductID { get; set; }
        public string ITm_Name { get; set; }
        public string Unit_Header { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal STax { get; set; }
        public decimal NetAmount { get; set; }
    }

    public class CreateOrderDto
    {
        public int OrderType { get; set; }
        public Guid StoreGUID { get; set; }
        public Guid? Account_Guid { get; set; }
        public string SOLD_TO_Info { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? BillDiscount { get; set; }
        public decimal? BillSTax { get; set; }
        public string Remark { get; set; }
        public Guid? UserID { get; set; }

        public List<CreateOrderProductDto> OrderDetails { get; set; }
    }

    public class CreateOrderProductDto
    {
        public Guid ProductID { get; set; }
        public Guid Unit_GUID { get; set; }
        public decimal Qty { get; set; }
        public decimal Price { get; set; }
        public decimal? Discount { get; set; }
        public decimal? STax { get; set; }
    }

    public class UpdateOrderDto
    {
        public string SOLD_TO_Info { get; set; }
        public string PaymentMethod { get; set; }
        public decimal? BillDiscount { get; set; }
        public string Remark { get; set; }

        public List<CreateOrderProductDto> OrderDetails { get; set; }
    }

    public class OrderPrintDto
    {
        public long OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string SOLD_TO_Info { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string PaymentMethod { get; set; }
        public List<OrderProductDto> Details { get; set; }
    }
}