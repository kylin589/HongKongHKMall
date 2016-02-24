using HKTHMall.Domain.AdminModel.Models.YHUser;
using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.YHUser
{
    public class YH_MerchantInfoService : BaseService, IYH_MerchantInfoService
    {
        /// <summary>
        /// 商家添加
        /// zhoub 20150918
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel Add(YH_MerchantInfoModel model)
        {
            ResultModel result = new ResultModel();
            var merchantInfoModel = base._database.Db.YH_MerchantInfo.Find(base._database.Db.YH_MerchantInfo.MerchantID == model.MerchantID);
            if (merchantInfoModel == null)
            {
                result.Data = _database.Db.YH_MerchantInfo.Insert(model);
                _database.Db.YH_User.UpdateByUserID(UserID: model.MerchantID, UserType: 1);
                result.Messages.Add("Businesses add success.");
            }
            else {
                result.IsValid = false;
                result.Messages.Add("Businesses already exist.");
            }
            return result;
        }

        /// <summary>
        /// 商家更新
        /// zhoub 20150918
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel Edit(YH_MerchantInfoModel model)
        {
            ResultModel result = new ResultModel();
            result.Data=_database.Db.YH_MerchantInfo.UpdateByMerchantID(MerchantID: model.MerchantID, ShopName: model.ShopName, Introduction: model.Introduction, AreaID: model.AreaID, ShopAddress: model.ShopAddress, Tel: model.Tel, Phone: model.Phone, BusinessContacter: model.BusinessContacter, BusinessTel: model.BusinessTel, ShipperAddress: model.ShipperAddress, CompanyName: model.CompanyName, Brand: model.Brand, BrandLogoURL: model.BrandLogoURL, BrandAuthorization: model.BrandAuthorization, Margin: model.Margin, CommissionRate: model.CommissionRate, LeasingManager: model.LeasingManager, LeasingPhone: model.LeasingPhone, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);
            if (result.Data > 0)
            {
                result.Messages.Add("Businesses edit success.");
            }
            else {
                result.IsValid = false;
                result.Messages.Add("Businesses edit Failure.");
            }
            return result;
        }

        /// <summary>
        /// 根据ID获取商家信息
        /// zhoub 20150918
        /// </summary>
        /// <param name="merchantID"></param>
        /// <returns></returns>
        public ResultModel GetYH_MerchantInfoById(long merchantID)
        {
            var me = _database.Db.YH_MerchantInfo;
            dynamic one, two;
            ResultModel result = new ResultModel();
            result.Data = me.All()
                .LeftJoin(_database.Db.THArea.As("t1"), out one).On(one.THAreaID == me.AreaID)
                .LeftJoin(_database.Db.THArea.As("t2"), out two).On(two.THAreaID == one.ParentID)
                .Where(me.MerchantID==merchantID)
                .Select(me.MerchantID,me.AuditStatus, me.ShopName, me.Introduction, me.AreaID, me.ShopAddress, me.Tel, me.Phone, me.BusinessContacter, me.BusinessTel, me.ShipperAddress, me.CompanyName, me.Brand, me.BrandLogoURL, me.BrandAuthorization, me.Margin, me.CommissionRate, me.LeasingManager, me.LeasingPhone, me.AuditRemark, one.ParentID.As("ShiTHAreaID"), two.ParentID.As("ShengTHAreaID"))
                .ToList<YH_MerchantInfoModel>();
            return result;
        }

        /// <summary>
        /// 商家审核
        /// zhoub 20150918
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AuditYH_MerchantInfo(YH_MerchantInfoModel model)
        {
            ResultModel result = new ResultModel();
            result.Data = _database.Db.YH_MerchantInfo.UpdateByMerchantID(MerchantID: model.MerchantID, AuditStatus: model.AuditStatus, AuditBy: model.AuditBy, AuditDT: model.AuditDT, AuditRemark: model.AuditRemark);
            if (result.Data > 0)
            {
                result.Messages.Add("Businesses Audit success.");
            }
            else
            {
                result.IsValid = false;
                result.Messages.Add("Businesses Audit Failure.");
            }
            return result;
        }
    }
}
