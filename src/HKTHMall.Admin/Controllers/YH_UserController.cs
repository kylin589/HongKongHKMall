using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Services.AC;
using HKTHMall.Core;
using HKTHMall.Services.YHUser;
using HKTHMall.Domain.Models.YHUser;
using System.Web.Security;
using System;
using System.Linq;
using HKTHMall.Domain.Enum;
using HKTHMall.Core.Extensions;
using HKTHMall.Services.Users;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using System.Configuration;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel.Models.YHUser;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class YH_UserController : HKBaseController
    {
        private readonly IYH_MerchantInfoService _yh_MerchantInfoService;
        private readonly ITHAreaService _thAreaService;
        private readonly IYH_UserService _yh_UserService;
        private readonly IYH_AgentService _yh_AgentService;
        private readonly IZJ_AmountChangeTypeService _zj_AmountChangeTypeService;
        private readonly static string payPass = ConfigurationManager.AppSettings["payPass"];     //默认交易密码
        private readonly static string loginPass = ConfigurationManager.AppSettings["loginPass"];     //默认登录密码

        public YH_UserController(IYH_UserService yh_UserService, IZJ_AmountChangeTypeService zj_AmountChangeTypeService, ITHAreaService thAreaService, IYH_MerchantInfoService yh_MerchantInfoService, IYH_AgentService yh_AgentService)
        {
            _yh_UserService = yh_UserService;
            _zj_AmountChangeTypeService = zj_AmountChangeTypeService;
            _thAreaService = thAreaService;
            _yh_MerchantInfoService = yh_MerchantInfoService;
            _yh_AgentService = yh_AgentService;
        }

        #region  商城用户

        /// <summary>
        /// 商城用户列表页
        /// zhoub 20150714
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.PayPass = payPass;
            ViewBag.LoginPass = loginPass;
            return View();
        }


        /// <summary>
        /// 省份下拉框数据获取
        /// zhoub 20150918
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        public JsonResult GetTHAreaSelect(int parentID)
        {
            var result = _thAreaService.GetTHAreaByParentID(ACultureHelper.GetLanguageID, parentID).Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 商城用户列表页
        /// zhoub 20150714
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public JsonResult YH_UserList(SearchYHUserModel searchModel)
        {
            var result = _yh_UserService.GetPagingYH_User(searchModel);
            var data = new
            {
                rows = result.Data,
                total = result.Data.TotalCount,
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除用户
        /// zhoub 20150714
        /// </summary>
        /// <param name="ParamenterID">用户Id</param>
        /// <returns></returns>
        public JsonResult DeleteYH_User(long? userID, int status)
        {
            var resultModel = new ResultModel();
            YH_UserModel model = new YH_UserModel();
            if (userID.HasValue)
            {
                model.UserID = userID.Value;
                model.IsDelete = status;
                model.UpdateBy = UserInfo.CurrentUserName;
                model.UpdateDT = DateTime.Now;
                var result = _yh_UserService.DeleteYH_UserByUserID(model).Data;
                if (result > 0)
                {
                    resultModel.Messages = new List<string> { "Success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Failed." };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            string opera = string.Format("商城用户删除:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
            LogPackage.InserAC_OperateLog(opera, "User strategies--Users list-delete");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户锁定
        /// zhoub 20150714
        /// </summary>
        /// <param name="ParamenterID">用户Id</param>
        /// <returns></returns>
        public JsonResult LockYH_User(long? userID, int status)
        {
            var resultModel = new ResultModel();
            YH_UserModel model = new YH_UserModel();
            if (userID.HasValue)
            {
                model.UserID = userID.Value;
                model.IsLock = status;
                model.UpdateBy = UserInfo.CurrentUserName;
                model.UpdateDT = DateTime.Now;
                var result = _yh_UserService.UpdateYH_UserIsLock(model).Data;
                if (result > 0)
                {
                    resultModel.Messages = new List<string> { "Success." };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Failed." };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            string opera = string.Format("商城用户锁定:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
            LogPackage.InserAC_OperateLog(opera, "User strategies--Users list-Lock");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 重置用户交易密码
        /// zhoub 20150714
        /// </summary>
        /// <param name="ParamenterID">用户Id</param>
        /// <returns></returns>
        public JsonResult ResetYH_UserPayPassWord(long? userID)
        {
            var resultModel = new ResultModel();
            YH_UserModel model = new YH_UserModel();
            if (userID.HasValue)
            {
                model.UserID = userID.Value;
                model.PayPassWord = FormsAuthentication.HashPasswordForStoringInConfigFile(payPass, "MD5");
                model.UpdateBy = UserInfo.CurrentUserName;
                model.UpdateDT = DateTime.Now;
                var result = _yh_UserService.UpdateYH_UserPayPassWord(model).Data;
                if (result > 0)
                {
                    resultModel.Messages = new List<string> { "Reset trading password success." };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Reset trading password failed." };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            string opera = string.Format("商城用户重置交易密码:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
            LogPackage.InserAC_OperateLog(opera, "User strategies--Users list-Reset deal password");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重置用户登录密码
        /// zhoub 20150714
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public JsonResult ResetYH_UserPassWord(long? userID)
        {
            var resultModel = new ResultModel();
            YH_UserModel model = new YH_UserModel();
            if (userID.HasValue)
            {
                model.UserID = userID.Value;
                model.PassWord = FormsAuthentication.HashPasswordForStoringInConfigFile(loginPass, "MD5");
                model.UpdateBy = UserInfo.CurrentUserName;
                model.UpdateDT = DateTime.Now;
                var result = _yh_UserService.UpdateYH_UserPassWord(model).Data;
                if (result > 0)
                {
                    resultModel.Messages = new List<string> { "Reset login password success." };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Reset login password failed." };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            string opera = string.Format("商城用户重置登录密码:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
            LogPackage.InserAC_OperateLog(opera, "User strategies--Users list-Reset login password");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 感恩惠粉人数获取
        /// zhoub 20150715
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetYH_UserReferrerIDCount(long? userID)
        {
            var s = _yh_UserService.GetYH_UserReferrerIDCount(userID.Value);
            return Convert.ToString(s.Data);
        }

        /// <summary>
        /// 根据用户ID获取消费金额、收益金额
        /// zhoub 20150826
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="type">1 消费金额 2 收益金额</param>
        /// <returns></returns>
        public string GetYH_UserMoney(long userID, int type)
        {
            var s = _yh_UserService.GetYH_UserMoney(userID, type);
            return Convert.ToString(Math.Abs(Convert.ToDecimal(s.Data)));
        }

        #region  升级商家

        /// <summary>
        /// 升级商家视图
        /// zhoub 20150918
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult UpgradeMerchantIndex(long userId, string type)
        {
            YH_MerchantInfoModel model = new YH_MerchantInfoModel();
            if (type == "2" || type == "3")
            {
                model = _yh_MerchantInfoService.GetYH_MerchantInfoById(userId).Data[0];
                ViewData["thAreaShi"] = _thAreaService.GetTHAreaByParentID(ACultureHelper.GetLanguageID, model.ShengTHAreaID.Value).Data;
                ViewData["thAreaQu"] = _thAreaService.GetTHAreaByParentID(ACultureHelper.GetLanguageID, model.ShiTHAreaID.Value).Data;
            }
            ViewData["thArea"] = _thAreaService.GetTHAreaByParentID(ACultureHelper.GetLanguageID, 0).Data;
            ViewBag.UserId = userId;
            ViewBag.Type = type;
            ViewBag.Message = Request.Params["message"];
            return View(model);
        }

        /// <summary>
        /// 升级商家操作
        /// zhoub 20150918
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpgradeMerchantIndex(YH_MerchantInfoModel model)
        {
            ResultModel result = new ResultModel();
            string opType = Request.Params["opType"];
            model.MerchantType = 1;
            model.IsPublishProduct = 1;
            model.IsProvideInvoices = true;
            model.Longitude = 0;
            model.Latitude = 0;
            if (opType == "1")
            {
                model.CreateBy = UserInfo.CurrentUserName;
                model.CreateDT = DateTime.Now;
                result = _yh_MerchantInfoService.Add(model);
                string opera = string.Format("升级商家添加:{0},操作结果:{1}", JsonConverts.ToJson(model), result.Messages);
                LogPackage.InserAC_OperateLog(opera, "User strategies--Users list--Upgrade Merchant");
            }
            else
            {
                model.UpdateBy = UserInfo.CurrentUserName;
                model.UpdateDT = DateTime.Now;
                result = _yh_MerchantInfoService.Edit(model);
                string opera = string.Format("升级商家修改:{0},操作结果:{1}", JsonConverts.ToJson(model), result.Messages);
                LogPackage.InserAC_OperateLog(opera, "User strategies--Users list--Upgrade Merchant");
            }
            return this.RedirectToAction("UpgradeMerchantIndex", new { userId = model.MerchantID, type = opType, message = result.Message });
        }

        /// <summary>
        /// 商家审核视图
        /// zhoub 20150918
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ActionResult AuditMerchant(long userId)
        {
            YH_MerchantInfoModel model = _yh_MerchantInfoService.GetYH_MerchantInfoById(userId).Data[0];
            return PartialView(model);
        }

        /// <summary>
        /// 商家审核操作
        /// zhoub 20150918
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AuditMerchant(YH_MerchantInfoModel model)
        {
            ResultModel resultModel = new ResultModel();
            model.AuditBy = UserInfo.CurrentUserName;
            model.AuditDT = DateTime.Now;
            resultModel = this._yh_MerchantInfoService.AuditYH_MerchantInfo(model);
            string opera = string.Format("商家审核:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
            LogPackage.InserAC_OperateLog(opera, "User strategies--Users list--Audit");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  升级代理商

        /// <summary>
        /// 升级代理商视图
        /// zhoub 20150924
        /// </summary>
        /// <returns></returns>
        public ActionResult UpgradeAgentIndex(long userID)
        {
            ResultModel result = _yh_AgentService.GetYH_AgentByUserId(userID);
            return PartialView(result.Data[0]);
        }

        /// <summary>
        /// 升级代理商操作
        /// zhoub 20150918
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpgradeAgentIndex(YH_AgentModel model)
        {
            ResultModel resultModel = new ResultModel();
            if (model.AgentID > 0)
            {
                model.UpdateDT = DateTime.Now;
                model.UpdateBy = UserInfo.CurrentUserName;
                resultModel = _yh_AgentService.EditYH_Agent(model);
            }
            else
            {
                model.CreateBy = UserInfo.CurrentUserName;
                model.CreateDT = DateTime.Now;
                resultModel = _yh_AgentService.AddYH_Agent(model);
            }
            string opera = string.Format("升级代理商:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
            LogPackage.InserAC_OperateLog(opera, "User strategies--Users list--Upgrade Agent");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region 商城用户余额变动记录

        /// <summary>
        /// 用户余额变动记录
        /// zhoub 20150714
        /// </summary>
        /// <returns></returns>
        public ActionResult UserBalanceChangeIndex(string userID)
        {
            //TempData["AddOrCutTypeEnum"] =SimpleReocrdExtensions.ToSelectListItem((Enum)AddOrCutTypeEnum.充值);
            SearchUserBalanceChangeModel searchModel = new SearchUserBalanceChangeModel();
            searchModel.UserID = !string.IsNullOrEmpty(userID) ? long.Parse(userID) : 0;//modified by jimmy,2015-8-29
            TempData["AddOrCutTypeEnum"] = SelectCommon.GetAmountChangeType();
            return View();
        }

        /// <summary>
        /// 用户余额变动数据获取
        /// zhoub 20150714
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public JsonResult GetUserBalanceChangeList(SearchUserBalanceChangeModel searchModel)
        {
            var result = _yh_UserService.GetPagingZJ_UserBalanceChangeLog(searchModel);

            var data = new
            {
                rows = result.Data,
                total = result.Data.TotalCount,
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}