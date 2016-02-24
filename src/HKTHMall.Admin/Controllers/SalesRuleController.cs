using HKTHMall.Core.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Services.Products;
using HKTHMall.Domain.Models;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
    
    /// <summary>
    /// 促销规则控制器处理
    /// </summary>
      [UserAuthorize]
    public class SalesRuleController : HKBaseController
    {
        private readonly ISalesRuleService _salesRuleService;

        public SalesRuleController(ISalesRuleService salesRuleService)
        {
            _salesRuleService = salesRuleService;
        }

        // GET: ParameterSet
        public ActionResult Index()
        {
            return View();
        }

        #region 查询促销规则列表

        /// <summary>
        /// 查询促销规则列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult List(SearchSalesRuleModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var paramList =
                _salesRuleService.Select(new SearchSalesRuleModel
                {
                    PagedIndex = model.PagedIndex,
                    PagedSize = model.PagedSize,
                    RuleName = model.RuleName
                });
            var data = new { rows = paramList.Data, total = paramList.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除促销规则

        /// <summary>
        ///     删除促销规则
        /// </summary>
        /// <param name="salesRuleId">促销规则Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult Delete(int? salesRuleId)
        {
            var resultModel = new ResultModel();
            if (salesRuleId.HasValue)
            {
                var result = _salesRuleService.Delete(salesRuleId.Value).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "删除促销规则成功" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "删除促销规则失败" };
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

        #region 创建促销规则信息

        /// <summary>
        ///     创建促销规则信息
        /// </summary>
        /// <param name="model">促销规则对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(SalesRuleModel model)
        {
            var admin = UserInfo.CurrentUserName;
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                if (model.SalesRuleId != 0)
                {
                    resultModel.Messages = new List<string>
                    {
                        _salesRuleService.Update(model).Data > 0 ? "修改促销规则成功" : "修改促销规则失败"
                    };
                }
                else
                {
                    model.SalesRuleId = (int)MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    resultModel.Messages = new List<string>
                    {
                        _salesRuleService.Add(model).Messages.Count == 0 ? "添加促销规则成功" : "添加促销规则失败"
                    };
                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            return PartialView(model);
        }

        #endregion

        #region 加载数据

        /// <summary>
        ///     加载数据
        /// </summary>
        /// <param name="id">对象促销规则Id</param>
        /// <returns></returns>
        public ActionResult Create(int? id)
        {
            SalesRuleModel model = null;
            if (id.HasValue)
            {
                var result = _salesRuleService.GetSalesRuleById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            else
            {
                model = new SalesRuleModel();
            }
            return PartialView(model);
        }

        #endregion
    }
}