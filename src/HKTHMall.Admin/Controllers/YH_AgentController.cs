using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using HKTHMall.Domain.AdminModel.Models;
using HKTHMall.Domain.AdminModel.Models.YHUser;
using HKTHMall.Domain.Models;
using HKTHMall.Services.AC;
using HKTHMall.Services.YHUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class YH_AgentController : Controller
    {
        private readonly IYH_AgentService _yh_AgentService;
        private readonly IYH_UserService _yh_UserService;

        public YH_AgentController(IYH_UserService yh_UserService, IYH_AgentService yh_AgentService)
        {
            _yh_UserService = yh_UserService;
            _yh_AgentService = yh_AgentService;
            
        }

        /// <summary>
        /// 【代理商表】
        /// wuyf 2015-9-24
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            
            return View();
        }

        /// <summary>
        /// 代理商表
        /// wuyf 2015-9-24
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public JsonResult List(SearchYH_AgentModel searchModel)
        {
            searchModel.PagedIndex = searchModel.PagedIndex == 0 ? 0 : searchModel.PagedIndex;
            searchModel.PagedSize = searchModel.PagedSize == 0 ? 10 : searchModel.PagedSize;
            var result = _yh_AgentService.GetPagingYH_Agent(searchModel); 
            var data = new
            {
                rows = result.Data,
                total = result.Data.TotalCount,
            };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 感恩惠粉人数获取
        /// wuyf 20150924
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
        /// wuyf 20150924
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="type">1 消费金额 2 收益金额</param>
        /// <returns></returns>
        public string GetYH_UserMoney(long userID, int type)
        {
            var s = _yh_UserService.GetYH_UserMoney(userID, type);
            return Convert.ToString(Math.Abs(Convert.ToDecimal(s.Data)));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        
        public JsonResult DeleteYH_User(long? UserID)
        {
            YH_AgentModel model = new YH_AgentModel();
            var resultModel = new ResultModel();
            if (UserID.HasValue)
            {
                model.UserID = UserID.Value;
                var result = this._yh_AgentService.DeleterYH_Agent(model).IsValid;
                if (result)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Delete success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Delete failed" };
                }

                var opera = string.Empty;

                opera = " UserID:" + model.UserID + ",结果:" + resultModel.Messages;
                LogPackage.InserAC_OperateLog(opera, "供应商-删除");
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Parameter ID error" };
            }

            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }
	}
}