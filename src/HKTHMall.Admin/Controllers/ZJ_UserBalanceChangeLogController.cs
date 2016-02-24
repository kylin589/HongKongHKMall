using HKTHMall.Admin.common;
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
    public class ZJ_UserBalanceChangeLogController : Controller
    {
        private IZJ_UserBalanceChangeLogService _zjUserBalanceChangeLog;
        private IZJ_AmountChangeTypeService _zjAmountChangeTypeService;
        public ZJ_UserBalanceChangeLogController(IZJ_UserBalanceChangeLogService zjUserBalanceChangeLog, IZJ_AmountChangeTypeService zjAmountChangeTypeService)
        {
            _zjUserBalanceChangeLog = zjUserBalanceChangeLog;
            _zjAmountChangeTypeService = zjAmountChangeTypeService;
        }
        //
        // GET: /ZJ_UserBalanceChangeLog/
        public ActionResult Index()
        {
            SearchZJ_AmountChangeTypeModel model=new SearchZJ_AmountChangeTypeModel();
            model.PagedIndex = 0;
            model.PagedSize=100;
            ViewBag.list = SelectCommon.GetAmountChangeType();
            //ViewBag.list = _zjAmountChangeTypeService.GetZJ_AmountChangeTypeList(model).Data;
            return View();
        }

        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public JsonResult List(SearchZJ_UserBalanceChangeLogModel model)
        {
            //SearchAC_OperateLogModel logmodel = new SearchAC_OperateLogModel();
            model.PagedIndex = model.PagedIndex == 0 ? 0 : model.PagedIndex;
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            


        
            //查询列表 
            var result = this._zjUserBalanceChangeLog.GetZJ_UserBalanceChangeLogList(model);
            List<ZJ_UserBalanceChangeLogModel> ds = result.Data;
            var data = new
            {

                rows = ds,
                total = result.Data.TotalCount,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }
	}
}