using HKTHMall.Domain.AdminModel.Models.User;
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
    public class ZJ_RechargeOrderController : Controller
    {
         private IZJ_RechargeOrderService _zjRechargeOrderService;
         public ZJ_RechargeOrderController(IZJ_RechargeOrderService zjRechargeOrderService)
        {
            _zjRechargeOrderService = zjRechargeOrderService;
        }
        //
        // GET: /ZJ_RechargeOrder/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult List(SearchZJ_RechargeOrderModel model)
        {
            //SearchAC_OperateLogModel logmodel = new SearchAC_OperateLogModel();
            model.PagedIndex = model.PagedIndex == 0 ? 0 : model.PagedIndex;
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            //model.BeginCreateDT = model.BeginCreateDT == null ? DateTime.Now.AddDays(-30) : model.BeginCreateDT;
            //model.EedCreateDT = model.EedCreateDT == null ? DateTime.Now.AddDays(1) : model.EedCreateDT.AddDays(1);
            //model.EndRechargeDT = model.EndRechargeDT == null ? DateTime.Now.AddDays(1) : model.EndRechargeDT.Value.AddDays(1);


            
            //查询列表 
            var result = this._zjRechargeOrderService.GetZJ_RechargeOrderList(model);
            List<ZJ_RechargeOrderModel> ds = result.Data;
            var data = new
            {

                rows = ds,
                total = result.Data.TotalCount,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
	}
}