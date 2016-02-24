using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.New;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.WebModel.Models.Users;
using HKTHMall.Services.New;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HKTHMall.Services.YHUser;
using HKTHMall.Services.Common.MultiLangKeys;

namespace HKTHMall.Web.Controllers
{
    public class NewsController : BaseController
    {
        private INewInfoService _newInfoService;
        private readonly IYH_UserService _IYH_UserService;
        public NewsController(INewInfoService iNewInfoService,IYH_UserService iYH_UserService)
        {
            _newInfoService = iNewInfoService;
            _IYH_UserService = iYH_UserService;
        }
        // GET: News
        public ActionResult Index()
        {
            ViewBag.SalesData = _newInfoService.GetIndexNews(8, 1, 1, null).Data;
            ViewBag.NewsData = _newInfoService.GetIndexNews(8, 1, 0, null).Data;
            return View();
        }

        public ActionResult Details(long Id)
        {
            var news =_newInfoService.GetNewsById(Id).Data;
            if (news == null)
            {
                return View("~/Views/Home/NotFound.cshtml");
            }
            ViewBag.News = news;
            return View();
        }

        public PartialViewResult _NewsList(int PageIndex,int PageSize,int Type)
        {
            if (Type == 1 || Type == 2)
            {
                ViewBag.ListData = _newInfoService.GetIndexNews(PageSize, PageIndex, Type - 1, null).Data;
            }
            return PartialView();     
        }

        [HttpPost]
        public JsonResult AddMailSub(MailSubscriptionModel model)
        {
            ResultModel result;
            try
            {
                if (!RegexUtil.IsEmail(model.Email))
                {
                    result = new ResultModel();
                    result.IsValid = false;
                    result.Messages = new List<string> { CultureHelper.GetLangString("ACCOUNT_USERINFO_ADDRESS_EMAILERROR") }; //邮箱格式不正确
                }
                else
                {
                    if (_IYH_UserService.HasEmailSubd(model.Email))
                    {
                        result = new ResultModel();
                        result.IsValid = false;
                        result.Messages = new List<string> { CultureHelper.GetLangString("HK_EmailHasBeenSubed") }; //邮箱已订阅
                    }
                    else
                    {
                        model.SubDate = DateTime.Now;
                        model.Ip = ToolUtil.GetRealIP();
                        if (this.UserID > 0)
                        {
                            model.UserID = this.UserID;
                        }
                        model.SubType = 1;
                        model.SendStatus = 1;
                        result = _IYH_UserService.AddEmailSub(model);
                        result.IsValid = true;
                        result.Messages = new List<string> { CultureHelper.GetLangString("HOME_FOOTER_SUCCDINGYUE") };//订阅成功
                    }
                }
            }
            catch (Exception e)
            {
                result = new ResultModel();
                result.IsValid = false;
                result.Messages = new List<string> { CultureHelper.GetLangString("SYSTEM_INNERERROR") }; //系统错误
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}