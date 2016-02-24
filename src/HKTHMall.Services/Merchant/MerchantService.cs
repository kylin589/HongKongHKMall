using System.Collections.Generic;
using BrCms.Framework.Collections;
using Castle.Core.Internal;
using HKTHMall.Domain.AdminModel.Models.Merchant;
using HKTHMall.Domain.Models;

namespace HKTHMall.Services.Merchant
{
    public class MerchantService : BaseService, IMerchantService
    {
        public ResultModel Search(SearchMerchantModel model)
        {
            return this._database.RunQuery(db =>
            {
                var result = new ResultModel();

                var q = db.YH_MerchantInfo.Query()
                    .LeftJoin(db.YH_User, UserID: db.YH_MerchantInfo.MerchantID)
                    .LeftJoin(db.THArea_lang)
                    .On(db.THArea_lang.THAreaID == db.YH_MerchantInfo.AreaID &&
                        db.THArea_lang.LanguageID == model.LanguageId)
                    .Select(
                        db.YH_MerchantInfo.MerchantID,
                        db.YH_MerchantInfo.ShopName,
                        db.YH_MerchantInfo.MerchantType,
                        db.YH_MerchantInfo.LeasingManager,
                        db.YH_MerchantInfo.LeasingPhone,
                        db.YH_MerchantInfo.BusinessContacter,
                        db.YH_MerchantInfo.BusinessTel,
                        db.YH_MerchantInfo.CreateBy,
                        db.YH_MerchantInfo.CreateDT,
                        db.YH_MerchantInfo.AuditBy,
                        db.YH_MerchantInfo.AuditDT,
                        db.YH_MerchantInfo.AuditRemark
                        , db.YH_User.IsLock
                        , db.THArea_lang.AreaName
                    )
                    .OrderByDescending(db.YH_MerchantInfo.MerchantID);

                result.Data = new SimpleDataPagedList<MerchantModel>(q, model.PagedIndex, model.PagedSize);

                return result;
            });
        }

        public ResultModel Add(AddMerchantModel model)
        {
            return this._database.RunQuery(db =>
            {
                var result = new ResultModel();

                db.YH_MerchantInfo.Insert(model);

                return result;
            });
        }

        public ResultModel GetMerchantByMerchantId(long id)
        {
            return this._database.RunQuery(db => new ResultModel
            {
                Data = db.YH_MerchantInfo.Get(id)
            });
        }
        public ResultModel Update(UpdateMerchantModel model)
        {
            return this._database.RunQuery(db =>
            {
                var result = new ResultModel();
                db.YH_MerchantInfo.Update(model);
                return result;
            });
        }

        public ResultModel Delete(IList<long> ids)
        {
            return this._database.RunQuery(db =>
            {
                var result = new ResultModel();
                db.YH_MerchantInfo.DeleteAll(db.YH_MerchantInfo.MerchantID == ids);
                //ids.ForEach(x =>
                //{
                //    db.YH_MerchantInfo.DeleteById(db.YH_MerchantInfo.MerchantID == x);
                //});
                
                return result;
            });
        }
    }
}