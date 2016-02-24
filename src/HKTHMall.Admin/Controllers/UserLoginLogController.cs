using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models.LoginLog;
using HKTHMall.Domain.Models.User;
using HKTHMall.Services.LoginLog;
using HKTHMall.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
      [UserAuthorize]
    public class UserLoginLogController :  HKBaseController
    {
        private IUserLoginLogService _userLoginLogService;

        private IAC_UserService _ac_UserService;

        public UserLoginLogController(IUserLoginLogService userLoginLogService, IAC_UserService ac_UserService)
        {
            this._userLoginLogService = userLoginLogService;
            this._ac_UserService = ac_UserService;
        }

        
        //列表页
        // GET: /AC_OperateLog/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List(SearchUserLoginLogModel logmodel)
        {
            //SearchAC_OperateLogModel logmodel = new SearchAC_OperateLogModel();
            logmodel.PagedIndex =logmodel.PagedIndex==0? 0:logmodel.PagedIndex;
            logmodel.PagedSize = logmodel.PagedSize == 0 ? 10 : logmodel.PagedSize;
            //加一天是为查询最后一天的数据
            logmodel.EndLoginTime = logmodel.EndLoginTime == null ? DateTime.Now.AddDays(1) : logmodel.EndLoginTime.Value.AddDays(1);
            //查询后台用户登录日志表
            var result = this._userLoginLogService.GetUserLoginLog(logmodel);
            List<UserLoginLogModel> ds = result.Data;
            var data = new
            {

                rows = ds,
                total = result.Data.TotalCount,
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult Login()
        {
            return View();
        }
        


        public string DeleteUserLoginLogLog(int? ID)
        {
            UserLoginLogModel model = new UserLoginLogModel();
            if (ID.HasValue)
            {
                model.ID = ID.Value;
                var result = this._userLoginLogService.DeleteUserLoginLog(model).Data;
                return result == true ? "Delete success！" : "Delete failed！";
            }

            return "Delete failed！";
            
        }
	}
}