using HKTHMall.Admin.common;
using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Models;
using HKTHMall.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class ZJ_UserBalanceController : Controller
    {
        private IZJ_UserBalanceService _zjUserBalanceService;
        public ZJ_UserBalanceController(IZJ_UserBalanceService zjUserBalanceService)
        {
            _zjUserBalanceService = zjUserBalanceService;
        }
        /// <summary>
        /// 账户管理-用户账户信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult List(SearchZJ_UserBalanceModel model)
        {
            
            model.PagedIndex = model.PagedIndex == 0 ? 0 : model.PagedIndex;
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            


            
            //查询列表 
            var result = this._zjUserBalanceService.GetZJ_UserBalanceList(model);
            List<ZJ_UserBalanceModel> ds = result.Data;
            var data = new
            {

                rows = ds,
                total = result.Data.TotalCount,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(long? id)
        {
            ZJ_UserBalanceModel model = new ZJ_UserBalanceModel();
            if (id.HasValue)
            {
                SearchZJ_UserBalanceModel smodel = new SearchZJ_UserBalanceModel();
                smodel.UserID = id.Value;
                smodel.PagedIndex = 0;
                smodel.PagedSize = 100;
                //查询列表 

                List<ZJ_UserBalanceModel> List = this._zjUserBalanceService.GetZJ_UserBalanceList(smodel).Data;

                if (List != null && List.Count > 0)
                {
                    model = List[0];
                }

            }
            return PartialView(model);
        }

        /// <summary>
        /// 新增,修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ZJ_UserBalanceModel model)
        {
            if (ModelState.IsValid)
            {
                ResultModel resultModel = new ResultModel();
               
                if (model.AddOrCutAmount>=0)
                {
                    model.AddOrCutType =15;
                }
                else
                {
                     model.AddOrCutType =16;
                     SearchZJ_UserBalanceModel smodel = new SearchZJ_UserBalanceModel();
                     smodel.UserID = model.UserID;
                     smodel.PagedIndex = 0;
                     smodel.PagedSize = 100;
                    //获取用户余额,用于判断最多扣款
                    List<ZJ_UserBalanceModel> List = this._zjUserBalanceService.GetZJ_UserBalanceList(smodel).Data;
                    if (List != null && List.Count > 0)
                    {
                        ZJ_UserBalanceModel zjubmodel = List[0];
                        if (zjubmodel.ConsumeBalance + model.AddOrCutAmount < 0 )
                        {
                            resultModel.Messages = new List<string> { "Insufficient balance！" };
                            return Json(resultModel, JsonRequestBehavior.AllowGet);
                        }
                        
                    }
                }
                model.IsDisplay = 1;
                var result = ZJ_UserBalanceCommon.UpdateZJ_UserBalance(model);

                resultModel.Messages = new List<string> { result == true ? "Recharge success！" : "Recharge failed！" };
                var opera = string.Empty;
                opera += " UserID:" + model.UserID + ",AddOrCutAmount(充值金额):" + model.AddOrCutAmount + ",结果:" + result;
                LogPackage.InserAC_OperateLog(opera, "账户管理-用户账户信息-余额充值");
                
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return PartialView(model);
        }
	}
}