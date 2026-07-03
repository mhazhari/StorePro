using Microsoft.EntityFrameworkCore;
using StorePro.Models.Entities;

namespace StorePro.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // جداول الأصناف والمخزون
        public DbSet<TblItm> TblItm { get; set; }
        public DbSet<TblItmOnUnit> TblItmOnUnit { get; set; }

        // جداول الفواتير والمبيعات
        public DbSet<OrderMain> OrderMain { get; set; }
        public DbSet<OrderProducts> OrderProducts { get; set; }

        // جداول النقدية والحسابات
        public DbSet<VoucherMain> VoucherMain { get; set; }
        public DbSet<ChartAccMain> ChartAccMain { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== جدول الأصناف ==========
            modelBuilder.Entity<TblItm>(entity =>
            {
                entity.HasKey(e => e.ProductID);
                entity.ToTable("Tbl_ITm");
                
                entity.Property(e => e.ProductID).HasColumnName("ProductID");
                entity.Property(e => e.ITm_ID).HasColumnName("ITm_ID").ValueGeneratedOnAdd();
                entity.Property(e => e.ITm_Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PrivateCode).HasMaxLength(50);
                entity.Property(e => e.CategoryName).HasMaxLength(50);
                entity.Property(e => e.CategorysTopMenuName).HasMaxLength(50);
                entity.Property(e => e.GeneralFactoryCode).HasMaxLength(50);
                entity.Property(e => e.Size_Info).HasMaxLength(50);
                entity.Property(e => e.Style_Width).HasPrecision(18, 2);
                entity.Property(e => e.Style_Height).HasPrecision(18, 2);
                entity.Property(e => e.Remark).HasMaxLength(260);
                entity.Property(e => e.StorMsgMin_UnitType).HasMaxLength(50);
                entity.Property(e => e.StorMsgMax_UnitType).HasMaxLength(50);
                entity.Property(e => e.StorGetOrder_UnitType).HasMaxLength(50);
                entity.Property(e => e.InfoPlace).HasMaxLength(128);
                
                // Decimal Properties
                entity.Property(e => e.ProfitRate_FromPurchases).HasPrecision(18, 3);
                entity.Property(e => e.Purchase_discount).HasPrecision(18, 3);
                entity.Property(e => e.Sale_discount).HasPrecision(18, 3);
                entity.Property(e => e.Discount_WholeSaleHalf).HasPrecision(18, 3);
                entity.Property(e => e.Discount_WholeSale).HasPrecision(18, 3);
                entity.Property(e => e.Discount_MiniSale).HasPrecision(18, 3);

                // Relationships
                entity.HasMany(e => e.TblItmOnUnits)
                    .WithOne(u => u.TblItm)
                    .HasForeignKey(u => u.ProductID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.OrderProducts)
                    .WithOne(o => o.TblItm)
                    .HasForeignKey(o => o.ProductID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ========== جدول وحدات الأصناف ==========
            modelBuilder.Entity<TblItmOnUnit>(entity =>
            {
                entity.HasKey(e => new { e.ProductID, e.Unit_GUID });
                entity.ToTable("Tbl_ITm_OnUnit");
                
                entity.Property(e => e.Unit_Pack).HasPrecision(30, 3);
                entity.Property(e => e.Price_Cost).HasPrecision(30, 6);
                entity.Property(e => e.Price_Sales).HasPrecision(30, 4);
                entity.Property(e => e.WholeSale).HasPrecision(30, 4);
                entity.Property(e => e.WholeSaleHalf).HasPrecision(30, 4);
                entity.Property(e => e.MiniSale).HasPrecision(30, 4);
                entity.Property(e => e.Unit_Name).HasMaxLength(20);

                entity.HasOne(e => e.TblItm)
                    .WithMany(p => p.TblItmOnUnits)
                    .HasForeignKey(e => e.ProductID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ========== جدول الفواتير الرئيسي ==========
            modelBuilder.Entity<OrderMain>(entity =>
            {
                entity.HasKey(e => e.OMainGuID);
                entity.ToTable("Order_Main");
                
                entity.Property(e => e.OMainGuID).HasColumnName("OMainGuID");
                entity.Property(e => e.OrderID).ValueGeneratedOnAdd();
                
                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.Ins_Date).IsRequired();
                
                // Decimal Properties
                entity.Property(e => e.BillDiscount).HasPrecision(18, 4);
                entity.Property(e => e.BillSTax).HasPrecision(18, 4);
                entity.Property(e => e.BillSpent).HasPrecision(18, 4);
                entity.Property(e => e.Amount).HasPrecision(18, 4);
                entity.Property(e => e.Recipt).HasPrecision(18, 4);
                entity.Property(e => e.Remainder).HasPrecision(18, 4);
                entity.Property(e => e.Delivery_Cost).HasPrecision(18, 4);
                entity.Property(e => e.CashPaidMoneyRecipt).HasPrecision(18, 4);
                entity.Property(e => e.AccBalanceBeforeOrder).HasPrecision(18, 4);
                entity.Property(e => e.AccBalanceAfterOrder).HasPrecision(18, 4);
                
                // String Properties
                entity.Property(e => e.Remark).HasMaxLength(200);
                entity.Property(e => e.Ship_Info).HasMaxLength(200);
                entity.Property(e => e.InvoiceRecordStatus).HasMaxLength(256);
                entity.Property(e => e.SOLD_TO_Info).HasMaxLength(255);
                entity.Property(e => e.PaymentMethod).HasMaxLength(20);
                entity.Property(e => e.PriceListTYPE).HasMaxLength(50);
                entity.Property(e => e.isBillDiscountType).HasMaxLength(6);
                entity.Property(e => e.isBillTaxType).HasMaxLength(6);
                entity.Property(e => e.isBillSpentType).HasMaxLength(6);

                // Relationships
                entity.HasMany(e => e.OrderProducts)
                    .WithOne(o => o.OrderMain)
                    .HasForeignKey(o => o.OMainGuID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ========== جدول تفاصيل الفواتير ==========
            modelBuilder.Entity<OrderProducts>(entity =>
            {
                entity.HasKey(e => new { e.OMainGuID, e.ProdOrderGuid });
                entity.ToTable("Order_Products");
                
                entity.Property(e => e.Qty).HasPrecision(20, 4);
                entity.Property(e => e.Price).HasPrecision(20, 4);
                entity.Property(e => e.BonusQuantity).HasPrecision(20, 4);
                entity.Property(e => e.SubTotal).HasPrecision(20, 4);
                entity.Property(e => e.Discount).HasPrecision(20, 4);
                entity.Property(e => e.STax).HasPrecision(20, 4);
                entity.Property(e => e.NetAmount).HasPrecision(20, 4);
                entity.Property(e => e.Prod_LengthMM).HasPrecision(20, 4);
                entity.Property(e => e.Prod_WidthMM).HasPrecision(20, 4);
                entity.Property(e => e.Prod_HeightMM).HasPrecision(20, 4);
                entity.Property(e => e.Prod_Cubic_Millimeter).HasPrecision(20, 4);
                entity.Property(e => e.Prod_AllQty).HasPrecision(20, 4);
                entity.Property(e => e.Price_Cost).HasPrecision(20, 4);
                entity.Property(e => e.CostTOTAL).HasPrecision(20, 4);
                entity.Property(e => e.CostBonusTotal).HasPrecision(20, 4);
                entity.Property(e => e.Profit).HasPrecision(24, 6);
                entity.Property(e => e.ProfitRate).HasPrecision(24, 6);
                entity.Property(e => e.CostOfGoodSold).HasPrecision(24, 6);
                entity.Property(e => e.OnBill_Unit_Pack2).HasPrecision(18, 4);
                entity.Property(e => e.OnBill_Unit_Pack3).HasPrecision(18, 4);
                entity.Property(e => e.QtyToSmallUnitCalculation).HasPrecision(18, 4);
                entity.Property(e => e.VATRate).HasPrecision(12, 3);
                entity.Property(e => e.Edit_PriceSALE).HasPrecision(18, 2);

                entity.Property(e => e.ITm_Name).HasMaxLength(256);
                entity.Property(e => e.Unit_Header).HasMaxLength(256);
                entity.Property(e => e.Product_Remark).HasMaxLength(256);
                entity.Property(e => e.Product_Record_Status).HasMaxLength(256);
                entity.Property(e => e.OnBill_Full_UnitName).HasMaxLength(256);
                entity.Property(e => e.VATRateName).HasMaxLength(50);

                entity.HasOne(e => e.OrderMain)
                    .WithMany(o => o.OrderProducts)
                    .HasForeignKey(e => e.OMainGuID)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.TblItm)
                    .WithMany(p => p.OrderProducts)
                    .HasForeignKey(e => e.ProductID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ========== جدول الحسابات ==========
            modelBuilder.Entity<ChartAccMain>(entity =>
            {
                entity.HasKey(e => e.Account_Guid);
                entity.ToTable("ChartAcc_Main");
                
                entity.Property(e => e.Acc_Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Acc_No).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.Acc_No).IsUnique();
                
                entity.Property(e => e.parentName).HasMaxLength(50);
                entity.Property(e => e.GroupName).HasMaxLength(100);
                entity.Property(e => e.MobileNo1).HasMaxLength(16);
                entity.Property(e => e.MobileNo2).HasMaxLength(16);
                
                entity.Property(e => e.AllowMaxDebit).HasPrecision(30, 4);
                entity.Property(e => e.AllowMaxCredit).HasPrecision(30, 4);

                entity.HasMany(e => e.VoucherMains)
                    .WithOne(v => v.ChartAccMain)
                    .HasForeignKey(v => v.Account_Guid)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // ========== جدول الايصالات النقدية ==========
            modelBuilder.Entity<VoucherMain>(entity =>
            {
                entity.HasKey(e => e.VoucherID);
                entity.ToTable("Voucher_Main");
                
                entity.HasIndex(e => e.Voucher_Guid).IsUnique();
                
                entity.Property(e => e.VoucherAmount).HasPrecision(18, 2);
                entity.Property(e => e.Remark).HasMaxLength(200);
                entity.Property(e => e.Voucher_Record_Status).HasMaxLength(50);
                entity.Property(e => e.IsTransferredEntries).HasMaxLength(256);
                
                entity.Property(e => e.AccBalanceBeforeOrder).HasPrecision(20, 4);
                entity.Property(e => e.AccBalanceAfterOrder).HasPrecision(20, 4);
                entity.Property(e => e.drawerBalanceBeforeVoucher).HasPrecision(20, 4);
                entity.Property(e => e.drawerBalanceAfterVoucher).HasPrecision(20, 4);

                entity.HasOne(e => e.ChartAccMain)
                    .WithMany(c => c.VoucherMains)
                    .HasForeignKey(e => e.Account_Guid)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}