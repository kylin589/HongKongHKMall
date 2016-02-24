using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.LoginLog;
using Simple.Data;

namespace HKTHMall.Services.LoginLog
{
    public class UserLoginLogService : BaseService, IUserLoginLogService
    {
        /// <summary>
        ///     添加用户登录日记
        /// </summary>
        /// <param name="model">用户登录日记模型</param>
        /// <returns>是否成功</returns>
        public ResultModel AddUserLoginLog(UserLoginLogModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.UserLoginLog.Insert(model)
            };

            return result;
        }

        /// <summary>
        ///     根据用户登录日记id获取用户登录日记
        /// </summary>
        /// <param name="id">类别id</param>
        /// <returns>用户登录日记模型</returns>
        public ResultModel GetUserLoginLogById(int ID)
        {
            var result = new ResultModel
            {
                Data = _database.Db.UserLoginLog.Get(ID)
            };

            return result;
        }

        /// <summary>
        ///     获取用户登录日记列表
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>用户登录日记列表</returns>
        public ResultModel GetUserLoginLog(SearchUserLoginLogModel model)
        {
            var tb = _database.Db.UserLoginLog;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal);

            if (!string.IsNullOrEmpty(model.UserName))
            {
                where = new SimpleExpression(where, tb.UserName.Like("%" + model.UserName.Trim() + "%"),
                    SimpleExpressionType.And); //会员名称
            }

            if (model.BeginLoginTime != null)
            {
                //查询开始登陆时间
                where = new SimpleExpression(where, tb.LoginTime >= model.BeginLoginTime, SimpleExpressionType.And);
            }

            if (model.EndLoginTime != null)
            {
                //结束 时间加一天是为了查询结束当天的数据
                where = new SimpleExpression(where, tb.LoginTime < model.EndLoginTime, SimpleExpressionType.And);
            }

            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<UserLoginLogModel>(_database.Db.UserLoginLog.FindAll(where).OrderByLoginTimeDescending(),
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        ///     更新用户登录日记
        /// </summary>
        /// <param name="model">用户登录日记模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateUserLoginLog(UserLoginLogModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.UserLoginLog.Update(model)
            };

            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }

        /// <summary>
        ///     删除用户登录日记
        /// </summary>
        /// <param name="model">用户登录日记模型</param>
        /// <returns>是否删除成功</returns>
        public ResultModel DeleteUserLoginLog(UserLoginLogModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.UserLoginLog.Delete(ID: model.ID)
            };

            result.IsValid = result.Data > 0 ? true : false;
            return result;
        }
    }
}