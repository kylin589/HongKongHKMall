using HKTHMall.Admin.common;
using HKTHMall.Admin.Filters;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.Models;
using HKTHMall.Services.AC;
using HKTHMall.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class SuppliersController : HKBaseController
    {

        private ISuppliersService _suppliersService;
        private readonly ITHAreaService _thAreaService;
        public SuppliersController(ISuppliersService suppliersService, ITHAreaService thAreaService)
        {
            _suppliersService = suppliersService;
            _thAreaService = thAreaService;
        }

        //wuyf  供应商管理
        // GET: /Suppliers/
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult List(SalesSuppliersModel model)
        {
            //SearchAC_OperateLogModel logmodel = new SearchAC_OperateLogModel();
            model.PagedIndex = model.PagedIndex == 0 ? 0 : model.PagedIndex;
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            model.Lang = ACultureHelper.GetLanguageID;
            //加一天是为查询最后一天的数据


            //系统操作日志
            var result = this._suppliersService.GetSuppliers(model);
            List<SuppliersModel> ds = result.Data;
            var data = new
            {
                rows = ds,
                total = result.Data.TotalCount
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        ///数据
        ///wuyf 20150908
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Create(long? id)
        {
            SuppliersModel model = new SuppliersModel();
            ViewBag.thArea = _thAreaService.GetTHAreaByParentID(ACultureHelper.GetLanguageID, 0).Data;
            if (id.HasValue)
            {
                SalesSuppliersModel smodel = new SalesSuppliersModel();
                smodel.SupplierId = id.Value;
                smodel.PagedIndex = 0;
                smodel.PagedSize = 100;
                smodel.Lang = ACultureHelper.GetLanguageID;
                //查询列表 

                List<SuppliersModel> List = this._suppliersService.GetSuppliers(smodel).Data;

                if (List != null && List.Count > 0)
                {
                    model = List[0];
                    ViewBag.shiArea = _thAreaService.GetTHAreaByParentID(ACultureHelper.GetLanguageID, model.ShengTHAreaID).Data;
                    ViewBag.xianArea = SelectCommon.GetTHArea_lang(_thAreaService.GetTHAreaByParentID(ACultureHelper.GetLanguageID, model.ShiTHAreaID).Data as List<HKTHMall.Domain.Models.THArea_lang>);//_thAreaService.GetTHAreaByParentID(CultureHelper.GetLanguageID, model.ShiTHAreaID).Data;
                    //这里是用于修改通过密码验证使用的（修改是不修改密码的）
                     model.PassWord = "111111~a!";
                    

                }

            }
            else
            {
                ViewBag.xianArea = SelectCommon.GetTHArea_lang(null);
            }
            return PartialView(model);
        }


        /// <summary>
        /// 新增,修改
        /// wuyf 20150908
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(SuppliersModel model)
        {
            ViewBag.thArea = _thAreaService.GetTHAreaByParentID(ACultureHelper.GetLanguageID, 0).Data;
            ViewBag.xianArea = SelectCommon.GetTHArea_lang(null);

            if (ModelState.IsValid)
            {
               
                ResultModel resultModel = new ResultModel();

                if (model.SupplierId > 0)
                {

                    model.UpdateBy = UserInfo.CurrentUserName;
                    model.UpdateDT = DateTime.Now;
                    var result = _suppliersService.UpdateSuppliers(model).IsValid;

                    resultModel.Messages = new List<string> { result == true ? " success!" : " failed!" };
                    var opera = string.Empty;
                    opera = string.Format("修改供应商管理:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
                    LogPackage.InserAC_OperateLog(opera, "修改供应商管理");

                }
                else
                {
                    SalesSuppliersModel smodel = new SalesSuppliersModel();
                    smodel.Mobile = model.Mobile;
                    smodel.PagedIndex = 0;
                    smodel.PagedSize = 2;
                    var bl = Mobile(smodel);
                    if (bl)
                    {
                        model.PassWord = FormsAuthentication.HashPasswordForStoringInConfigFile(model.PassWord.Trim(), "MD5");
                        model.SupplierId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
                        model.CreateBy = UserInfo.CurrentUserName;
                        model.CreateDT = DateTime.Now;
                        model.UpdateBy = UserInfo.CurrentUserName;
                        model.UpdateDT = DateTime.Now;
                        var result = _suppliersService.AddSuppliers(model).IsValid;
                        resultModel.Messages = new List<string> { result == true ? " success!" : " failed!" };
                        var opera = string.Empty;
                        opera = string.Format("添加供应商管理:{0},操作结果:{1}", JsonConverts.ToJson(model), resultModel.Messages);
                        LogPackage.InserAC_OperateLog(opera, "添加供应商管理");
                    }
                    else
                    {
                        resultModel.IsValid = false;
                        resultModel.Messages = new List<string> { " Mobile phone number already exists!" };
                    }
                }


                return Json(resultModel, JsonRequestBehavior.AllowGet);
            }

            return PartialView(model);
        }

        /// <summary>
        /// 验证手机号码是否存在
        /// wuyf 20150925
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool Mobile(SalesSuppliersModel model)
        {
            bool bl = true;
            List<SuppliersModel> List = this._suppliersService.GetSuppliers(model).Data;

            if (List != null && List.Count > 0)
            {
                bl = false;
            }

            return bl;
        }

        /// <summary>
        /// 省份下拉框数据获取
        /// wuyf 20150908
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>

        public JsonResult GetTHAreaSelect(int parentID)
        {
            var result = _thAreaService.GetTHAreaByParentID(ACultureHelper.GetLanguageID, parentID).Data;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public string DeleteSuppliers(long? ID)
        {
            SuppliersModel model = new SuppliersModel();
            if (ID.HasValue)
            {
                model.SupplierId = ID.Value;
                var result = this._suppliersService.DeleteSuppliers(model).Data;
                return result == true ? "Delete success！" : "Delete failed！";
            }

            return "Delete failed！";

        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public JsonResult ResetPassword(long? ID)
        {
            SuppliersModel model = new SuppliersModel();
            model.PassWord = FormsAuthentication.HashPasswordForStoringInConfigFile("111111!q", "MD5");
            var result = new ResultModel();
            if (ID.HasValue)
            {
                model.SupplierId = ID.Value;
                 result = this._suppliersService.UpdateSuppliersPassWord(model);
                 result.Messages =new List<string> { result.IsValid==true ? " success!" : " failed!"};
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }


    }
}