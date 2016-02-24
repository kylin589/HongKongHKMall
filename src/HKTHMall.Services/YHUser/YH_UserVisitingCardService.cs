using System.Linq;
using BrCms.Framework.Collections;
using HKTHMall.Core;
using HKTHMall.Core.Data;
using HKTHMall.Domain.AdminModel.Models.YHUser;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Sys;
using HKTHMall.Domain.Models.YHUser;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Domain.WebModel.Models.Users;
using HKTHMall.Services.Common;
using HKTHMall.Services.Sys;
using HKTHMall.Services.WebLogin.Impl;
using Omise;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using Simple.Data.RawSql;
using HKTHMall.Domain.WebModel.Models.YH;

namespace HKTHMall.Services.YHUser
{
    public class YH_UserVisitingCardService : BaseService, IYH_UserVisitingCardService
    {


        /// <summary>
        /// 获取用户二维码信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ResultModel findByUserid(long userid)
        {
            var result = new ResultModel { Data = _database.Db.YH_UserVisitingCard.FindByUserID(userid) };
            return result;
        }
        /// <summary>
        /// 新增用户二维码信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>IsValid为false 表示新增失败或已存在二维码</returns>
        public ResultModel Add(dynamic model)//YH_UserVisitingCard  刘文宁 修改 2015/9/2
        {
            ResultModel result = new ResultModel();
            result.Data = null;

            var isExits = _database.Db.YH_UserVisitingCard.GetCount(_database.Db.YH_UserVisitingCard.UserID == model.UserID && _database.Db.YH_UserVisitingCard.NickName == model.NickName
                && _database.Db.YH_UserVisitingCard.Phone == model.Phone && _database.Db.YH_UserVisitingCard.HeadImgUrl == model.HeadImgUrl);
            if (isExits == 0)
            {
                result.Data = _database.Db.YH_UserVisitingCard.Insert(model);
            }
            else
            {
                result.IsValid = false;
            }

            return result;
        }
        /// <summary>
        /// 修改用户二维码信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns>IsValid为false</returns>
        public ResultModel Update(dynamic model)//YH_UserVisitingCard  刘文宁 修改 2015/9/2
        {
            ResultModel result = new ResultModel();
            result.Data = null;

            var isExits = _database.Db.YH_UserVisitingCard.GetCount(_database.Db.YH_UserVisitingCard.ID == model.ID);
            if (isExits == 0)
            {//不存在二维码
                result.IsValid = false;
                result.Messages = new List<string> { "NO UserVisitingCard" };
            }
            else
            {
                result.Data = _database.Db.YH_UserVisitingCard.Update(model);
            }

            return result;
        }
        /// <summary>
        /// 删除用户二维码信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        public ResultModel Delete(long userid)
        {
            var result = new ResultModel { Data = _database.Db.YH_UserVisitingCard.DeleteByUserID(userid) };
            return result;
        }
        /// <summary>
        /// 更新用户二维码生成状态
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="status">二维码生成状态</param>
        /// <returns></returns>
        public ResultModel UpdateStatus(long id, byte status)
        {
            var result = new ResultModel { Data = _database.Db.YH_UserVisitingCard.UpdateByID(ID: id, IsSuccess: status) };
            return result;

        }
    }
}
