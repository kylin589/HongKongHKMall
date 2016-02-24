using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.AdminModel.Models.Keywork;
using HKTHMall.Domain.Models;
using HKTHMall.Services.Keywork;
using HKTHMall.Admin.common;
using HKTHMall.Domain.Enum;
using HKTHMall.Core.Utils;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class FloorKeywordController : HKBaseController
    {
        private readonly IFloorKeywordService _floorKeywordService;

        private readonly HKTHMall.Services.YHUser.IMyCollectionService _myCollectionService;

        public FloorKeywordController(IFloorKeywordService floorKeywordService, HKTHMall.Services.YHUser.IMyCollectionService myCollectionService)
        {
            _floorKeywordService = floorKeywordService;
            _myCollectionService = myCollectionService;
        }

        // GET: ParameterSet
        public ActionResult Index()
        {
            return View();
        }

        #region 查询关键字

        /// <summary>
        ///     查询关键字
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public JsonResult List(SearchFloorKeywordModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var paramList =
                _floorKeywordService.Select(model);
            var data = new { rows = paramList.Data, total = paramList.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 删除关键字

        /// <summary>
        ///     删除关键字
        /// </summary>
        /// <param name="ParamenterID">关键字Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-8</remarks>
        public JsonResult Delete(long? floorKeywordId)
        {
            var resultModel = new ResultModel();
            if (floorKeywordId.HasValue)
            {
                var result = _floorKeywordService.Delete(floorKeywordId.Value).Data;
                if (result > 0)
                {
                    resultModel.IsValid = true;
                    resultModel.Messages = new List<string> { "Keyword delete success" };
                }
                else
                {
                    resultModel.IsValid = false;
                    resultModel.Messages = new List<string> { "Delete key Failure" };
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Key ID error" };
            }
            var opera = string.Format("删除关键字 floorKeywordId:{0},结果:{1}", floorKeywordId, resultModel.IsValid ? "成功" : "失败");
            LogPackage.InserAC_OperateLog(opera, "广告管理--关键字管理");
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 创建关键字

        /// <summary>
        ///     创建关键字
        /// </summary>
        /// <param name="model">关键字对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(FloorKeywordModel model)
        {
            if (ModelState.IsValid)
            {
                var updateName = UserInfo.CurrentUserName;
                var resultModel = new ResultModel();
                if (model.FloorKeywordId != 0)
                {
                    model.UpdateBy = updateName;
                    model.UpdateDT = DateTime.Now;
                    var result = _floorKeywordService.Update(model);
                    if (result.IsValid)
                    {
                        resultModel.IsValid = true;
                        resultModel.Messages = new List<string> { "Keyword change success" };
                    }
                    else
                    {
                        resultModel.IsValid = false;
                        resultModel.Messages = new List<string> { result.Messages[0] };
                    }
                    string opera = string.Format("修改系统功能参数:FloorKeywordId={0},结果:{1}", model.FloorKeywordId, resultModel.IsValid ? "成功" : "失败");
                    LogPackage.InserAC_OperateLog(opera, "广告管理--关键字管理");
                }
                else
                {


                    model.FloorKeywordId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    model.CreateBy = updateName;
                    model.CreateDT = DateTime.Now;
                    model.UpdateBy = updateName;
                    model.UpdateDT = DateTime.Now;
                    model.Sorts = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                    model.PlaceCode = 0;
                    var result = _floorKeywordService.Add(model);
                    if (result.IsValid)
                    {
                        resultModel.IsValid = true;
                        resultModel.Messages = new List<string> { "Add keyword success" };
                    }
                    else
                    {
                        resultModel.IsValid = false;
                        resultModel.Messages = new List<string> { result.Messages[0] };
                    }

                }
                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                DrowList(null);
            }
            return PartialView(model);
        }

        #endregion

        #region 加载数据

        /// <summary>
        ///     加载数据
        /// </summary>
        /// <param name="id">对象系统Id</param>
        /// <returns></returns>
        public ActionResult Create(long? id)
        {
            FloorKeywordModel model = null;
            var result = new ResultModel();
            if (id.HasValue)
            {
                result = _floorKeywordService.GetFloorKeywordById(id.Value);
                if (result.Data != null)
                {
                    model = result.Data;
                    string langName = "";
                    LanguageType ly = (LanguageType)Enum.Parse(typeof(LanguageType), model.LanguageID.ToString());
                    //update by liujc
                    langName = ACultureHelper.GetLanguageName(ly);
                    //switch (ly)
                    //{
                    //    case LanguageType.zh_CN:
                    //        langName = "Chinese";//中文
                    //        break;
                    //    case LanguageType.en_US:
                    //        langName = "English";//英文
                    //        break;
                    //    case LanguageType.th_TH:
                    //        langName = "Thai";//泰文
                    //        break;
                    //    case LanguageType.zh_HK:
                    //        langName = "Hongkong";//中文，香港地区 add by liujc
                    //        break;
                    //    default:
                    //        break;
                    //}
                    ViewBag.LangName = langName;
                }
            }
            else
            {
                model = new FloorKeywordModel();
                model.Sorts = 1;
            }

            DrowList(model);

            return PartialView(model);
        }

        #endregion

        private void DrowList(FloorKeywordModel model)
        {
            #region 语言

            //语言 update by liujc
            var lang = ACultureHelper.GetLanguageList();
            //var lang = new Dictionary<int, string>();
            //lang.Add(3, "Thai");//泰文
            //lang.Add(1, "Chinese");//中文
            //lang.Add(2, "English");//英文
            //lang.Add(4, "Hongkong");//香港 add by liujc
            var listLang = new List<SelectListItem>();

            foreach (var item in lang)
            {
                var info = new SelectListItem();
                if (model != null)
                {
                    if (item.Key == model.LanguageID)
                    {
                        info.Selected = true;
                    }
                }
                info.Value = item.Key.ToString();
                info.Text = item.Value;
                listLang.Add(info);
            }

            ViewData["langs"] = listLang;

            #endregion
        }

        #region 关键字管理
        /// <summary>
        /// 关键字管理
        /// </summary>
        /// <param name="SalesProductId">关键字管理ID</param>
        /// <param name="rowId">行数下标</param>
        /// <param name="sortType">1、上移；2、下移</param>
        /// <returns></returns>
        public JsonResult UpdatePlace(long? FloorKeywordId, int rowId, int sortType, string KeyWordName, string LanguageID, int PagedIndex, int PagedSize)
        {
            LanguageID = string.IsNullOrEmpty(LanguageID) ? "0" : LanguageID;
            var resultModel = new ResultModel();
            resultModel.Messages = new List<string> { "Move success." };
            if (FloorKeywordId.HasValue)
            {
                List<FloorKeywordModel> paramList =
               _floorKeywordService.Select(new SearchFloorKeywordModel
               {
                   PagedIndex = PagedIndex,
                   PagedSize = PagedSize,
                   KeyWordName = KeyWordName,
                   LanguageID = int.Parse(LanguageID)
               }).Data;
                if (paramList != null && paramList.Count > 0)
                {
                    switch (sortType)
                    {
                        case 1://上移
                            if (rowId > 0)
                            {
                                _floorKeywordService.UpdatePlace(paramList[rowId].FloorKeywordId, (int)paramList[rowId - 1].Sorts);
                                _floorKeywordService.UpdatePlace(paramList[rowId - 1].FloorKeywordId, (int)paramList[rowId].Sorts);
                            }
                            else
                            {
                                resultModel.IsValid = false;
                                resultModel.Messages = new List<string> { "This is top line." };
                            }
                            break;
                        case 2://下移
                            if (rowId < (paramList.Count - 1))
                            {
                                _floorKeywordService.UpdatePlace(paramList[rowId].FloorKeywordId, (int)paramList[rowId + 1].Sorts);
                                _floorKeywordService.UpdatePlace(paramList[rowId + 1].FloorKeywordId, (int)paramList[rowId].Sorts);
                            }
                            else
                            {
                                resultModel.IsValid = false;
                                resultModel.Messages = new List<string> { "This is last line!." };
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                resultModel.IsValid = false;
                resultModel.Messages = new List<string> { "Failed, ID error." };
            }
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}