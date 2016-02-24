using HKTHMall.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simple.Data;
using HKTHMall.Domain.WebModel.Models.Login;

namespace HKTHMall.Services.WebLogin.Impl
{
    public class YH_PasswordErrorService : BaseService, IYH_PasswordErrorService
    {
        /// <summary>
        /// 根据用户ID 和密码类型获取密码错误信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="passwordType">密码类型</param>
        /// <returns></returns>
        public ResultModel GetPasswordErrorInfo(long userId, int passwordType)
        {
            return this.GetPasswordErrorInfoPrivate(userId, passwordType, this._database.Db);
        }


        /// <summary>
        /// 根据用户ID 和密码类型获取密码错误信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="passwordType">密码类型</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public ResultModel GetPasswordErrorInfo(long userId, int passwordType, dynamic trans)
        {
            return this.GetPasswordErrorInfoPrivate(userId, passwordType, trans);
        }

        /// <summary>
        /// 根据用户ID 和密码类型获取密码错误信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="passwordType">密码类型</param>
        /// <param name="db">数据Db</param>
        /// <returns></returns>
        private ResultModel GetPasswordErrorInfoPrivate(long userId, int passwordType, dynamic db)
        {
            var tb = db.YH_PasswordError;
            var where = new SimpleExpression(tb.UserID == userId, tb.PassWordType == passwordType, SimpleExpressionType.And);
            var result = new ResultModel()
            {
                Data = tb.FindAll(where).ToList<YH_PasswordErrorModel>()
            };
            return result;
        }


        /// <summary>
        /// 密码错误表插入
        /// </summary>
        /// <param name="model">密码错误对象</param>
        /// <returns></returns>
        public ResultModel AddError(YH_PasswordErrorModel model)
        {
            return this.AddErrorPrivate(model, this._database.Db);
        }


        /// <summary>
        /// 密码错误表插入
        /// </summary>
        /// <param name="model">密码错误对象</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public ResultModel AddError(YH_PasswordErrorModel model, dynamic trans)
        {
            return this.AddErrorPrivate(model, trans);
        }

        /// <summary>
        /// 密码错误表插入
        /// </summary>
        /// <param name="model">密码错误对象</param>
        /// <param name="db">数据Db</param>
        /// <returns></returns>
        private ResultModel AddErrorPrivate(YH_PasswordErrorModel model, dynamic db)
        {
            var tb = db.YH_PasswordError;
            var result = new ResultModel()
            {
                Data = tb.Insert(model)
            };
            return result;
        }


        /// <summary>
        /// 密码错误表更新
        /// </summary>
        /// <param name="model">密码错误对象</param>
        /// <returns></returns>
        public ResultModel Update(YH_PasswordErrorModel model)
        {
            return this.UpdatePrivate(model, _database.Db);
        }

        /// <summary>
        /// 密码错误表更新
        /// </summary>
        /// <param name="model">密码错误对象</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public ResultModel Update(YH_PasswordErrorModel model, dynamic trans)
        {
            return this.UpdatePrivate(model, trans);
        }

        /// <summary>
        /// 密码错误表更新
        /// </summary>
        /// <param name="model">密码错误对象</param>
        /// <param name="db">数据Db</param>
        /// <returns></returns>
        public ResultModel UpdatePrivate(YH_PasswordErrorModel model, dynamic db)
        {
            var result = new ResultModel() { Data = db.YH_PasswordError.Update(model) };
            return result;
        }
    }
}
