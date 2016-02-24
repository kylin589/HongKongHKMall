using HKTHMall.Domain.AdminModel.Models.New;
using HKTHMall.Services.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HKTHMall.WebApi.Areas.Huifen.Controllers
{
    public class BD_NewsInfoController : Controller
    {
        private readonly BD_NewsInfoService _bD_NewsInfoService = new BD_NewsInfoService();
        public static readonly string ImagePath = HKSJ.Common.ConfigHelper.GetConfigString("ImagePath");

        // GET: BD_NewsInfo
        public ActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 新闻详情页面
        /// </summary>
        /// <param name="id">新闻Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-8-27</remarks>
        public ActionResult Details(int? id)
        {
            BD_NewsInfoModel model = null;
            if (id.HasValue)
            {
                model = _bD_NewsInfoService.GetBD_NewsInfoById(id.Value).Data;
                if (model != null)
                {
                    model.imagePicth = ImagePath;
                }
            }
            return View(model);
        }
    }
}