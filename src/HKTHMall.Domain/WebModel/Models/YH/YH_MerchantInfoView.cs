using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.WebModel.Models.YH
{
    /// <summary>
    /// 商家信息
    /// </summary>
    public class YH_MerchantInfoView
    {
        public long MerchantID { get; set; }
        public string ShopName { get; set; }
        public string Introduction { get; set; }
        public Nullable<int> MerchantType { get; set; }
        public Nullable<int> IsPublishProduct { get; set; }
        public bool IsProvideInvoices { get; set; }
        public Nullable<int> AreaID { get; set; }
        public string ShopAddress { get; set; }
        public Nullable<decimal> Longitude { get; set; }
        public Nullable<decimal> Latitude { get; set; }
        public string Tel { get; set; }
        public string Phone { get; set; }
        public string QQ { get; set; }
        public string Email { get; set; }
        public string BusinessContacter { get; set; }
        public string BusinessTel { get; set; }
        public string OtherTel { get; set; }
        public string ShipperAddress { get; set; }
        public Nullable<int> ConsignerID { get; set; }
        public string ConsignerAddress { get; set; }
        public string ShoppingPlatform { get; set; }
        public string ExpressCompany { get; set; }
        public Nullable<byte> AuditStatus { get; set; }
        public string AuditBy { get; set; }
        public Nullable<System.DateTime> AuditDT { get; set; }
        public string AuditRemark { get; set; }
        public string CompanyName { get; set; }
        public string BusinessLicense { get; set; }
        public string TaxCode { get; set; }
        public string OrganizationCode { get; set; }
        public string LegalPerson { get; set; }
        public string LegalIdentityCard { get; set; }
        public Nullable<int> IndustryID { get; set; }
        public string Brand { get; set; }
        public string BrandLogoURL { get; set; }
        public string BrandAuthorization { get; set; }
        public Nullable<decimal> Margin { get; set; }
        public Nullable<decimal> CommissionRate { get; set; }
        public string LeasingManager { get; set; }
        public string LeasingPhone { get; set; }
        public Nullable<int> FreightType { get; set; }
        public string CreateBy { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
    }
}
