using System;
using System.Collections.Generic;
using System.Web.Mvc;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Services.Users;
using HKTHMall.Admin.Filters;

namespace HKTHMall.Admin.Controllers
{
    /// <summary>
    /// 留言控制器
    /// </summary>
    [UserAuthorize]
    public class MessageController : HKBaseController
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }
        public ActionResult Index()
        {
            return View();
        }

        #region 查询留言信息列表

        /// <summary>
        /// 查询留言信息列表
        /// </summary>
        /// <param name="model">搜索实体对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-16</remarks>
        public JsonResult List(SearchMessageModel model)
        {
            model.PagedSize = model.PagedSize == 0 ? 10 : model.PagedSize;
            var list = _messageService.Select(model);
            var data = new { rows = list.Data, total = list.Data.TotalCount };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}