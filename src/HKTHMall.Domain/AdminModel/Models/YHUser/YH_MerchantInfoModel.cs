using FluentValidation.Attributes;
using HKTHMall.Domain.AdminModel.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Domain.AdminModel.Models.YHUser
{
    [Validator(typeof(YH_MerchantInfoValidator))]
    public class YH_MerchantInfoModel
    {
        /// <summary>
        /// 商家ID
        /// </summary>
        public long MerchantID { get; set; }

        /// <summary>
        /// 商家名称
        /// </summary>
        public string ShopName { get; set; }
        
        /// <summary>
        /// 商家介绍
        /// </summary>
        public string Introduction { get; set; }
        
        /// <summary>
        /// 商家类型
        /// </summary>
        public Nullable<int> MerchantType { get; set; }
        
        /// <summary>
        /// 是否能发布商品
        /// </summary>
        public Nullable<int> IsPublishProduct { get; set; }
        
        /// <summary>
        /// 是否提供发票
        /// </summary>
        public bool IsProvideInvoices { get; set; }
        
        /// <summary>
        /// 商家所在地区ID
        /// </summary>
        public Nullable<int> AreaID { get; set; }

        /// <summary>
        /// 省ID
        /// </summary>
        public Nullable<int> ShengTHAreaID { get; set; }

        /// <summary>
        /// 市ID
        /// </summary>
        public Nullable<int> ShiTHAreaID { get; set; }
        
        /// <summary>
        /// 商家地址
        /// </summary>
        public string ShopAddress { get; set; }
        
        /// <summary>
        /// 经度
        /// </summary>
        public Nullable<decimal> Longitude { get; set; }
        
        /// <summary>
        /// 纬度
        /// </summary>
        public Nullable<decimal> Latitude { get; set; }
        
        /// <summary>
        /// 商家电话
        /// </summary>
        public string Tel { get; set; }
        
        /// <summary>
        /// 商家手机
        /// </summary>
        public string Phone { get; set; }
        
        /// <summary>
        /// 客服QQ
        /// </summary>
        public string QQ { get; set; }
        
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// 业务联系人
        /// </summary>
        public string BusinessContacter { get; set; }
        
        /// <summary>
        /// 业务联系人电话
        /// </summary>
        public string BusinessTel { get; set; }
        
        /// <summary>
        /// 其他联系方式
        /// </summary>
        public string OtherTel { get; set; }
        
        /// <summary>
        /// 发货地址
        /// </summary>
        public string ShipperAddress { get; set; }
        
        /// <summary>
        /// 退/ 换货区域ID
        /// </summary>
        public Nullable<int> ConsignerID { get; set; }
        
        /// <summary>
        /// 退/ 换货地址
        /// </summary>
        public string ConsignerAddress { get; set; }
        
        /// <summary>
        /// 其他电商平台地址
        /// </summary>
        public string ShoppingPlatform { get; set; }
        
        /// <summary>
        /// 物流说明
        /// </summary>
        public string ExpressCompany { get; set; }
        
        /// <summary>
        /// 审核状态
        /// </summary>
        public int AuditStatus { get; set; }
        
        /// <summary>
        /// 审核人
        /// </summary>
        public string AuditBy { get; set; }
        
        /// <summary>
        /// 审核时间
        /// </summary>
        public Nullable<System.DateTime> AuditDT { get; set; }
        
        /// <summary>
        /// 审核备注
        /// </summary>
        public string AuditRemark { get; set; }
        
        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }
        
        /// <summary>
        /// 营业执照号
        /// </summary>
        public string BusinessLicense { get; set; }
        
        /// <summary>
        /// 税务登记证号
        /// </summary>
        public string TaxCode { get; set; }
        
        /// <summary>
        /// 企业组织机构代码
        /// </summary>
        public string OrganizationCode { get; set; }
        
        /// <summary>
        /// 法人代表
        /// </summary>
        public string LegalPerson { get; set; }
        
        /// <summary>
        /// 法人身份证号
        /// </summary>
        public string LegalIdentityCard { get; set; }
        
        /// <summary>
        /// 经营类别
        /// </summary>
        public Nullable<int> IndustryID { get; set; }
        
        /// <summary>
        /// 品牌名称
        /// </summary>
        public string Brand { get; set; }
        
        /// <summary>
        /// 品牌Logo
        /// </summary>
        public string BrandLogoURL { get; set; }
        
        /// <summary>
        /// 品牌授权书/代理
        /// </summary>
        public string BrandAuthorization { get; set; }
        
        /// <summary>
        /// 保证金
        /// </summary>
        public Nullable<decimal> Margin { get; set; }
        
        /// <summary>
        /// 扣点费率
        /// </summary>
        public Nullable<decimal> CommissionRate { get; set; }
        
        /// <summary>
        /// 招商经理姓名
        /// </summary>
        public string LeasingManager { get; set; }
        
        /// <summary>
        /// 招商经理电话
        /// </summary>
        public string LeasingPhone { get; set; }
        
        /// <summary>
        /// 运费计算方式
        /// </summary>
        public Nullable<int> FreightType { get; set; }
        
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateBy { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public Nullable<System.DateTime> CreateDT { get; set; }
        
        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateBy { get; set; }
        
        /// <summary>
        /// 修改时间
        /// </summary>
        public Nullable<System.DateTime> UpdateDT { get; set; }
    }
}
