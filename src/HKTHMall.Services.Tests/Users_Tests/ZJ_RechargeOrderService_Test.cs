using BrCms.Framework.Infrastructure;
using HKTHMall.Services.Users;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using HKTHMall.Domain.AdminModel.Models.User;

namespace HKTHMall.Services.Tests.Users_Tests
{
    public class ZJ_RechargeOrderService_Test
    {
        private IZJ_RechargeOrderService _zjRechargeOrderService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._zjRechargeOrderService = BrEngineContext.Current.Resolve<IZJ_RechargeOrderService>();

        }

        [Test]
        public void ZJ_RechargeOrderService_AddZJ_RechargeOrder()
        {

            var model = new ZJ_RechargeOrderModel
            {
                UserID = 1,
                OrderNO = "asdfasdf22342344",
                RechargeChannel = 50,
                RechargeAmount = 1,
                RechargeDT = DateTime.Now,
                RechargeResult=0,
                IsDisplay=0,
                OrderSource=0,
                CreateDT = DateTime.Now
            };



            var result = this._zjRechargeOrderService.AddZJ_RechargeOrder(model);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void ZJ_RechargeOrderService_GetZJ_RechargeOrderList()
        {
            var result = this._zjRechargeOrderService.GetZJ_RechargeOrderList(new SearchZJ_RechargeOrderModel()
            {
                PagedSize = 15,
                PagedIndex = 0
            });

            var result1 = this._zjRechargeOrderService.GetZJ_RechargeOrderList(new SearchZJ_RechargeOrderModel()
            {
                PagedSize = 15,
                PagedIndex = 0,
                Account = "spark126"

            });



            var result3 = this._zjRechargeOrderService.GetZJ_RechargeOrderList(new SearchZJ_RechargeOrderModel()
            {
                PagedSize = 15,
                PagedIndex = 0,
                RealName = "测试1"
            });

            Assert.IsTrue(result.Data.Count != 0);
        }
    }
}
