using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Services.Users;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class YH_UserBankAccountController : HKBaseController
    {
        private readonly IYH_UserBankAccountService _yH_UserBankAccountService;

        public YH_UserBankAccountController(IYH_UserBankAccountService yH_UserBankAccountService)
        {
            _yH_UserBankAccountService = yH_UserBankAccountService;
        }

        // GET: ParameterSet
        public ActionResult Index()
        {
            return View();
        }

        #region 查询用户银行帐户信息表列表

        /// <summary>
        ///     查询用户银行帐户信息表列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        public JsonResult List(SearchYH_UserBankAccountModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var paramList =
                _yH_UserBankAccountService.Select(new SearchYH_UserBankAccountModel
                {
                    PagedIndex = model.PagedIndex,
                    PagedSize = model.PagedSize,
                    Account = model.Account,
                    Phone = model.Phone,
                    IsUse = model.IsUse
                });
            var data = new { rows = paramList.Data, total = paramList.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除用户银行帐户信息表

        /// <summary>
        ///     删除用户银行帐户信息表
        /// </summary>
        /// <param name="ParamenterID">用户银行帐户信息表Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        public JsonResult Delete(int? ID)
        {
            var resultModel = new ResultModel();
            if (ID.HasValue)
            {
                var result = _yH_UserBankAccountService.Delete(ID.Value).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Delete user's bank account information success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Delete user's bank account information failed" };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        //#region 创建系统信息

        ///// <summary>
        /////     创建系统信息
        ///// </summary>
        ///// <param name="model">用户银行帐户信息表对象</param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult Create(YH_UserBankAccountModel model)
        //{
        //    var admin = UserInfo.CurrentUserName;
        //    if (ModelState.IsValid)
        //    {
        //        var resultModel = new ResultModel();
        //        if (model.ID != 0)
        //        {
        //            model.UpdateBy = admin;
        //            model.UpdateDT = DateTime.Now;
        //            resultModel.Messages = new List<string>
        //            {
        //                _yH_UserBankAccountService.Update(model).Data > 0 ? "修改用户银行帐户信息表成功" : "修改用户银行帐户信息表失败"
        //            };
        //        }
        //        else
        //        {
        //            model.ParamenterID = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
        //            model.CreateBy = admin;
        //            model.CreateDT = DateTime.Now;
        //            model.UpdateBy = admin;
        //            model.UpdateDT = DateTime.Now;
        //            resultModel.Messages = new List<string>
        //            {
        //                _yH_UserBankAccountService.Add(model).Messages.Count == 0 ? "添加用户银行帐户信息表成功" : "添加用户银行帐户信息表失败"
        //            };
        //        }
        //        return Json(resultModel, JsonRequestBehavior.AllowGet);
        //    }
        //    return PartialView(model);
        //}

        //#endregion

        #region 加载数据

        /// <summary>
        ///     加载数据
        /// </summary>
        /// <param name="id">对象系统Id</param>
        /// <returns></returns>
        public ActionResult Create(int? id)
        {
            YH_UserBankAccountModel model = null;
            if (id.HasValue)
            {
                var result = _yH_UserBankAccountService.GetYH_UserBankAccountById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            else
            {
                model = new YH_UserBankAccountModel();
            }
            return PartialView(model);
        }

        #endregion
    }
}