using HKTHMall.Admin.common;
using HKTHMall.Core;
using HKTHMall.Core.Controllers;
using HKTHMall.Domain.Models.LoginLog;
using HKTHMall.Domain.Models.User;
using HKTHMall.Services.LoginLog;
using HKTHMall.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BrCms.Framework.Logging;
using HKTHMall.Core.Utils;
using HKTHMall.Domain.WebModel.Models.YH;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Services.Orders;
using Autofac;


namespace HKTHMall.Admin.Controllers
{
    public class AC_UserController : HKBaseController
    {
       private IAC_UserService _ac_UserService;
       private IUserLoginLogService _userLoginLogService;
       private ILogger _Logger;
       private readonly IOrderService _acOrederService;
      // private readonly ILogger Logger = BrEngineContext.Current.Resolve<ILogger>();
       public AC_UserController(IUserLoginLogService userLoginLogService, IAC_UserService ac_UserService, ILogger Logger,IOrderService acOrederService)
        {
            this._userLoginLogService = userLoginLogService;
            this._ac_UserService = ac_UserService;
            this._Logger = Logger;
            this._acOrederService = acOrederService;
        }

        public ActionResult Login()
        {
            
           // HtmlToPDF.HtmlToPdf(outputPath, fileName, url);

            return View();
        }
        /// <summary>
        /// 保存登陆信息和日记
        /// </summary>
        /// <param name="model"></param>
        public void GetAC_UserByUserName(AC_UserModel model)
        {
            UserInfoModel info = new UserInfoModel();
            info.UserID = model.UserID;
            info.UserName = model.UserName;
            info.RoleID = model.RoleID;
            Session["UserInfo"] = info;//登陆成功保存用户信息

            //插入登陆时间日记
            var result = LogPackage.InserUserLoginLog("Thailand Mall Backgroud", 1);
        }

        public JsonResult IsLogin()
        {
            var UserName = Request["UserName"];
            var PassWord = Request["PassWord"];
            string type = "1";//用户不存在
            string Messages = "Users do not exist";

            try
            {
                AC_UserModel model = this._ac_UserService.GetAC_UserByUserName(UserName).Data;

                if (model != null)
                {
                    if (MD5.Equals(model.Password, FormsAuthentication.HashPasswordForStoringInConfigFile(PassWord, "MD5")) && model.UserMode == 1)
                    {
                        type = "3";//登陆成功
                        Messages = "Login success";
                        GetAC_UserByUserName(model);
                    }
                    else
                    {
                        type = "2";//密码不正确
                        Messages = "Password wrong or account locked";
                    }
                }
                var data = new
                {

                    logintype = type,
                    Messages = Messages,
                };
                return Json(data);
            }
            catch (Exception ex)
            {
                var data = new
                {

                    logintype = type,
                    Messages = Messages,
                };

                var opera = string.Format("登录错误-用户名:{0},错误结果:{1}", UserName, ex.Message);
                LogPackage.InserAC_OperateLog(opera, "Login error");

                return Json(data);
                
            }
            
            
        }

        public JsonResult UserOut()
        {
            string type = "1";//用户不存在
            string Messages = "Logout success";
            try
            {
                //Session["UserInfo"] =null;
                Session.Remove("UserInfo");
                var data = new
                {

                    logintype = type,
                    Messages = Messages,
                };
                return Json(data);
            }
            catch (Exception ex)
            {
                this._Logger.Error(this.GetType().Name,ex.ToString());
                var opera = string.Format("退出UserOut:{0},错误结果:{1}", "UserOut", ex.Message);
                LogPackage.InserAC_OperateLog(opera, "退出错误UserOut");
                var data = new
                {

                    logintype = type,
                    Messages = Messages,
                };

                return Json(data);
            }
            
            
        }

        public ActionResult SelectDaYin(string orderId)
        {
            
            ViewBag.orderId = orderId;
            dynamic order;
            YH_MerchantInfoView YH_MerchantInfo = new YH_MerchantInfoView();
            string orderStatusStr = "";
            string payTypeStr = "";
            var list = new List<HKTHMall.Domain.AdminModel.Models.Orders.OrderDetailsModel>();
            OrderModel model = new OrderModel();
            string Address = "";
            var imgpath = "";
            try
            {
                imgpath = Code.BarCode.GetBarCode.GetTxm(orderId);
                ObjesToPdf.Orderinfo(orderId, 3, out order, out YH_MerchantInfo, out orderStatusStr, out payTypeStr);
                model = order as OrderModel;
                //订单分页详情(商品的信息)
                list = _acOrederService.GetPagingOrderDetails(Convert.ToInt64(orderId), 3).Data as List<HKTHMall.Domain.AdminModel.Models.Orders.OrderDetailsModel>;

                IUserAddressService userAddressService = BrCms.Framework.Infrastructure.BrEngineContext.Current.Resolve<IUserAddressService>();
                //省市区
                var userAddress = userAddressService.GetTHAreaAreaName(model.THAreaID, 3).Data;
                Address = userAddress + model.DetailsAddress;
            }
            catch (Exception ex)
            {

                var opera = string.Format("显示打印详情错误:{0},操作结果:{1}", ex.Message, "失败");
                LogPackage.InserAC_OperateLog(opera, "PDF");
            }

            ViewBag.imgpath = imgpath;
            ViewBag.list = list;
            ViewBag.ordermodel = model;
            ViewBag.YH_MerchantInfo = YH_MerchantInfo;
            ViewBag.orderStatusStr = orderStatusStr;
            ViewBag.payTypeStr = payTypeStr;
            ViewBag.Address = Address;

            //return PartialView();
            return View();
        }

	}
}