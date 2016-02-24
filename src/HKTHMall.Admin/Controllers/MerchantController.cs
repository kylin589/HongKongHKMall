using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Admin.common;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.AdminModel.Models.Merchant;
using HKTHMall.Services.Merchant;

namespace HKTHMall.Admin.Controllers
{
    public class MerchantController : HKBaseController
    {
        private readonly IMerchantService _merchantService;

        public MerchantController(IMerchantService merchantService)
        {
            this._merchantService = merchantService;
        }

        // GET: Business
        public ActionResult Index()
        {
            return this.View();
        }

        public JsonResult Search(SearchMerchantModel model)
        {
            model.LanguageId = ACultureHelper.GetLanguageID;
            var result = this._merchantService.Search(model);
            return this.Json(new {rows = result.Data, total = result.Data.TotalCount}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddOrUpdate(long? id)
        {
            var model = new MerchantModel();
            if (id.HasValue)
            {
                model = this._merchantService.GetMerchantByMerchantId(id.Value).Data;
            }
            return this.View(model);
        }

        public JsonResult Add(AddMerchantModel model)
        {
            var result = this._merchantService.Add(model);
            return this.Json(result);
        }

        public JsonResult Update(UpdateMerchantModel model)
        {
            var result = this._merchantService.Update(model);
            return this.Json(result);
        }

        public JsonResult Delete(IList<long> ids)
        {
            var result = this._merchantService.Delete(ids);
            return this.Json(result);
        }
    }
}