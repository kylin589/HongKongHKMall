using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Login;
using Simple.Data;
using System.Text.RegularExpressions;
using HKTHMall.Core;
using HKTHMall.Services.Common.MultiLangKeys;

namespace HKTHMall.Services.WebLogin.Impl
{
    public class LoginService : BaseService, ILoginService
    {
        /// <summary>
        /// 根据手机号获取用户信息
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public ResultModel GetUserInfoByPhone(string phone)
        {
            var result = new ResultModel
            {
                Data = _database.Db.YH_User.FindByPhone(phone)
            };
            return result;
        }

        /// <summary>
        /// 根据id获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel GetUserInfoById(long id)
        {
            var result = new ResultModel();
            result.Data = this._database.RunQuery(db =>
            {
                return db.YH_User.Get(id);
            });
            return result;
        }

        /// <summary>
        /// 根据账号获取用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public ResultModel GetUserInfoByAccount(string account)
        {
            var result = new ResultModel
            {
                Data = _database.Db.YH_User.FindByAccount(account)
            };
            return result;
        }
        /// <summary>
        /// 根据邮箱获取用户信息
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ResultModel GetUserInfoByEmail(string email)
        {
            var result = new ResultModel
            {
                Data = _database.Db.YH_User.FindByEmail(email)
            };
            return result;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public ResultModel Update(YH_UserModel model)
        {
            var result = new ResultModel()
            {
                Data = _database.Db.YH_User.Update(model)
            };
            return result;
        }

        /// <summary>
        /// 插入用户信息
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public ResultModel Add(YH_UserModel model)
        {
            var result = new ResultModel()
            {
                Data = _database.Db.YH_User.Insert(model)
            };
            return result;
        }


        #region 注册模块


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="welcomeCode"></param>
        /// <param name="phone"></param>
        /// <param name="pwd"></param>
        /// <param name="pwd_md5"></param>
        /// <param name="code"></param>
        /// <param name="_user"></param>
        /// <returns></returns>
        public BackMessage Register(string pwd, string pwd_md5, string email, out YH_UserModel _user)
        {
            _user = null;

            //注册回调信息
            BackMessage msg = new BackMessage();
            msg.status = 0;

           
            if ("" == pwd || new Regex(@"/[^\x00-\xff]|\s/").IsMatch(pwd))
            {
                msg.message = CultureHelper.GetLangString("LOGIN_PWD_FORMAT");//"密码格式不正确8-20位字母数字组合!";
                return msg;
            }
         
            if (!string.IsNullOrEmpty(email))
            {
                if (!new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").IsMatch(email))
                {
                    msg.message = CultureHelper.GetLangString("ACCOUNT_USERINFO_ADDRESS_EMAILERROR");//邮箱格式错误
                    return msg;
                }
            }
       

            using (var tx = _database.Db.BeginTransaction())
            {

                try
                {
                   

                    YH_UserModel user = new YH_UserModel();
                    user.UserID = HKTHMall.Core.MemCacheFactory.GetCurrentMemCache().Increment("commonId"); //rstatus.uid;
                    //user.Phone = phone;
                    user.PassWord = pwd_md5;
                    user.NickName = SetNickName();
                    user.Sex = 0;
                    user.RegisterSource = 1;
                    user.UserType = 0;
                    user.ActivePhone = 1;
                    user.IsLock = 0;
                    user.IsDelete = 0;

                  

                    //判断本地UID数据是否存在
                    var result =GetUserInfoByEmail(email);
                    //YH_UserModel has_uid = GetUserInfoById().Data;
                    if (!result.IsValid)
                    {
                        tx.Rollback();
                        msg.message = CultureHelper.GetLangString("REGISETR_HADREGISTER");//您输入的手机号码已注册;
                        return msg;
                    }
                    else
                    {
                        user.Account =email;
                        user.Email = email;
                        user.ActiveEmail = 0;
                        user.ReferrerID = 0;
                        user.ParentID2 = 0;
                        user.ParentID3 = 0;
                        user.ParentIDs = "";
                        //user.UpdateDT = ;
                        user.RegisterDate=DateTime.Now;
                        //写入本地数据
                        if (!Add(user).IsValid)
                        {
                            tx.Rollback();
                            msg.message = CultureHelper.GetLangString("REGISETR_USERREGISTERFAIL"); //会员注册失败;
                            return msg;
                        }

                        ZJ_UserBalanceModel zj_UserBalanceModel = new ZJ_UserBalanceModel();
                        zj_UserBalanceModel.UserID = user.UserID;
                        zj_UserBalanceModel.ConsumeBalance = 0;
                        zj_UserBalanceModel.AccountStatus = 1;
                        zj_UserBalanceModel.CreateBy = user.Account;
                        zj_UserBalanceModel.CreateDT = DateTime.Now;
                        zj_UserBalanceModel.UpdateBy = user.Account;
                        zj_UserBalanceModel.UpdateDT = DateTime.Now;

                        if (!new ZJ_UserBalanceService().AddBalance(zj_UserBalanceModel).IsValid)
                        {
                            tx.Rollback();
                            msg.message = CultureHelper.GetLangString("REGISETR_USERREGISTERFAIL"); //会员注册失败;
                            return msg;
                        }


                        YH_UserUpdateInfoModel updateInfo = new YH_UserUpdateInfoModel { UserID = user.UserID, TimeStamp = HKTHMall.Services.Common.ConvertsTime.DateTimeToTimeStamp(DateTime.Now) };
                        if (!new YH_UserUpdateInfoService().Add(updateInfo).IsValid)
                        {
                            tx.Rollback();
                            msg.message = CultureHelper.GetLangString("REGISETR_USERREGISTERFAIL"); //会员注册失败;
                            return msg;
                        }

                        _user = new YH_UserModel { UserID = user.UserID, Account = user.Account, NickName = user.NickName, UserType = user.UserType,Email=email };
                        tx.Commit();

                        msg.status = 1;
                        msg.message = CultureHelper.GetLangString("REGISETR_USERREGISTERSUCCESS"); //会员注册成功;
                        return msg;
                    }
                }
                catch
                {
                    tx.Rollback();
                    msg.message = CultureHelper.GetLangString("REGISETR_USERREGISTERFAIL"); //会员注册失败;
                    return msg;
                }
            }
        }

        #endregion


        /// <summary>
        /// 随机生成昵称
        /// </summary>
        /// <returns></returns>
        public static string SetNickName()
        {
            int number;
            char code;
            string checkCode = String.Empty;
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                number = random.Next();
                if (number % 2 == 0)
                    code = (char)('0' + (char)(number % 10));
                else
                    code = (char)('A' + (char)(number % 26));
                checkCode += code.ToString();
            }
            return "TH" + checkCode;
        }

        /// <summary>
        /// UTC秒转换为本地日期时间
        /// </summary>
        /// <param name="utcStr"></param>
        /// <returns></returns>
        public static DateTime ConvertUTCToDateTime(long utcStr)
        {
            DateTime startTime = new System.DateTime(1970, 1, 1, 8, 0, 0);
            return startTime.AddSeconds(utcStr);
        }

        /// <summary>
        /// 获取邮箱验证
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultModel GetModelByUserID(long userId)
        {
            var result = new ResultModel
            {
                Data = _database.Db.YH_ValidEmail.FindByUserID(userId)
            };
            return result;
        }
        /// <summary>
        /// 更改邮箱验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel UpdateValidEmail(YH_ValidEmailModel model)
        {

            var result = new ResultModel()
            {
                Data = _database.Db.YH_ValidEmail.Update(model)
            };
            return result;
        }
        /// <summary>
        /// 插入邮箱验证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ResultModel AddValidEmail(YH_ValidEmailModel model)
        {

            var result = new ResultModel()
            {
                Data = _database.Db.YH_ValidEmail.Insert(model)
            };
            return result;
        }

        /// <summary>
        /// 第三方账号登录 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ResultModel LoginThree(string id_3rd, string name, int type)
        {
            ResultModel rm = new ResultModel();
            rm.IsValid = false;
            YH_UserModel has_uid = _database.Db.YH_User.FindByThirdID(id_3rd);
            if (has_uid == null)
            {
                has_uid = new YH_UserModel();
                has_uid.UserID =MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                has_uid.Account = name;
                has_uid.PassWord = "";
                has_uid.PayPassWord = null;
                has_uid.NickName = name;
                has_uid.HeadImageUrl = "";
                has_uid.Sex = 0;
                has_uid.RealName = null;
                has_uid.Email ="";
                has_uid.Phone ="";
                has_uid.Birthday = null;
                has_uid.RegisterDate = DateTime.Now;
                has_uid.RegisterSource = 1;
                has_uid.ActivePhone = 0;
                has_uid.ActiveEmail = 0;
                has_uid.RecommendCode = null;
                has_uid.ReferrerID = 0;
                has_uid.ParentID2 = 0;
                has_uid.ParentID3 = 0;
                has_uid.ParentIDs = null;
                has_uid.Level = 0;
                has_uid.UserType = 0;
                has_uid.IsLock = 0;
                has_uid.IsDelete = 0;
                has_uid.UpdateBy = name;
                has_uid.UpdateDT = DateTime.Now;
                has_uid.DetailsAddress = null;
                has_uid.OrcodeUrl = "";
                has_uid.ThirdID = id_3rd;
                has_uid.ThirdType = type;
                Add(has_uid);

                //设置账号vip字段
                //ReqSetUserVipRights vipReq = new ReqSetUserVipRights();
                //vipReq.acc = log3.acc;
                //vipReq.uid = log3.uid;
                //vipReq.vip = new int[] { 2 };
                //vipReq.sys_id = Core.Settings.EmSystemId;
                //vipReq.mac = Core.Settings.GetMacAddress();
                //vipReq.ip = Core.Settings.GetIPAddress();
                //vipReq.dev = Core.Settings.EmDev;
                //vipReq.bus_seq = "T011111111111";
                //EmMethodManage.EmFindUpdateInstance.MsgSetUserVipRightsReq(vipReq);

                ZJ_UserBalanceModel zj_UserBalanceModel = new ZJ_UserBalanceModel();
                zj_UserBalanceModel.UserID = has_uid.UserID;
                zj_UserBalanceModel.ConsumeBalance = 0;
                zj_UserBalanceModel.AccountStatus = 1;
                zj_UserBalanceModel.CreateBy = has_uid.Account;
                zj_UserBalanceModel.CreateDT = DateTime.Now;
                zj_UserBalanceModel.UpdateBy = has_uid.Account;
                zj_UserBalanceModel.UpdateDT = DateTime.Now;
                new ZJ_UserBalanceService().AddBalance(zj_UserBalanceModel);

                YH_UserUpdateInfoModel updateInfo = new YH_UserUpdateInfoModel
                {
                    UserID = has_uid.UserID,
                    TimeStamp =HKTHMall.Services.Common.ConvertsTime.DateTimeToTimeStamp( has_uid.UpdateDT.Value)
                };
                new YH_UserUpdateInfoService().Add(updateInfo);
            }
            rm.IsValid = true;
            rm.Data = has_uid;



            //ResponseLogOn3rdAcc log3=LoginThreeIm(id_3rd, name, type);
            //if (log3.Status == 0x001000D2)
            //{
            //    RequestConfirm3rdAcc requestConfirm3rdAcc=new RequestConfirm3rdAcc();
            //    requestConfirm3rdAcc.seq_id = log3.seq_id;
            //    requestConfirm3rdAcc.type = 1;
            //    requestConfirm3rdAcc.sys_id = Core.Settings.EmSystemId;
            //    requestConfirm3rdAcc.nick = name;
            //    requestConfirm3rdAcc.sys_id = Core.Settings.EmSystemId;
            //    requestConfirm3rdAcc.mac = Core.Settings.GetMacAddress();
            //    requestConfirm3rdAcc.ip = Core.Settings.GetIPAddress();
            //    requestConfirm3rdAcc.dev = Core.Settings.EmDev;
            //    log3 = EmMethodManage.EmLoginInstance.MsgConfirm3rdAccReq(requestConfirm3rdAcc);
            //}
            //if (log3.Status==0)
            //{
            //    YH_UserModel has_uid = GetUserInfoById(log3.uid).Data;
            //    if(has_uid==null)
            //    {
            //        has_uid = new YH_UserModel();
            //        has_uid.UserID = log3.uid;
            //        has_uid.Account = log3.acc;
            //        has_uid.PassWord = log3.pwd;
            //        has_uid.PayPassWord = null;
            //        has_uid.NickName = name;
            //        has_uid.HeadImageUrl = log3.img_url;
            //        has_uid.Sex = (byte)log3.sex;
            //        has_uid.RealName = null;
            //        has_uid.Email = log3.email;
            //        has_uid.Phone = log3.phone;
            //        has_uid.Birthday = null;
            //        has_uid.RegisterDate = ConvertUTCToDateTime(log3.regtime);
            //        has_uid.RegisterSource = 1;
            //        has_uid.ActivePhone = 0;
            //        has_uid.ActiveEmail = 0;
            //        has_uid.RecommendCode = null;
            //        has_uid.ReferrerID = 0;
            //        has_uid.ParentID2 = 0;
            //        has_uid.ParentID3 = 0;
            //        has_uid.ParentIDs = null;
            //        has_uid.Level = 0;
            //        has_uid.UserType = 0;
            //        has_uid.IsLock = 0;
            //        has_uid.IsDelete = 0;
            //        has_uid.UpdateBy = log3.acc;
            //        has_uid.UpdateDT = DateTime.Now;
            //        has_uid.DetailsAddress = null;
            //        has_uid.OrcodeUrl = "";
            //        Add(has_uid);

            //        //设置账号vip字段
            //        ReqSetUserVipRights vipReq = new ReqSetUserVipRights();
            //        vipReq.acc = log3.acc;
            //        vipReq.uid = log3.uid;
            //        vipReq.vip = new int[] { 2 };
            //        vipReq.sys_id = Core.Settings.EmSystemId;
            //        vipReq.mac = Core.Settings.GetMacAddress();
            //        vipReq.ip = Core.Settings.GetIPAddress();
            //        vipReq.dev = Core.Settings.EmDev;
            //        vipReq.bus_seq = "T011111111111";
            //        EmMethodManage.EmFindUpdateInstance.MsgSetUserVipRightsReq(vipReq);

            //        ZJ_UserBalanceModel zj_UserBalanceModel = new ZJ_UserBalanceModel();
            //        zj_UserBalanceModel.UserID = log3.uid;
            //        zj_UserBalanceModel.ConsumeBalance = 0;
            //        zj_UserBalanceModel.AccountStatus = 1;
            //        zj_UserBalanceModel.CreateBy = log3.acc;
            //        zj_UserBalanceModel.CreateDT = DateTime.Now;
            //        zj_UserBalanceModel.UpdateBy = log3.acc;
            //        zj_UserBalanceModel.UpdateDT = DateTime.Now;
            //        new ZJ_UserBalanceService().AddBalance(zj_UserBalanceModel);

            //        YH_UserUpdateInfoModel updateInfo = new YH_UserUpdateInfoModel
            //        {
            //            UserID = log3.uid,
            //            TimeStamp = log3.regtime
            //        };
            //        new YH_UserUpdateInfoService().Add(updateInfo);
            //    }
            //    rm.IsValid = true;
            //    rm.Data = has_uid;
            //}
            return rm;
        }

    }
}
