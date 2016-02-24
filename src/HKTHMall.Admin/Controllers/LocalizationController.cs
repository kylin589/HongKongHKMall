using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Admin.Filters;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.AdminModel.Models.Localization;
using HKTHMall.Domain.Models;
using HKTHMall.Services.Localization;
using BrCms.Framework.Mvc.Extensions;

namespace HKTHMall.Admin.Controllers
{
    [UserAuthorize]
    public class LocalizationController : HKBaseController
    {
        private readonly ILanguageService _languageService;

        public LocalizationController(ILanguageService languageService)
        {
            this._languageService = languageService;
        }

        public ActionResult Index()
        {
            ViewBag.DataTypes = BrCms.Framework.Mvc.Extensions.MvcExtension.ToSelectList<LanguageModel.EDataType>();
            return this.View();
        }

        public JsonResult Search(SearchLanguageModel model)
        {
            var result = this._languageService.Search(model);
            return this.Json(new { rows = result.Data, total = result.Data.TotalCount }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult AddOrUpdate(int? id)
        {
            LanguageModel language = null;
            var dataType = LanguageModel.EDataType.Web;
            if (id.HasValue)
            {
                language = this._languageService.Search(new SearchLanguageModel
                {
                    LanguageId = id,
                    PagedIndex = 0,
                    PagedSize = 1
                }).Data[0];
                dataType = (LanguageModel.EDataType)language.DataType;
            }
            else
            {
                language = new LanguageModel();
                language.DataType = (int)LanguageModel.EDataType.Web;
            }

          
            ViewBag.DataTypes = dataType.ToSelectList(selected: language.DataType);

            return this.PartialView(language);
        }

        [ValidateInput(false)]
        public JsonResult Add(AddLanguageModel model)
        {
            model.CreateBy = UserInfo.CurrentUserName;
            model.CreateDT = DateTime.Now;
            model.NameTH = (model.NameTH == null ? "" : model.NameTH);//add by liujc
            model.NameOther = model.NameChs;
            var result = this._languageService.Add(model);
            return this.Json(result);
        }

        [ValidateInput(false)]
        public JsonResult Update(UpdateLanguageModel model)
        {
            model.UpdateBy = UserInfo.CurrentUserName;
            model.UpdateDT = DateTime.Now;
            model.NameTH = (model.NameTH == null ? "" : model.NameTH);//add by liujc
            model.NameOther = model.NameChs;
            var result = this._languageService.Update(model);
            return this.Json(result);
        }

        public JsonResult Delete(List<long> ids)
        {
            var result = this._languageService.Delete(ids);
            return this.Json(result);
        }

        public JsonResult ImportExcel()
        {
            var file = this.Request.Files[0];
            if (file != null && file.ContentLength > 0 && file.FileName.Contains(".xlsx"))
            {
                var result = this._languageService.ImportExcel(file.InputStream, UserInfo.CurrentUserName);
                return this.Json(result);
            }
            return this.Json(new ResultModel
            {
                Messages = new List<string>
                {
                    "文件长度为0"
                }
            });
        }

        public FileResult Export(List<long> ids, bool isAll = false)
        {
            ResultModel resultModel = this._languageService.Export(ids, isAll);
            byte[] fileBytes = resultModel.Data;
            return this.File(fileBytes, "text/xls", DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx");
        }

        public FileResult ExportCondition(SearchLanguageModel searchModel)
        {
            ResultModel resultModel = this._languageService.Export(searchModel);
            byte[] fileBytes = resultModel.Data;
            return this.File(fileBytes, "text/xls", DateTime.Now.ToString("yyyyMMddhhmmss") + ".xlsx");
        }
    }
}