using HKTHMall.Services.Common.MultiLangKeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace HKTHMall.Web.Controllers
{
    public class HelpController : BaseController
    {
        // GET: Help
        public ActionResult Index()
        {
            return View();
        }

        #region 新手指南
        //免费注册
        public ActionResult Register()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Register_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Register_TH", "Help");
            }
            return View();
        }

        public ActionResult Register_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Register", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Register_TH", "Help");
            }
            return View();
        }

        public ActionResult Register_TH()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Register", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Register_EN", "Help");
            }
            return View();
        }

        //关于惠粉
        public ActionResult About()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("About_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("About_TH", "Help");
            }
            return View();
        }

        public ActionResult About_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("About", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("About_TH", "Help");
            }
            return View();
        }

        public ActionResult About_TH()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("About_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("About", "Help");
            }
            return View();
        }

        //关于惠粉
        public ActionResult AboutAgent()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("AboutAgent_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("AboutAgent_TH", "Help");
            }
            return View();
        }

        public ActionResult AboutAgent_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("AboutAgent", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("AboutAgent_TH", "Help");
            }
            return View();
        }

        public ActionResult AboutAgent_TH()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("AboutAgent_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("AboutAgent", "Help");
            }
            return View();
        }

        //惠粉盈利模式//暂时拿掉
        public ActionResult HFProfitModel()
        {
            return View();
        }

        //购物流程
        public ActionResult Shopping()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Shopping_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Shopping_TH", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Shopping_HK", "Help");
            }
            return View();
        }

        public ActionResult Shopping_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Shopping", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Shopping_TH", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Shopping_HK", "Help");
            }
            return View();
        }

        public ActionResult Shopping_TH()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Shopping_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Shopping", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Shopping_HK", "Help");
            }
            return View();
        }
        public ActionResult Shopping_HK()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Shopping_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Shopping", "Help");
            }
            return View();
        }
        //支付方式
        public ActionResult Pay()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Pay_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Pay_TH", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Pay_HK", "Help");
            }
            return View();
        }

        public ActionResult Pay_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Pay", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Pay_TH", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Pay_HK", "Help");
            }
            return View();
        }

        public ActionResult Pay_TH()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Pay_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Pay", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Pay_HK", "Help");
            }
            return View();
        }
        public ActionResult Pay_HK()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Pay_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Pay", "Help");
            }
            return View();
        }
        #endregion 购物指南

        #region 配送服务

        //配送说明
        public ActionResult Distribution()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Distribution_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Distribution_TH", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Distribution_HK", "Help");
            }
            return View();
        }

        public ActionResult Distribution_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Distribution", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Distribution_TH", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Distribution_HK", "Help");
            }
            return View();
        }

        public ActionResult Distribution_TH()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Distribution", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Distribution_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Distribution_HK", "Help");
            }
            return View();
        }
        public ActionResult Distribution_HK()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Distribution", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Distribution_EN", "Help");
            }
            return View();
        }
        //签收验货
        public ActionResult Receive()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Receive_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Receive_TH", "Help");
            }
            return View();
        }

        public ActionResult Receive_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Receive", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Receive_TH", "Help");
            }
            return View();
        }

        public ActionResult Receive_TH()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Receive", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Receive_EN", "Help");
            }
            return View();
        }

        //退换货政策
        public ActionResult Return()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Return_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Return_TH", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Return_HK", "Help");
            }
            return View();
        }

        public ActionResult Return_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Return", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Return_TH", "Help");
            } if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Return_HK", "Help");
            }
            return View();
        }

        public ActionResult Return_TH()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Return", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Return_EN", "Help");
            } if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Return_HK", "Help");
            }
            return View();
        }
        public ActionResult Return_HK()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Return", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Return_EN", "Help");
            }
            return View();
        }
        //退换货流程
        public ActionResult Return2()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Return2_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Return2_TH", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Return2_HK", "Help");
            }
            return View();
        }

        public ActionResult Return2_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Return2", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Return2_TH", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Return2_HK", "Help");
            }
            return View();
        }

        public ActionResult Return2_TH()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Return2", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Return2_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Return2_HK", "Help");
            }
            return View();
        }
        public ActionResult Return2_HK()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Return2", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Return2_EN", "Help");
            }
            return View();
        }

        //退换货申请
        public ActionResult ReturnApplication()
        {
            return View();
        }

        //账户充值

        #endregion 配送服务

        #region 客户服务

        //服务协议
        public ActionResult Agreement()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Agreement_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Agreement_TH", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Agreement_HK", "Help");
            }
            return View();
        }

        public ActionResult Agreement_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Agreement", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Agreement_TH", "Help");
            } if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Agreement_HK", "Help");
            }
            return View();
        }

        public ActionResult Agreement_TH()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Agreement", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Agreement_EN", "Help");
            } if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Agreement_HK", "Help");
            }
            return View();
        }
        public ActionResult Agreement_HK()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Agreement", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Agreement_EN", "Help");
            }
            return View();
        }

        //安全支付
        public ActionResult SafePay()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("SafePay_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("SafePay_TH", "Help");
            }
            return View();
        }

        public ActionResult SafePay_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("SafePay", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("SafePay_TH", "Help");
            }
            return View();
        }

        public ActionResult SafePay_TH()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("SafePay", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("SafePay_EN", "Help");
            }
            return View();
        }

        //常见问题
        public ActionResult Question()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Question_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Question_TH", "Help");
            } if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Question_HK", "Help");
            }
            return View();
        }

        public ActionResult Question_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Question", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("Question_TH", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Question_HK", "Help");
            }
            return View();
        }

        public ActionResult Question_TH()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Question", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Question_EN", "Help");
            } if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("Question_HK", "Help");
            }
            return View();
        }
        public ActionResult Question_HK()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("Question", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("Question_EN", "Help");
            }
            return View();
        }

        #endregion 客户服务

        #region 关于我们

        //网站介绍
        public ActionResult WebsiteAbout()
        {
            return View();
        }

        //联系我们
        public ActionResult ContactUs()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("ContactUs_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("ContactUs_TH", "Help");
            } if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("ContactUs_HK", "Help");
            }
            return View();
        }

        public ActionResult ContactUs_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("ContactUs", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("ContactUs_TH", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("ContactUs_HK", "Help");
            }
            return View();
        }

        public ActionResult ContactUs_TH()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("ContactUs", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("ContactUs_EN", "Help");
            } if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("ContactUs_HK", "Help");
            }
            return View();
        }
        public ActionResult ContactUs_HK()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("ContactUs", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("ContactUs_EN", "Help");
            }
            return View();
        }

        //惠卡承诺
        public ActionResult HuikaPromise()
        {
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("HuikaPromise_EN", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("HuikaPromise_TH", "Help");
            } if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("HuikaPromise_HK", "Help");
            }
            return View();
        }

        public ActionResult HuikaPromise_EN()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("HuikaPromise", "Help");
            }
            if (CultureHelper.GetLanguageID() == 3)
            {
                return RedirectToAction("HuikaPromise_TH", "Help");
            }
            if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("HuikaPromise_HK", "Help");
            }
            return View();
        }

        public ActionResult HuikaPromise_TH()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("HuikaPromise", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("HuikaPromise_EN", "Help");
            } if (CultureHelper.GetLanguageID() == 4)
            {
                return RedirectToAction("HuikaPromise_HK", "Help");
            }
            return View();
        }
        public ActionResult HuikaPromise_HK()
        {
            if (CultureHelper.GetLanguageID() == 1)
            {
                return RedirectToAction("HuikaPromise", "Help");
            }
            if (CultureHelper.GetLanguageID() == 2)
            {
                return RedirectToAction("HuikaPromise_EN", "Help");
            }
            return View();
        }

        #endregion 关于我们

    }
}