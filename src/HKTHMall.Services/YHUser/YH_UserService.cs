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

using YH_UserModel = HKTHMall.Domain.Models.YHUser.YH_UserModel;
using HKSJ.Common.FastDFS;
using HKTHMall.Services.Common.MultiLangKeys;

namespace HKTHMall.Services.YHUser
{
    public class YH_UserService : BaseService, IYH_UserService
    {

        private ParameterSetService parameterSetService;

        private YH_PasswordErrorService passwordErrorService;

        public YH_UserService(ParameterSetService parameterSetService, YH_PasswordErrorService passwordErrorService)
        {
            this.parameterSetService = parameterSetService;
            this.passwordErrorService = passwordErrorService;
        }

        #region 商城用户

        /// <summary>
        /// 分页获取商城用户列表
        /// </summary>
        /// <param name="model">搜索模型</param>
        /// <returns>列表数据</returns>
        public ResultModel GetPagingYH_User(SearchYHUserModel model)
        {
            var tb = _database.Db.YH_User;
            var whereExpr = tb.Account.Like("%" + (model.Account != null ? model.Account.Trim() : model.Account) + "%");

            if (model.RealName != null)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.RealName.Like("%" + model.RealName.Trim() + "%"), SimpleExpressionType.And);
            }

            if (model.Phone != null)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.Phone.Like("%" + model.Phone.Trim() + "%"), SimpleExpressionType.And);
            }

            if (model.IsLock != -1)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.IsLock == model.IsLock, SimpleExpressionType.And);
            }
            if (model.RegisterDateBegin != null)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.RegisterDate >= model.RegisterDateBegin, SimpleExpressionType.And);
            }
            if (model.RegisterDateEnd != null)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.RegisterDate < Convert.ToDateTime(model.RegisterDateEnd).AddDays(1), SimpleExpressionType.And);
            }

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<YH_UserModel>(_database.Db.YH_User.All().Where(whereExpr), model.PagedIndex, model.PagedSize)
            };

            return result;
        }
        /// <summary>
        /// 获取商城用户列表
        /// </summary>     
        /// <returns>列表数据</returns>
        public int[] GetAllUserId()
        {
            var tb = _database.Db.YH_User;

            var whereExpr = new SimpleExpression(1, 1, SimpleExpressionType.And);
            whereExpr = new SimpleExpression(whereExpr, _database.Db.YH_User.IsDelete == 1, SimpleExpressionType.And);
            whereExpr = new SimpleExpression(whereExpr, _database.Db.YH_User.UserID > 0, SimpleExpressionType.And);
            var query = tb.All().Select(
                tb.UserID)
            .Where(whereExpr);

            return null;
        }
        /// <summary>
        /// 删除用户
        /// zhoub 20150714
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ResultModel DeleteYH_UserByUserID(YH_UserModel model)
        {
            var result = new ResultModel();
            result.Data = _database.Db.YH_User.UpdateByUserID(UserID: model.UserID, IsDelete: model.IsDelete, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);
            return result;

        }

        /// <summary>
        /// 更新用户锁定状态
        /// zhoub 20150714
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public ResultModel UpdateYH_UserIsLock(YH_UserModel model)
        {
            var result = new ResultModel();
            result.Data = _database.Db.YH_User.UpdateByUserID(UserID: model.UserID, IsLock: model.IsLock, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);
            return result;
        }

        /// <summary>
        /// 重置用户登录密码
        /// zhoub 20150714
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public ResultModel UpdateYH_UserPassWord(YH_UserModel model)
        {
            var result = new ResultModel();
            using (var bt = _database.Db.BeginTransaction())
            {
                try
                {
                    result.Data = _database.Db.YH_User.UpdateByUserID(UserID: model.UserID, PassWord: model.PassWord, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);
                    //重置错误次数 
                    _database.Db.YH_PasswordError.UpdateAll(VerifyTime: DateTime.Now, FailVerifyTimes: 0, Condition: (_database.Db.YH_PasswordError.UserID == model.UserID && _database.Db.YH_PasswordError.PassWordType == 1));
                    //重置帐号系统登录密码
               
                        int count = _database.Db.YH_UserUpdateInfo.GetCount(_database.Db.YH_UserUpdateInfo.UserID == model.UserID);
                        if (count == 0)
                        {
                            _database.Db.YH_UserUpdateInfo.Insert(UserID: model.UserID, TimeStamp: 1);
                        }
                        else
                        {
                            _database.Db.YH_UserUpdateInfo.UpdateByUserID(UserID: model.UserID, TimeStamp: 1);
                        }
                        bt.Commit();
                }
                catch (Exception e)
                {
                    result.Data = 0;
                    bt.Rollback();
                }
            }
            return result;
        }

        /// <summary>
        /// 重置用户交易密码
        /// zhoub 20150714
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel UpdateYH_UserPayPassWord(YH_UserModel model)
        {
            var result = new ResultModel();
            result.Data = _database.Db.YH_User.UpdateByUserID(UserID: model.UserID, PayPassWord: model.PayPassWord, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT);
            //重置错误次数 
            _database.Db.YH_PasswordError.UpdateAll(VerifyTime: DateTime.Now, FailVerifyTimes: 0, Condition: (_database.Db.YH_PasswordError.UserID == model.UserID && _database.Db.YH_PasswordError.PassWordType == 2));
            return result;
        }

        /// <summary>
        /// 感恩惠粉人数获取
        /// zhoub 20150715
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultModel GetYH_UserReferrerIDCount(long userId)
        {
            var result = new ResultModel();
            result.Data = _database.Db.YH_User.GetCount(_database.Db.YH_User.ReferrerID == userId);
            return result;
        }


        /// <summary>
        /// 根据用户ID获取消费金额、收益金额
        /// zhoub 20150826
        /// </summary>
        /// <param name="type">1 消费金额 2 收益金额</param>
        /// <returns></returns>
        public ResultModel GetYH_UserMoney(long userID, int type)
        {
            List<int> list = new List<int>();
            if (type == 1)
            {
                list.Add(2);
            }
            else
            {
                list.Add(6);
                list.Add(7);
                list.Add(8);
                list.Add(9);
                list.Add(10);
                list.Add(11);
                list.Add(12);
                list.Add(13);
                list.Add(14);
            }
            SimpleExpression whereExpr = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            whereExpr = new SimpleExpression(whereExpr, new SimpleExpression(_database.Db.ZJ_UserBalanceChangeLog.AddOrCutType, list.ToArray(), SimpleExpressionType.Equal), SimpleExpressionType.And);
            whereExpr = new SimpleExpression(whereExpr, new SimpleExpression(_database.Db.ZJ_UserBalanceChangeLog.UserID, userID, SimpleExpressionType.Equal), SimpleExpressionType.And);
            var result = new ResultModel();
            result.Data = _database.Db.ZJ_UserBalanceChangeLog.All().Select(_database.Db.ZJ_UserBalanceChangeLog.AddOrCutAmount.Sum()).Where(whereExpr).ToScalarOrDefault();
            return result;
        }

        #endregion

        #region 余额变动记录

        /// <summary>
        /// 分页余额变动记录
        /// </summary>
        /// <param name="model">搜索模型</param>
        /// <returns>列表数据</returns>
        public ResultModel GetPagingZJ_UserBalanceChangeLog(SearchUserBalanceChangeModel model)
        {
            var tb = _database.Db.ZJ_UserBalanceChangeLog;
            var whereExpr = _database.Db.YH_User.Account.Like("%" + model.Account + "%");

            if (model.UserID != null)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.UserID == model.UserID, SimpleExpressionType.And);
            }
            if (model.ID != null)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.ID == model.ID, SimpleExpressionType.And);
            }

            if (model.RealName != null)
            {
                whereExpr = new SimpleExpression(whereExpr, _database.Db.YH_User.RealName.Like("%" + model.RealName + "%"), SimpleExpressionType.And);
            }

            if (model.IsAddOrCut != -1)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.IsAddOrCut == model.IsAddOrCut, SimpleExpressionType.And);
            }

            if (model.AddOrCutType != null && model.AddOrCutType.Value != 0)//modified by jimmy,2015-8-29
            {
                whereExpr = new SimpleExpression(whereExpr, tb.AddOrCutType == model.AddOrCutType, SimpleExpressionType.And);
            }
            if (model.CreateBy != null)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.CreateBy.Like("%" + model.CreateBy + "%"), SimpleExpressionType.And);
            }

            if (model.OrderNo != null)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.OrderNo.Like("%" + model.OrderNo + "%"), SimpleExpressionType.And);
            }
            if (model.CreateDTBegin != null)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.CreateDT >= model.CreateDTBegin, SimpleExpressionType.And);
            }
            if (model.CreateDTEnd != null)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.CreateDT < Convert.ToDateTime(model.CreateDTEnd).AddDays(1), SimpleExpressionType.And);
            }

            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<UserBalanceChange>(_database.Db.ZJ_UserBalanceChangeLog.All().LeftJoin(_database.Db.YH_User).On(_database.Db.YH_User.UserID == _database.Db.ZJ_UserBalanceChangeLog.UserID).Select(_database.Db.ZJ_UserBalanceChangeLog.ID, _database.Db.ZJ_UserBalanceChangeLog.UserID, _database.Db.ZJ_UserBalanceChangeLog.AddOrCutAmount, _database.Db.ZJ_UserBalanceChangeLog.IsAddOrCut, _database.Db.ZJ_UserBalanceChangeLog.OldAmount, _database.Db.ZJ_UserBalanceChangeLog.NewAmount, _database.Db.ZJ_UserBalanceChangeLog.AddOrCutType, _database.Db.ZJ_UserBalanceChangeLog.OrderNo, _database.Db.ZJ_UserBalanceChangeLog.Remark, _database.Db.ZJ_UserBalanceChangeLog.IsDisplay, _database.Db.ZJ_UserBalanceChangeLog.CreateBy, _database.Db.ZJ_UserBalanceChangeLog.CreateDT, _database.Db.YH_User.Account, _database.Db.YH_User.RealName).Where(whereExpr), model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        #endregion

        #region 获取用户信息
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <param name="lang">1:中文,2:英文,3:泰文</param>
        /// <returns></returns>
        //public ResultModel GetUserInfo(long userid, int lang)
        //{
        //    var result = new ResultModel();
        //    try
        //    {
        //        //查询账号系统此用户
        //        long[] uids = new long[] { userid };
        //        ResponseUserInfoData response = EmMethodManage.EmFindUpdateInstance.MsgBatchGetInfoByIdReq(uids);
        //        if (response.isOK == false)
        //        {
        //            string err = CultureHelper.GetIMErrorLangString(response.Status);
        //            if (string.IsNullOrEmpty(err))
        //            {
        //                result.IsValid = false;
        //                result.Messages.Add(CultureHelper.GetIMLangString(response.Status, lang));
        //            }
        //            else
        //            {
        //                result.IsValid = true;
        //                result.Messages.Add(err);
        //            }
        //            return result;
        //        }
        //        if (response != null)
        //        {
        //            if (response.data != null)
        //            {
        //                if (response.data.Count > 0)
        //                {
        //                    //用户信息
        //                    var pc = _database.Db.YH_User;
        //                    dynamic tb, at;

        //                    result.IsValid = true;
        //                    var datatb = pc
        //                     .Query()
        //                     .LeftJoin(_database.Db.YH_Agent, out at)
        //                     .On(pc.UserID == at.UserID)
        //                     .LeftJoin(_database.Db.ZJ_UserBalance, out tb)
        //                     .On(pc.UserID == tb.UserID)
        //                     .Select(
        //                         tb.ConsumeBalance.As("balance"),//余额
        //                         at.AgentType,

        //                         pc.UserID,
        //                         pc.Account,
        //                         pc.PayPassWord.As("isPayPassWord"),
        //                         pc.NickName,
        //                         pc.HeadImageUrl.As("imageUrl"),
        //                         pc.Sex,
        //                         pc.RealName,
        //                         pc.Email,
        //                         pc.Phone,
        //                         pc.signature,
        //                         pc.OrcodeUrl,
        //                         pc.Level,
        //                         pc.Birthday,
        //                         pc.tHAreaID,
        //                         pc.UserType
        //                     )
        //                     .Where(pc.UserID == userid).ToList<YH_UserInfoModel>();


        //                    List<YH_UserInfoModel> _YH_UserInfoModel = datatb as List<YH_UserInfoModel>;

        //                    if (_YH_UserInfoModel != null)
        //                    {
        //                        if (_YH_UserInfoModel.Count > 0)
        //                        {
        //                            //常驻地址 
        //                            if (_YH_UserInfoModel[0].tHAreaID == 9)//惠粉公众号专属数字 9
        //                            {
        //                                switch (lang)
        //                                {
        //                                    case 1:
        //                                        _YH_UserInfoModel[0].address = "曼谷 泰国";//中文
        //                                        break;
        //                                    case 2:
        //                                        _YH_UserInfoModel[0].address = "Bangkok Thailand";//英文
        //                                        break;
        //                                    case 3:
        //                                        _YH_UserInfoModel[0].address = "กรุงเทพฯ ประเทศไทย";//太文
        //                                        break;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                List<string> lstArea = GetAddress(_YH_UserInfoModel[0].tHAreaID, lang);

        //                                foreach (string area in lstArea)
        //                                {
        //                                    _YH_UserInfoModel[0].address += " " + area;
        //                                }
        //                            }
        //                            //累计消费收益
        //                            StringBuilder strSql = new StringBuilder();
        //                            strSql.Append(" SELECT SUM(AddOrCutAmount) amount FROM ZJ_UserBalanceChangeLog ");
        //                            strSql.AppendFormat(" WHERE UserID={0} AND IsDisplay=1  and IsAddOrCut=1 AND ", userid);
        //                            strSql.Append(" AddOrCutType IN (6,7,8,9,10,11,12);");
        //                            //执行sql
        //                            var data = _database.RunSqlQuery(x => x.ToResultSets(strSql.ToString()));
        //                            if (data[0][0].amount == null)
        //                            {
        //                                _YH_UserInfoModel[0].consumptionIncome = 0M;
        //                            }
        //                            else
        //                            {
        //                                _YH_UserInfoModel[0].consumptionIncome = data[0][0].amount;
        //                            }

        //                            //待评价数
        //                            var ordertb = _database.Db.OrderDetails;
        //                            dynamic orderpc;
        //                            var query = ordertb
        //                                .Query()
        //                                .LeftJoin(_database.Db.Order, out orderpc)
        //                                .On(orderpc.OrderID == ordertb.OrderID)
        //                                .Select(
        //                                    orderpc.OrderID
        //                                )
        //                                .Where(_database.Db.OrderDetails.Iscomment == 0).Where(orderpc.UserID == userid).Where(orderpc.OrderStatus == 5 || orderpc.OrderStatus == 6).ToList().Count;

        //                            _YH_UserInfoModel[0].evaluationOfStay = query;
        //                            //头像
        //                            if (!string.IsNullOrEmpty(_YH_UserInfoModel[0].imageUrl))
        //                            {
        //                                _YH_UserInfoModel[0].imageUrl = (ConfigurationManager.AppSettings["ImageHeader"] + _YH_UserInfoModel[0].imageUrl);
        //                            }
        //                            //二维码
        //                            if (!string.IsNullOrEmpty(_YH_UserInfoModel[0].orcodeUrl))
        //                            {
        //                                _YH_UserInfoModel[0].orcodeUrl = (ConfigurationManager.AppSettings["ImagePath"] + _YH_UserInfoModel[0].orcodeUrl);
        //                            }
        //                            //生日
        //                            if (_YH_UserInfoModel[0].birthday != null)
        //                            {

        //                                _YH_UserInfoModel[0].birthday = ConvertsTime.DateTimeToTimeStamp(_YH_UserInfoModel[0].birthday).ToString();
        //                            }
        //                            //密码
        //                            if (!string.IsNullOrEmpty(_YH_UserInfoModel[0].isPayPassWord))
        //                            {
        //                                _YH_UserInfoModel[0].isPayPassWord = "1";//设置支付密码
        //                            }
        //                            else
        //                            {
        //                                _YH_UserInfoModel[0].isPayPassWord = "0";//未设置支付密码
        //                            }
        //                            //待付款 
        //                            _YH_UserInfoModel[0].obligation = _database.Db.Order.GetCount(_database.Db.Order.OrderStatus == 2 && _database.Db.Order.UserID == userid);
        //                            //待发货 
        //                            _YH_UserInfoModel[0].toBeShipped = _database.Db.Order.GetCount(_database.Db.Order.OrderStatus == 3 && _database.Db.Order.UserID == userid);
        //                            //待收货  
        //                            _YH_UserInfoModel[0].incomingGoods = _database.Db.Order.GetCount(_database.Db.Order.OrderStatus == 4 && _database.Db.Order.UserID == userid);
        //                            result.Data = _YH_UserInfoModel[0];
        //                        }
        //                        else
        //                        {
        //                            result.IsValid = true;
        //                            result.Messages.Add(CultureHelper.GetAPPLangSgring("NO_DATA", lang));
        //                        }

        //                    }
        //                    else
        //                    {
        //                        result.IsValid = true;
        //                        result.Messages.Add(CultureHelper.GetAPPLangSgring("NO_DATA", lang));
        //                    }
        //                }
        //                else
        //                {
        //                    result.IsValid = false;
        //                    result.Messages.Add(CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", lang));
        //                }
        //            }
        //            else
        //            {
        //                result.IsValid = false;
        //                result.Messages.Add(CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", lang));
        //            }
        //        }
        //        else
        //        {
        //            result.IsValid = false;
        //            result.Messages.Add(CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", lang));
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        result.IsValid = false;
        //        result.Messages.Add(CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", lang));
        //    }
        //    return result;
        //}
        private List<string> lstAddares = new List<string>();
        private List<string> GetAddress(int tHAreaID, int lang)
        {
            //常驻地址
            var tb = _database.Db.THArea;
            dynamic al;
            var tbTHArea = tb
                .Query()
                .LeftJoin(_database.Db.THArea_lang, out al)
                .On(tb.THAreaID == al.THAreaID)
                .Select(tb.ParentID, al.AreaName)
                .Where(tb.THAreaID == tHAreaID && al.LanguageID == lang).ToList();
            if (tbTHArea.Count > 0)
            {
                lstAddares.Add(tbTHArea[0].AreaName);
                if (tbTHArea[0].ParentID != 0)
                {
                    GetAddress(tbTHArea[0].ParentID, lang);
                }
            }
            return lstAddares;

        }
        #endregion

        #region 获取数据库表用户信息
        /// <summary>
        /// 获取数据库表用户信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public ResultModel GetUserInfoByUserId(long userid)
        {
            var result = new ResultModel();
            result.Data = _database.Db.YH_User.FindAll(_database.Db.YH_User.UserID == userid).ToList<YH_UserModel>();
            return result;
        }
        #endregion

        #region 变更用户信息
        
        /// <summary>
        /// 激活邮箱
        /// </summary>
        /// <returns></returns>
        public ResultModel ActiveEmail(string email)
        {
            
            var result = new ResultModel();
            try
            {
                result.IsValid = false;
                if (!string.IsNullOrEmpty(email))
                {
                    //更新常驻地址
                    result.IsValid = true;
                    _database.Db.YH_User.UpdateByEmail(Email: email, ActiveEmail: 1, UpdateDT: DateTime.Now, UpdateBy: "用户");//更新数据库 
                }
                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Messages.Add(CultureHelper.GetLangString("UPDATE_Failure") + ":" + ex.Message);
            }
            return result;
        }
       
       
        ///// <summary>
        ///// 更新用户信息
        ///// </summary>
        ///// <param name="model">用户信息对象</param>
        ///// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        //private ResultModel UpdateYH_User(YH_UserInfoModel model, int lang)
        //{
        //    var result = new ResultModel();
        //    try
        //    {
        //        RequestUserInfo user = new RequestUserInfo();
        //        result.IsValid = true;
        //        //更新用户资料 
        //        decimal dt;
        //        if (model.nickName != null)
        //        {
        //            user.nick = model.nickName;//更新昵称 
        //            result=UpdateUserInfo(user, model.userId, lang);
        //            if (result.IsValid)
        //            {
        //                dt = _database.Db.YH_User.UpdateByUserID(UserID: model.userId, NickName: model.nickName, UpdateDT: DateTime.Now, UpdateBy: "用户");//更新数据库 
        //            }
           
        //        }
        //        if (model.imageUrl != null)
        //        {
        //            user.img_url = model.imageUrl;//更新头像 
        //            result = UpdateUserInfo(user, model.userId, lang);
        //            if (result.IsValid)
        //            {
        //                dt = _database.Db.YH_User.UpdateByUserID(UserID: model.userId, HeadImageUrl: model.imageUrl, UpdateDT: DateTime.Now, UpdateBy: "用户");//更新数据库
        //            }
        //        }
        //        if (model.sex != null)
        //        {
        //            int sex;
        //            int.TryParse(model.sex, out sex);//更新性别   
        //            user.sex = sex;
        //            result = UpdateUserInfo(user, model.userId, lang);
        //            if (result.IsValid)
        //            {
        //                dt = _database.Db.YH_User.UpdateByUserID(UserID: model.userId, Sex: model.sex, UpdateDT: DateTime.Now, UpdateBy: "用户");//更新数据库
        //            }
        //        }
        //        if (model.birthday != null)//更新生日
        //        {
        //            dt = _database.Db.YH_User.UpdateByUserID(UserID: model.userId, Birthday: ConvertsTime.TimeStampToDateTime(long.Parse(model.birthday)), UpdateDT: DateTime.Now, UpdateBy: "用户");//更新数据库
        //        }
        //        if (model.signature != null)//更新签名
        //        {
        //            user.sign = model.signature;
        //            result = UpdateUserInfo(user, model.userId, lang);
        //            if (result.IsValid)
        //            {
        //                dt = _database.Db.YH_User.UpdateByUserID(UserID: model.userId, signature: model.signature, UpdateDT: DateTime.Now, UpdateBy: "用户");//更新数据库
        //            }
        //        }
        //        if (model.orcodeUrl != null)//更新二维码
        //        {
        //            List<YH_UserModel> lst = _database.Db.YH_User.FindAll(_database.Db.YH_User.UserID == model.userId).ToList<YH_UserModel>();
        //            if (lst != null)
        //            {
        //                if (lst.Count > 0)
        //                {
        //                    if (UrlHelp.UrlIsExist(ConfigurationManager.AppSettings["ImagePath"] + lst[0].OrcodeUrl))
        //                    {
        //                        FastDFSClient.RemoveFile(FastDFSClient.DefaultGroup.GroupName, lst[0].OrcodeUrl);//删除旧二维码
        //                    }
        //                }
        //            }
        //            dt = _database.Db.YH_User.UpdateByUserID(UserID: model.userId, OrcodeUrl: model.orcodeUrl, UpdateDT: DateTime.Now, UpdateBy: "用户");//更新数据库 
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        result.IsValid = false;
        //        result.Messages.Add(CultureHelper.GetAPPLangSgring("UPDATE_Failure", lang) + ":" + ex.Message);
        //    }
        //    return result;
        //}
        #endregion


        #region 与支付相关

        /// <summary>
        /// 获取用户信息（用于支付,配合GetYH_UserForPayment方法使用）
        /// </summary>
        /// <param name="userInfoView">用户信息</param>
        /// <param name="compareUserInfoView">用于比较的用户信息</param>
        /// <returns>是否可以支付</returns>
        public ResultModel GetYH_UserForPaymentMessage(UserInfoViewForPayment userInfoView, UserInfoViewForPayment compareUserInfoView)
        {
            return this.GetYH_UserForPaymentMessagePrivate(userInfoView, compareUserInfoView, this._database.Db);
        }


        /// <summary>
        /// 获取用户信息（用于支付,配合GetYH_UserForPayment方法使用）
        /// </summary>
        /// <param name="userInfoView">用户信息</param>
        /// <param name="compareUserInfoView">用于比较的用户信息</param>
        /// <param name="trans">事务对象</param>
        /// <returns>是否可以支付</returns>
        public ResultModel GetYH_UserForPaymentMessage(UserInfoViewForPayment userInfoView,
            UserInfoViewForPayment compareUserInfoView, dynamic trans)
        {
            return this.GetYH_UserForPaymentMessagePrivate(userInfoView, compareUserInfoView, trans);
        }

        /// <summary>
        /// 获取用户信息（用于支付,配合GetYH_UserForPayment方法使用）
        /// </summary>
        /// <param name="userInfoView">用户信息</param>
        /// <param name="compareUserInfoView">用于比较的用户信息</param>
        /// <returns>是否可以支付</returns>
        private ResultModel GetYH_UserForPaymentMessagePrivate(UserInfoViewForPayment userInfoView,
            UserInfoViewForPayment compareUserInfoView, dynamic db)
        {
            ResultModel resultModel = new ResultModel()
            {
                IsValid = false

            };

            if (userInfoView == null || userInfoView.IsDelete == 1)
            {
                //没有找到该用户
                resultModel.Messages.Add(CultureHelper.GetAPPLangSgring("USER_QUERY_NOT_FOUND", compareUserInfoView.LanguageId));
            }
            else
            {
                if (userInfoView.IsLock == 1)
                {
                    //账户被锁定,请联系客服
                    resultModel.Messages.Add(CultureHelper.GetAPPLangSgring("USER_QUERY_ACCOUNT_LOCKED", compareUserInfoView.LanguageId));
                }
                //进行密码检测
                else if (compareUserInfoView != null)
                {
                    if (string.IsNullOrEmpty(userInfoView.PayPassWord))
                    {

                        resultModel.Messages.Add(CultureHelper.GetAPPLangSgring("MONEY_ORDER_SETPAYPASSWORD", compareUserInfoView.LanguageId));
                    }
                    else
                    {
                        int passwordType = (int)UserEnums.PasswordType.PayPassword;

                        //查看是否有历史输入错误记录

                        List<YH_PasswordErrorModel> passwordErrors =
                            passwordErrorService.GetPasswordErrorInfo(userInfoView.UserID, passwordType, db).Data;

                        //交易密码错误信息
                        YH_PasswordErrorModel passwordErrorModel = null;
                        if (passwordErrors == null || passwordErrors.Count == 0)
                        {

                            //如果用户交易密码不同数据库交易密码
                            if (compareUserInfoView.PayPassWord != userInfoView.PayPassWord)
                            {
                                passwordErrorModel = new YH_PasswordErrorModel()
                                {
                                    Account = userInfoView.Account,
                                    UserID = userInfoView.UserID,
                                    VerifyTime = DateTime.Now,
                                    PassWordType = passwordType,
                                    FailVerifyTimes = 1

                                };
                                passwordErrorService.AddError(passwordErrorModel, db);
                                resultModel.Messages.Add(CultureHelper.GetAPPLangSgring("MONEY_ORDER_PAYPASSWORD_ERROR", compareUserInfoView.LanguageId));

                            }
                            else
                            {
                                resultModel.IsValid = true;
                            }

                        }
                        else
                        {


                            passwordErrorModel = passwordErrors[0];

                            //获取有关交易密码限制的参数
                            List<ParameterSetModel> payPwdParams =
                                parameterSetService.GetParameterSetsBy(new long[]
                                {
                                    ParameterSetExtension.PARAM_PAYPASSWORD_TIME, ParameterSetExtension.PARAM_PAYPASSWORD_COUNT
                                }, db).Data;

                            //次数限制参数
                            ParameterSetModel payPwdCount =
                                payPwdParams.FirstOrDefault(
                                    x => x.ParamenterID == ParameterSetExtension.PARAM_PAYPASSWORD_COUNT);

                            //时间限制参数
                            ParameterSetModel payPwdTime =
                                payPwdParams.FirstOrDefault(
                                    x => x.ParamenterID == ParameterSetExtension.PARAM_PAYPASSWORD_TIME);

                            //限制次数
                            int count = payPwdCount == null ? 5 : Convert.ToInt32(payPwdCount.PValue);

                            //限制时间（分钟）
                            double time = payPwdTime == null ? 120 : Convert.ToDouble(payPwdTime.PValue);

                            //限制时间
                            DateTime limitTime = passwordErrorModel.VerifyTime.AddMinutes(time);

                            //交易密码是否正确
                            bool isPwdTrue = compareUserInfoView.PayPassWord == userInfoView.PayPassWord;

                            //错误次数等于限制次数 并且当前时间小于限制时间 则不允许进行交易密码验证
                            if (passwordErrorModel.FailVerifyTimes >= count
                                && limitTime > DateTime.Now)
                            {
                                resultModel.Messages.Add(string.Format(CultureHelper.GetAPPLangSgring("MONEY_ORDER_PAYPASSWORDCOUNT", compareUserInfoView.LanguageId), count, Math.Ceiling((limitTime - DateTime.Now).TotalMinutes)));
                                return resultModel;
                            }

                            if (!isPwdTrue)
                            {
                                if (passwordErrorModel.FailVerifyTimes >= count && limitTime <= DateTime.Now)
                                {
                                    //如果用户交易密码不同数据库交易密码,进行次数设置为1,验证时间为当前时间

                                    passwordErrorModel.FailVerifyTimes = 1;
                                    passwordErrorModel.VerifyTime = DateTime.Now;
                                    passwordErrorService.Update(passwordErrorModel, db);
                                    resultModel.Messages.Add(CultureHelper.GetAPPLangSgring("MONEY_ORDER_PAYPASSWORD_ERROR", compareUserInfoView.LanguageId));
                                }
                                else if (passwordErrorModel.FailVerifyTimes < count)
                                {
                                    //如果用户交易密码不同数据库交易密码,进行次数设置为1,验证时间为当前时间

                                    passwordErrorModel.FailVerifyTimes = passwordErrorModel.FailVerifyTimes + 1;
                                    passwordErrorModel.VerifyTime = DateTime.Now;
                                    passwordErrorService.Update(passwordErrorModel, db);
                                    resultModel.Messages.Add(CultureHelper.GetAPPLangSgring("MONEY_ORDER_PAYPASSWORD_ERROR", compareUserInfoView.LanguageId));

                                }
                                else
                                {
                                    resultModel.Messages.Add(CultureHelper.GetAPPLangSgring("MONEY_ORDER_PAYPASSWORD_ERROR", compareUserInfoView.LanguageId));
                                }
                            }
                            else
                            {
                                //将错误次数清空
                                passwordErrorModel.FailVerifyTimes = 0;
                                passwordErrorModel.VerifyTime = DateTime.Now;
                                passwordErrorService.Update(passwordErrorModel, db);


                                resultModel.IsValid = true;
                            }

                        }
                    }

                }
                else
                {
                    resultModel.IsValid = true;
                }
            }
            return resultModel;
        }



        ///<summary>
        ///获取用户信息（用于支付）
        ///</summary>
        ///<param name="userId">用户Id</param>
        ///<returns>用户信息</returns>
        public ResultModel GetYH_UserForPayment(long userId)
        {
            return this.GetYH_UserForPaymentPrivate(userId, _database.Db);
        }

        ///<summary>
        ///获取用户信息（用于支付）
        ///</summary>
        ///<param name="userId">用户Id</param>
        /// <param name="trans">事务对象</param>
        ///<returns>用户信息</returns>
        public ResultModel GetYH_UserForPayment(long userId, dynamic trans)
        {
            return this.GetYH_UserForPaymentPrivate(userId, trans);
        }

        /// <summary>
        /// 获取用户信息（用于支付）
        /// </summary>
        /// <param name="userId">Id</param>
        /// <param name="db">数据Db</param>
        /// <returns>用户信息</returns>
        private ResultModel GetYH_UserForPaymentPrivate(long userId, dynamic db)
        {
            ResultModel resultModel = new ResultModel();

            UserInfoViewForPayment userInfoView = db.YH_User.All()
                .LeftJoin(db.ZJ_UserBalance)
                .On(db.YH_User.UserID == db.ZJ_UserBalance.UserID)
                .Where(db.YH_User.UserID == userId)
                .Select(
                    db.YH_User.UserID,
                    db.YH_User.Phone,
                    db.YH_User.Account,
                    db.YH_User.PayPassWord,
                    db.YH_User.IsLock,
                    db.YH_User.IsDelete,
                    db.ZJ_UserBalance.ConsumeBalance
                ).FirstOrDefault<UserInfoViewForPayment>();

            resultModel.Data = userInfoView;
            return resultModel;
        }




        #endregion

        #region 邮件订阅
        /// <summary>
        /// 添加订阅
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddEmailSub(MailSubscriptionModel model)
        {
            ResultModel result;
            if(model.UserID.HasValue)
            {
                result = new ResultModel
                {
                    Data = this._database.Db.YH_MailSubscription.Insert(Email: model.Email
                    , Ip: model.Ip, SubType: model.SubType, SubDate: model.SubDate, SendStatus: model.SendStatus
                    , UserID: model.UserID.Value)
                };
            }
            else
            {
                result = new ResultModel
                {
                    Data = this._database.Db.YH_MailSubscription.Insert(Email: model.Email
                    , Ip: model.Ip, SubType: model.SubType, SubDate: model.SubDate, SendStatus: model.SendStatus
                    )
                };
            }
            return result;
        }
        /// <summary>
        /// 是否已经订阅过
        /// </summary>
        /// <param name="Email">Email地址</param>
        /// <returns></returns>
        public bool HasEmailSubd(string Email)
        {
            ResultModel result;
            if (!string.IsNullOrEmpty(Email))
            {
                result = new ResultModel
                {
                    Data = this._database.Db.YH_MailSubscription.FindAllByEmail(Email)
                };
                return result.Data.ToList().Count > 0;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
