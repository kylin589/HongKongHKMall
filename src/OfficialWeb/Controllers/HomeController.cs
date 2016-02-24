using HKTHMall.Domain.AdminModel.Models.New;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Services.New;
using HKTHMall.Services.Products;
using HKTHMall.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfficialWeb.Controllers
{
    public class HomeController : Controller
    {
        private INewInfoService _newInfoService;
        private IProductImageService _productImageService;
        private IMessageService _messageService;
        public HomeController(INewInfoService newInfoService, IProductImageService productImageService, IMessageService messageService)
        {
            this._newInfoService = newInfoService;
            this._productImageService = productImageService;
            this._messageService = messageService;
        }

        /// <summary>
        /// 首页
        /// zhoub 20150825
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //推荐新闻数据获取
            SearchNewInfoModel newInfoModelOne = new SearchNewInfoModel();
            newInfoModelOne.IsRecommend = 1;
            newInfoModelOne.PagedIndex =0;
            newInfoModelOne.PagedSize =1;
            ViewData.Add("NewInfoOne", _newInfoService.GetNewInfoList(newInfoModelOne).Data);
            //新闻数据获取
            SearchNewInfoModel newInfoModelTwo = new SearchNewInfoModel();
            newInfoModelTwo.IsRecommend = 0;
            newInfoModelTwo.PagedIndex =0;
            newInfoModelTwo.PagedSize = 4;
            ViewData.Add("NewInfoOneTwo", _newInfoService.GetNewInfoList(newInfoModelTwo).Data);
            //产品
            SearchProductImageModel productImageModelOne = new SearchProductImageModel();
            productImageModelOne.ImageType = 1;
            productImageModelOne.PagedIndex = 0;
            productImageModelOne.PagedSize = 100;
            ViewData.Add("ProductImageOne", _productImageService.Select(productImageModelOne).Data);
            //视频
            SearchProductImageModel productImageModelTwo = new SearchProductImageModel();
            productImageModelTwo.ImageType = 2;
            productImageModelTwo.PagedIndex = 0;
            productImageModelTwo.PagedSize = 100;
            ViewData.Add("ProductImageTwo", _productImageService.Select(productImageModelTwo).Data);
            return View();
        }

        /// <summary>
        /// 用户留言
        /// zhoub 20150825
        /// </summary>
        /// <returns></returns>
        public JsonResult AddMessage()
        {
            MessageModel model = new MessageModel();
            model.MsgPerson = Request.Params["msgPerson"];
            model.Email =Request.Params["email"];
            model.subject = Request.Params["subject"];
            model.MsgContent = Request.Params["msgContent"];
            model.CreateDT = DateTime.Now;
            var resultModel = _messageService.AddMessage(model);
            return Json(resultModel, JsonRequestBehavior.AllowGet);
        }
    }
}
