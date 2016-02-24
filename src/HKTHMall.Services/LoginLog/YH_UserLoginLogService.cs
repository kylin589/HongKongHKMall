using System;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.LoginLog;
using Simple.Data;

namespace HKTHMall.Services.LoginLog
{
    public class YH_UserLoginLogService : BaseService, IYH_UserLoginLogService
    {
        /// <summary>
        ///     添加用户登录日记
        /// </summary>
        /// <param name="model">用户登录日记模型</param>
        /// <returns>是否成功</returns>
        public ResultModel AddYH_UserLogin(YH_UserLoginLogModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.YH_UserLoginLog.Insert(model)
            };
            return result;
        }

        /// <summary>
        ///     根据用户登录日记id获取用户登录日记
        /// </summary>
        /// <param name="id">类别id</param>
        /// <returns>用户登录日记模型</returns>
        public ResultModel GetYH_UserLoginLogById(int ID)
        {
            var result = new ResultModel
            {
                Data = _database.Db.YH_UserLoginLog.Get(ID)
            };

            return result;
        }

        /// <summary>
        ///     获取用户登录日记列表
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>用户登录日记列表</returns>
        public ResultModel GetYH_UserLogin(SearchYH_UserLoginLogModel model)
        {
            var tb = _database.Db.YH_UserLoginLog;

            model.BeginLoginTime = model.BeginLoginTime == null ? DateTime.Now.AddDays(-30) : model.BeginLoginTime;

            //查询开始登陆时间
            var where = tb.LoginTime >= model.BeginLoginTime;

            if ( model.ID > 0)
            {
                //查询开始登陆时间
                where = new SimpleExpression(where, tb.ID == model.ID, SimpleExpressionType.And);
            }

            if (model.UserID != null)
            {
                //查询开始登陆时间
                where = new SimpleExpression(where, tb.UserID == model.UserID, SimpleExpressionType.And);
            }

            if (model.EndLoginTime != null)
            {
                //结束 时间加一天是为了查询结束当天的数据
                where = new SimpleExpression(where, tb.LoginTime < model.EndLoginTime, SimpleExpressionType.And);
            }

            
            dynamic pc;

            var query = tb
                .Query()
                .LeftJoin(_database.Db.YH_User, out pc)
                .On(_database.Db.YH_User.UserID == tb.UserID)
                .Select(
                    tb.UserID,
                    tb.LoginAddress,
                    tb.LoginTime,
                    tb.IP,
                    tb.LoginSource,
                    pc.Account
                )
                .Where(where)
                .OrderByLoginTimeDescending();

            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<YH_UserLoginLogModel>(query,
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        ///     更新用户登录日记
        /// </summary>
        /// <param name="model">用户登录日记模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateYH_UserLogin(YH_UserLoginLogModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.YH_UserLoginLog.Update(model)
            };

            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }

        /// <summary>
        ///     删除用户登录日记
        /// </summary>
        /// <param name="model">用户登录日记模型</param>
        /// <returns>是否删除成功</returns>
        public ResultModel DeleteYH_UserLogin(YH_UserLoginLogModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.YH_UserLoginLog.Delete(ID: model.ID)
            };

            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }

        /// <summary>
        /// 根据用户ID 获取上次登录信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultModel GetUserLoginInfo(long userId)
        {
            var result = new ResultModel
            {
                Data = _database.Db.YH_UserLoginLog.FindAllByUserID(userId).Take(1).OrderByDescending(_database.Db.YH_UserLoginLog.LoginTime).ToList<YH_UserLoginLogModel>()
            };
            return result;
        }
    }
}