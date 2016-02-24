using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.AdminModel.Models.User;
using Simple.Data;
using System;


namespace HKTHMall.Services.Users
{
    public class YH_UserBankAccountService : BaseService, IYH_UserBankAccountService
    {
        /// <summary>
        /// 根据用户ID获取银行卡
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultModel GetUserBank(long userId)
        {



            var userBrank = _database.Db.YH_UserBankAccount;
            var result = new ResultModel();
            result.Data = userBrank.FindAll(userBrank.UserID == userId && userBrank.IsUse == 1).ToList<UserBankModel>();

            //var brank = _database.Db.BD_Bank;
            //dynamic bk;

            //var query = userBrank.All().
            //  LeftJoin(brank, out bk).On(brank.BankID == userBrank.BankID).
            //  Select(
            //  userBrank.ID,
            //  userBrank.BankID,
            //  userBrank.IsDefault,
            //  userBrank.BankAccount,
            //  userBrank.BankSubbranch,
            //  userBrank.BankUserName,
            //  userBrank.IsUse,
            //  userBrank.CreateBy,
            //  userBrank.CreateDT,
            //  bk.BankName,
            //  userBrank.BankAddress
            //  ).Where(userBrank.UserID == userId && userBrank.IsUse == 1);

            //var result = new ResultModel();
            //result.Data = query.ToList<YH_UserBankAccountModel>();
            return result;
        }


        /// <summary>
        /// 更新用户银行卡信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel UpdateModel(UserBankModel model)
        {
            var result = new ResultModel() { Data = base._database.Db.YH_UserBankAccount.Update(model)};
            return result;
        }

        /// <summary>
        /// 添加用户银行卡信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel Insert(UserBankModel model)
        {
            var result = new ResultModel() { Data = base._database.Db.YH_UserBankAccount.Insert(model) };
            return result;
        }

        /// <summary>
        /// 添加用户银行卡信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Insert(UserBankModel model,UserBankModel model2)
        {
            string status = "0";
            using (var tx = _database.Db.BeginTransaction())
            {
                try
                {
                    tx.YH_UserBankAccount.Insert(model);
                    tx.YH_UserBankAccount.Update(model2);
                    status = "1";
                    tx.Commit();                
                }
                catch (Exception ex)
                {
                    status = "0";
                    tx.Rollback();               
                }
            }
            return status;
        }


        /// <summary>
        /// 通过Id查询用户银行帐户信息表对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        public ResultModel GetYH_UserBankAccountById(int id)
        {
            var result = new ResultModel() { Data = base._database.Db.YH_UserBankAccount.FindByID(id) };
            return result;
        }

        /// <summary>
        /// 用户银行帐户信息表对象分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        public ResultModel Select(SearchYH_UserBankAccountModel model)
        {
            var userBrank = _database.Db.YH_UserBankAccount;
            var user = _database.Db.YH_User;
            var brank = _database.Db.BD_Bank;
            dynamic u;
            dynamic bk;

            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            if (!string.IsNullOrEmpty(model.Account))
            {
                whereParam = new SimpleExpression(whereParam, user.Account.Like("%" + model.Account + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.Phone))
            {
                whereParam = new SimpleExpression(whereParam, user.Phone.Like("%" + model.Phone + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                whereParam = new SimpleExpression(whereParam, user.Email.Like("%" + model.Email + "%"), SimpleExpressionType.And);
            }

            var query = userBrank.All().
                LeftJoin(user, out u).On(userBrank.UserID == u.UserID).
                LeftJoin(brank, out bk).On(brank.BankID == userBrank.BankID).
                Select(
                userBrank.ID,
                userBrank.IsDefault,
                userBrank.BankAccount,
                userBrank.BankSubbranch,
                userBrank.BankUserName,
                userBrank.IsUse,
                userBrank.CreateBy,
                userBrank.CreateDT,
                u.Account,
                u.RealName,
                u.Phone,
                u.Email,
                bk.BankName
                ).Where(whereParam).
                OrderBy(userBrank.IsUse);
             
            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<YH_UserBankAccountModel>(query,
                            model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 通过Id删除用户银行帐户信息表
        /// </summary>
        /// <param name="id">用户银行帐户信息表id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        public ResultModel Delete(int id)
        {
            var result = new ResultModel() { Data = base._database.Db.YH_UserBankAccount.DeleteByID(id) };
            return result;
        }


        /// <summary>
        /// 更新用户银行帐户信息表
        /// </summary>
        /// <param name="model">用户银行帐户信息表对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        public ResultModel Update(YH_UserBankAccountModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
