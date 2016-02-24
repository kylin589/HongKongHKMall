﻿
 //------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HKTHMall.Domain.Entities
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HKTHMallEntities : DbContext
    {
        public HKTHMallEntities()
            : base("name=HKTHMallEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AC_Department> AC_Department { get; set; }
        public virtual DbSet<AC_Function> AC_Function { get; set; }
        public virtual DbSet<AC_Module> AC_Module { get; set; }
        public virtual DbSet<AC_OperateLog> AC_OperateLog { get; set; }
        public virtual DbSet<AC_Role> AC_Role { get; set; }
        public virtual DbSet<AC_User> AC_User { get; set; }
        public virtual DbSet<APP_VersionInfo> APP_VersionInfo { get; set; }
        public virtual DbSet<banner> banner { get; set; }
        public virtual DbSet<bannerProduct> bannerProduct { get; set; }
        public virtual DbSet<BD_Bank> BD_Bank { get; set; }
        public virtual DbSet<BD_NewsInfo> BD_NewsInfo { get; set; }
        public virtual DbSet<BD_NewsType> BD_NewsType { get; set; }
        public virtual DbSet<BD_NewsTypelang> BD_NewsTypelang { get; set; }
        public virtual DbSet<Blobs> Blobs { get; set; }
        public virtual DbSet<Brand> Brand { get; set; }
        public virtual DbSet<Brand_Category> Brand_Category { get; set; }
        public virtual DbSet<Brand_Lang> Brand_Lang { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Category_Lang> Category_Lang { get; set; }
        public virtual DbSet<CategoryType> CategoryType { get; set; }
        public virtual DbSet<Complaints> Complaints { get; set; }
        public virtual DbSet<CompoundKeyDetail> CompoundKeyDetail { get; set; }
        public virtual DbSet<CompoundKeyMaster> CompoundKeyMaster { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<DateTimeOffsetTest> DateTimeOffsetTest { get; set; }
        public virtual DbSet<DeleteTest> DeleteTest { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<EnumTest> EnumTest { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLog { get; set; }
        public virtual DbSet<Favorites> Favorites { get; set; }
        public virtual DbSet<Feedback> Feedback { get; set; }
        public virtual DbSet<FeedbackType> FeedbackType { get; set; }
        public virtual DbSet<FeedbackType_lang> FeedbackType_lang { get; set; }
        public virtual DbSet<FloorCategory> FloorCategory { get; set; }
        public virtual DbSet<FloorConfig> FloorConfig { get; set; }
        public virtual DbSet<FloorKeyword> FloorKeyword { get; set; }
        public virtual DbSet<GeographyTest> GeographyTest { get; set; }
        public virtual DbSet<GeometryTest> GeometryTest { get; set; }
        public virtual DbSet<GroupTestDetail> GroupTestDetail { get; set; }
        public virtual DbSet<GroupTestMaster> GroupTestMaster { get; set; }
        public virtual DbSet<HierarchyIdTest> HierarchyIdTest { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Items> Items { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<MultiLanguage> MultiLanguage { get; set; }
        public virtual DbSet<NewInfo> NewInfo { get; set; }
        public virtual DbSet<Notes> Notes { get; set; }
        public virtual DbSet<OptionalColumnTest> OptionalColumnTest { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderAddress> OrderAddress { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<OrderDetails_lang> OrderDetails_lang { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrderTrackingLog> OrderTrackingLog { get; set; }
        public virtual DbSet<PagingTest> PagingTest { get; set; }
        public virtual DbSet<ParameterSet> ParameterSet { get; set; }
        public virtual DbSet<PaymentOrder> PaymentOrder { get; set; }
        public virtual DbSet<PaymentOrder_Orders> PaymentOrder_Orders { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Product_Lang> Product_Lang { get; set; }
        public virtual DbSet<ProductConsult> ProductConsult { get; set; }
        public virtual DbSet<ProductImage> ProductImage { get; set; }
        public virtual DbSet<ProductPic> ProductPic { get; set; }
        public virtual DbSet<ProductRule> ProductRule { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public virtual DbSet<PurchaseOrderDetails> PurchaseOrderDetails { get; set; }
        public virtual DbSet<RecommendBonusRecord> RecommendBonusRecord { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<ReportType> ReportType { get; set; }
        public virtual DbSet<ReportType_lang> ReportType_lang { get; set; }
        public virtual DbSet<ReturnProductInfo> ReturnProductInfo { get; set; }
        public virtual DbSet<SalesProduct> SalesProduct { get; set; }
        public virtual DbSet<SalesRule> SalesRule { get; set; }
        public virtual DbSet<ShipmentTemplate> ShipmentTemplate { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCart { get; set; }
        public virtual DbSet<SKU_Attributes> SKU_Attributes { get; set; }
        public virtual DbSet<SKU_AttributeValues> SKU_AttributeValues { get; set; }
        public virtual DbSet<SKU_Product> SKU_Product { get; set; }
        public virtual DbSet<SKU_ProductAttributes> SKU_ProductAttributes { get; set; }
        public virtual DbSet<SKU_ProductTypeAttribute> SKU_ProductTypeAttribute { get; set; }
        public virtual DbSet<SKU_ProductTypes> SKU_ProductTypes { get; set; }
        public virtual DbSet<SKU_SKUItems> SKU_SKUItems { get; set; }
        public virtual DbSet<SP_ProductComment> SP_ProductComment { get; set; }
        public virtual DbSet<Suppliers> Suppliers { get; set; }
        public virtual DbSet<THArea> THArea { get; set; }
        public virtual DbSet<THArea_lang> THArea_lang { get; set; }
        public virtual DbSet<TimestampTest> TimestampTest { get; set; }
        public virtual DbSet<UserAddress> UserAddress { get; set; }
        public virtual DbSet<UserLoginLog> UserLoginLog { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UsersWithChar> UsersWithChar { get; set; }
        public virtual DbSet<YF_FareTemplate> YF_FareTemplate { get; set; }
        public virtual DbSet<YF_FareTemplateAreaGroup> YF_FareTemplateAreaGroup { get; set; }
        public virtual DbSet<YH_Agent> YH_Agent { get; set; }
        public virtual DbSet<YH_GroupMark> YH_GroupMark { get; set; }
        public virtual DbSet<YH_MerchantInfo> YH_MerchantInfo { get; set; }
        public virtual DbSet<YH_PasswordError> YH_PasswordError { get; set; }
        public virtual DbSet<YH_User> YH_User { get; set; }
        public virtual DbSet<YH_UserBankAccount> YH_UserBankAccount { get; set; }
        public virtual DbSet<YH_UserFeedBack> YH_UserFeedBack { get; set; }
        public virtual DbSet<YH_UserLoginLog> YH_UserLoginLog { get; set; }
        public virtual DbSet<YH_UserUpdateInfo> YH_UserUpdateInfo { get; set; }
        public virtual DbSet<YH_UserVerify> YH_UserVerify { get; set; }
        public virtual DbSet<YH_UserVisitingCard> YH_UserVisitingCard { get; set; }
        public virtual DbSet<YH_ValidEmail> YH_ValidEmail { get; set; }
        public virtual DbSet<ZJ_AmountChangeType> ZJ_AmountChangeType { get; set; }
        public virtual DbSet<ZJ_AmountChangeType_lang> ZJ_AmountChangeType_lang { get; set; }
        public virtual DbSet<ZJ_RechargeOrder> ZJ_RechargeOrder { get; set; }
        public virtual DbSet<ZJ_UserBalance> ZJ_UserBalance { get; set; }
        public virtual DbSet<ZJ_UserBalanceChangeLog> ZJ_UserBalanceChangeLog { get; set; }
        public virtual DbSet<ZJ_WithdrawOrder> ZJ_WithdrawOrder { get; set; }
        public virtual DbSet<DecimalTest> DecimalTest { get; set; }
        public virtual DbSet<SchemaTable> SchemaTable { get; set; }
    }
}
