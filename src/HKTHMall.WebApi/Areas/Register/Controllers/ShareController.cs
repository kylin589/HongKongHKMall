using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

using HKTHMall.Services.WebLogin.Impl;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.WebApi.VisitingCard;
using System.Threading;
using HKTHMall.Services.Common.MultiLangKeys;

namespace HKTHMall.WebApi.Areas.Register.Controllers
{
    public class ShareController : Controller
    {
        private LoginService _LoginService = new LoginService();
        // GET: Register/Share
        public ActionResult Index(string id, string lang)
        {
            ViewBag.ImgUrl = Url.Content("~/Html/images/hkmall_logo.png");
            if (string.IsNullOrEmpty(lang))
            {
                lang = "3";
            }
            if (!lang.Equals("1") && !lang.Equals("2") && !lang.Equals("3"))
            {
                lang = "";
            }
            int Lang = 0;
            if (!int.TryParse(lang, out Lang))
            {
                Lang = 3;
            }
            //语言
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(CultureHelper.GetLanguage(Lang));
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
            id = Encoding.UTF8.GetString(Convert.FromBase64String(string.IsNullOrEmpty(id) ? "" : id));
            long ID = 0;
            if (!long.TryParse(id, out ID))
            {
                ID = 0;
            }
            if (ID == 0)
            {
                return Content(CultureHelper.GetAPPLangSgring("HOME_SHOPPING_ERROR", Lang));
            }
            ViewBag.referID = ID;
            var result = new VisitingCardCreate().GeneratedVisitingCard(ID,Lang);
            if (result.flag == 0)
            {
                return Content(CultureHelper.GetAPPLangSgring("HOME_SHOPPING_ERROR", Lang));
            }
            ViewBag.Pic = result.rs.imgUrl;
            return View();
        }

        private string GetPicAddress(long ID)
        {
            return "";
        }
    }
}