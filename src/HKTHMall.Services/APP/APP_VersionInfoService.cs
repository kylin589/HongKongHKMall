using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.APP;
using HKTHMall.Domain.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.APP
{
    public class APP_VersionInfoService : BaseService, IAPP_VersionInfoService
    {
        /// <summary>
        /// 添加APP版本信息表
        /// </summary>
        /// <param name="model">用APP版本信息表模型</param>
        /// <returns>是否成功</returns>

        public ResultModel AddAPP_VersionInfo(APP_VersionInfoModel model)
        {
            var result = new ResultModel()
            {
                Data = base._database.Db.APP_VersionInfo.Insert(model)
                
            }; 
            return result;
        }

        /// <summary>
        /// 获取APP版本信息表
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        public ResultModel GetAPP_VersionInfoList(SearchAPP_VersionInfoModel model)
        {
            var tb = _database.Db.APP_VersionInfo;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal); //

            if (model.ID > 0)
            {
                //
                where = new SimpleExpression(where, tb.ID == model.ID, SimpleExpressionType.And);
            }

            if (!string.IsNullOrEmpty(model.APPName))
            {
                //
                where = new SimpleExpression(where, tb.APPName.Like("%" + model.APPName.Trim() + "%"), SimpleExpressionType.And);
            }
            if (model.APPTypeID>0)
            {
                //
                where = new SimpleExpression(where, tb.APPTypeID == model.APPTypeID, SimpleExpressionType.And);
            }
            
            if (!string.IsNullOrEmpty(model.PackageName))
            {
                //
                where = new SimpleExpression(where, tb.PackageName.Like("%" + model.PackageName.Trim() + "%"), SimpleExpressionType.And);
            }
            if (model.Platform > 0)
            {
                //
                where = new SimpleExpression(where, tb.Platform == model.Platform, SimpleExpressionType.And);
            }

            if (!string.IsNullOrEmpty(model.VersionName))
            {
                //
                where = new SimpleExpression(where, tb.PackageName.Like("%" + model.VersionName.Trim() + "%"), SimpleExpressionType.And);
            }

            if (!string.IsNullOrEmpty(model.VersionNO))
            {
                //
                where = new SimpleExpression(where, tb.PackageName.Like("%" + model.VersionNO.Trim() + "%"), SimpleExpressionType.And);
            }
            
            
            if (model.BeginCreateDT !=null)
            {
                // 开始时间 
                where = new SimpleExpression(where, tb.CreateDT >= model.BeginCreateDT, SimpleExpressionType.And);
            }
            if (model.EndCreateDT != null )
            {
                // 结束时间 
                where = new SimpleExpression(where, tb.CreateDT < model.EndCreateDT, SimpleExpressionType.And);
            }
           

            //dynamic pc;

            var query = tb
                .Query()

                //.LeftJoin(_database.Db.YH_User, out pc)
                //.On(_database.Db.YH_User.UserID == tb.UserID)
                .Select(
                    tb.ID,
                    tb.APPName,
                    tb.DownloadURL,
                    tb.Platform,
                    tb.PackageName,
                    tb.VersionNO,
                    tb.VersionName,
                    tb.FileSize,

                    tb.UpdateInfo,
                    tb.UpdateInfoEN,
                    tb.UpdateInfoTH,
                    tb.IsForceUpdate,
                    tb.CreateBy,
                    tb.CreateDT,
                    tb.UpdateBy,
                    tb.UpdateDT
                )
                .Where(where)
                .OrderByCreateDTDescending();

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<APP_VersionInfoModel>(query,
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 更新APP_VersionInfo
        /// </summary>
        /// <param name="model">APP_VersionInfo模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateAPP_VersionInfo(APP_VersionInfoModel model)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.APP_VersionInfo.UpdateByID(ID: model.ID, APPName: model.APPName, Platform: model.Platform, VersionNO: model.VersionNO, VersionName: model.VersionName, UpdateInfo: model.UpdateInfo, UpdateInfoTH: model.UpdateInfoTH, UpdateInfoEN: model.UpdateInfoEN, PackageName: model.PackageName, DownloadURL: model.DownloadURL, FileSize: model.FileSize, IsForceUpdate: model.IsForceUpdate, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT)
            };

            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// 删除APP_VersionInfo
        /// </summary>
        /// <param name="model">APP_VersionInfo模型</param>
        /// <returns>是否删除成功</returns>
        public ResultModel DeleteAPP_VersionInfo(APP_VersionInfoModel model)
        {

            var result = new ResultModel()
            {
                Data = base._database.Db.APP_VersionInfo.Delete(ID: model.ID)
            };

            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }
    }
}
