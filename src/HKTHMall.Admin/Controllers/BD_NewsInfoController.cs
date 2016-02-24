//using HKSJ.MidMessage.Protocol;
//using HKSJ.MidMessage.Services;
using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel.Models.New;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.Models;
using HKTHMall.Services.New;
using HKTHMall.Services.YHUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.Admin.Controllers
{
    /// <summary>
    /// 新闻信息控制
    /// </summary>
    [UserAuthorize]
    public class BD_NewsInfoController : HKBaseController
    {
        private readonly IBD_NewsInfoService _bD_NewsInfoService;
        private readonly IYH_UserService _yH_UserService;
        private readonly IAPP_NewsInfoService _aPP_NewsInfoService;
        public static readonly string ImagePath = HKSJ.Common.ConfigHelper.GetConfigString("ImagePath");
        public static readonly string ImageHeader = HKSJ.Common.ConfigHelper.GetConfigString("ImageHeader");
        public BD_NewsInfoController(IBD_NewsInfoService bD_NewsInfoService, IYH_UserService yH_UserService,
            IAPP_NewsInfoService aPP_NewsInfoService)
        {
            _bD_NewsInfoService = bD_NewsInfoService;
            _yH_UserService = yH_UserService;
            _aPP_NewsInfoService = aPP_NewsInfoService;

        }
        public ActionResult Index()
        {
            DrowList(null);
            return View();
        }

        #region 查询新闻信息列表

        /// <summary>
        ///     查询新闻信息列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult List(SearchBD_NewsInfoModel model)
        {
            DrowList(null);
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var list =
                _bD_NewsInfoService.Select(model);
            
            var data = new { rows = list.Data, total = list.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除新闻信息

        /// <summary>
        ///     删除新闻信息
        /// </summary>
        /// <param name="ParamenterID">新闻信息Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-3</remarks>
        public JsonResult Delete(int? ID)
        {
            var resultModel = new ResultModel();
            if (ID.HasValue)
            {
                var result = _bD_NewsInfoService.Delete(ID.Value).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Delete news success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Delete news failed" };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            var opera = string.Format("删除新闻信息 ID:{0},操作结果:{1}", ID, resultModel.IsValid ? "成功" : "失败");
            LogPackage.InserAC_OperateLog(opera, "企业信息--新闻信息");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 创建系统信息

        /// <summary>
        ///     创建系统信息
        /// </summary>
        /// <param name="model">新闻信息对象</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(BD_NewsInfoModel model)
        {
            var _userName = UserInfo.CurrentUserName;
            if (ModelState.IsValid)
            {
                var resultModel = new ResultModel();
                if (model.ID != 0)
                {
                    model.UpdateBy = _userName;
                    model.UpdateDT = DateTime.Now;
                    var result = _bD_NewsInfoService.Update(model);
                    resultModel.Messages = new List<string>
                    {
                       result.Data > 0 ? "Modify news success" : "Modify news failed"
                    };
                    var opera = string.Format("修改新闻信息:{0},操作结果:{1}",JsonConverts.ToJson(model), result.Data > 0 ? "成功" : "失败");
                    LogPackage.InserAC_OperateLog(opera, "企业信息--新闻信息");

                }
                else
                {
                    model.IsDelete = 0;
                    model.IsCheck = (int)EIsCheck.ToAudit;
                    model.Releaser = _userName;
                    model.ReleaseDT = DateTime.Now;
                    model.AcUserID = 0;
                    model.UserID = 0;
                    model.CreateBy = _userName;
                    model.CreateDT = DateTime.Now;
                    model.UpdateBy = _userName;
                    model.UpdateDT = DateTime.Now;
                    model.SendStatus = 3;
                    resultModel.Messages = new List<string>
                    {
                        _bD_NewsInfoService.Add(model).Messages.Count == 0 ? "Add news success" : "Add news failed"
                    };
                }
                if (resultModel.IsValid)
                {
                    Response.Redirect("/BD_NewsInfo/Index");
                }
                else
                {
                    ViewBag.Messages = resultModel.Messages;
                    return View(model);
                }
            }
            else
            {
                DrowList(null);
            }
            return View(model);
        }

        #endregion

        #region 加载数据

        /// <summary>
        ///   加载数据
        /// </summary>
        /// <param name="id">新闻信息Id</param>
        /// <returns></returns>
        public ActionResult Create(int? id)
        {
            ViewBag.RootImage = ImagePath;
            BD_NewsInfoModel model = null;
            if (id.HasValue)
            {
                var result = _bD_NewsInfoService.GetBD_NewsInfoById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            else
            {
                model = new BD_NewsInfoModel();
            }
            DrowList(model);
            return PartialView(model);
        }

        #endregion

        private void DrowList(BD_NewsInfoModel model)
        {
            #region 新闻类型

            //1.惠卡动态,2惠粉公告,3惠粉消息
            var lang = new Dictionary<int, string>();
            var modelLang = _bD_NewsInfoService.GetBD_NewsTypelang(ACultureHelper.GetLanguageID).Data;
            if (modelLang != null)
            {
                foreach (var item in modelLang)
                {
                    lang.Add(item.ID, item.TypeName);
                }
            }
            var list = new List<SelectListItem>();

            foreach (var item in lang)
            {
                var info = new SelectListItem();
                if (model != null)
                {
                    if (item.Key == model.TypeID)
                    {
                        info.Selected = true;
                    }
                }
                info.Value = item.Key.ToString();
                info.Text = item.Value;
                list.Add(info);
            }

            ViewData["TypeID"] = list;

            #endregion
        }
        private void DrowCheckList(BD_NewsInfoModel model)
        {
            #region 审核状态

            //是否已经审核(1:审核通过,2:待审核,3.拒审)
            var lang = new Dictionary<int, string>();
            lang.Add((int)EIsCheck.Auditthrough, "Approved");
            lang.Add((int)EIsCheck.ToAudit, "Review Pending");
            lang.Add((int)EIsCheck.Refuse, "Refused");
            var list = new List<SelectListItem>();

            foreach (var item in lang)
            {
                var info = new SelectListItem();
                if (model != null)
                {
                    if (item.Key == 1)
                    {
                        info.Selected = true;
                    }
                }
                info.Value = item.Key.ToString();
                info.Text = item.Value;
                list.Add(info);
            }

            ViewData["IsCheck"] = list;

            #endregion
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="id">产品Id</param>
        /// <returns></returns>
        [HttpGet]
        public PartialViewResult NewsInfoCheck(int? id)
        {
            BD_NewsInfoModel model = null;
            if (id.HasValue)
            {
                var result = _bD_NewsInfoService.GetBD_NewsInfoById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                }
            }
            DrowCheckList(model);
            return this.PartialView(model);
        }

        /// <summary>
        /// 执行审核
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult NewsInfoCheck(int? ID, int IsCheck)
        {
            var _userName = UserInfo.CurrentUserName;
            var resultModel = new ResultModel();
            resultModel.Data = IsCheck;
            BD_NewsInfoModel model = new BD_NewsInfoModel();
            if (ID.HasValue)
            {
                model.ID = ID.Value;
                model.IsCheck = IsCheck;
                model.UpdateBy = _userName;
                model.UpdateDT = DateTime.Now;
                var result = _bD_NewsInfoService.UpdateState(model);
                resultModel.Messages = new List<string>
                    {
                       result.Data > 0 ? "Success" : "Failed"
                    };

                #region 推送消息
                try
                {
                    //审核状态为:审核通过并且操作成功,才发送消息 
                    if ((IsCheck == (int)EIsCheck.Auditthrough) && result.Data > 0)
                    {
                      //  new BD_NewsInfoService().SendMsgForHFByAsync(ID.Value.ToString());
                    }
                }
                catch (Exception ex)
                {
                    HKSJ.Common.LogHelper.logText(ex.ToString(), "PinMallMVC", 2);
                    var opera1 = string.Format("推送消息:{0},操作结果:{1}", "NewsInfoCheck", ex.Message);
                    LogPackage.InserAC_OperateLog(opera1, "企业信息--新闻信息");
                }
                #endregion

                var opera = string.Format("审核信息:{0},操作结果:{1}",JsonConverts.ToJson(model), result.Data > 0 ? "成功" : "失败");
                LogPackage.InserAC_OperateLog(opera, "企业信息--新闻信息");

            }
            return this.Json(resultModel);
        }
    }
}