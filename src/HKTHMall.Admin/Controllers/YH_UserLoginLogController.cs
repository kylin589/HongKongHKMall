using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models.LoginLog;
using HKTHMall.Services.LoginLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class YH_UserLoginLogController : HKBaseController
    {
         private IYH_UserLoginLogService _YH_userLoginLogService;

         public YH_UserLoginLogController(IYH_UserLoginLogService userLoginLogService)
        {
            this._YH_userLoginLogService = userLoginLogService;
        }
        //列表页
        // GET: /AC_OperateLog/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List(SearchYH_UserLoginLogModel logmodel)
        {
            
            logmodel.PagedIndex =logmodel.PagedIndex==0? 0:logmodel.PagedIndex;
            logmodel.PagedSize = logmodel.PagedSize == 0 ? 10 : logmodel.PagedSize;
            //加一天是为查询最后一天的数据
            logmodel.EndLoginTime = logmodel.EndLoginTime == null ? DateTime.Now.AddDays(1) : logmodel.EndLoginTime.Value.AddDays(1);
            //查询用户登录日志表
            var result = this._YH_userLoginLogService.GetYH_UserLogin(logmodel);
            List<YH_UserLoginLogModel> ds = result.Data;
            var data = new
            {
                rows = ds,
                total = result.Data.TotalCount,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }





        public string DeleteYH_UserLoginLogLog(int? ID)
        {
            YH_UserLoginLogModel model = new YH_UserLoginLogModel();
            if (ID.HasValue)
            {
                model.ID = ID.Value;
                var result = this._YH_userLoginLogService.DeleteYH_UserLogin(model).Data;
                return result == true ? "Delete success！" : "Delete failed！";
            }

            return "Delete failed！";
            
        }
	}
}