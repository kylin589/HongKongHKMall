using System;
using System.Collections.Generic;
using Autofac;
using BrCms.Framework.Data;
using BrCms.Framework.Infrastructure;
using HKTHMall.Core;
using HKTHMall.Core.Data;
using HKTHMall.Domain.AdminModel.Models.Orders;
using HKTHMall.Domain.Entities;
using HKTHMall.Services.Orders;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.Products_Tests
{
    [TestFixture]
    public class PaymentOrderTest
    {
        private IDatabaseHelper _database;
        private IPaymentOrderService _paymentOrderService;

        [TestFixtureSetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
            _paymentOrderService = BrEngineContext.Current.Resolve<IPaymentOrderService>();
        }

        [Test]
        public void PaymentOrder_Select_Test()
        {
            var resultModel = this._paymentOrderService.Select(new SearchPaymentOrderModel()
            {
                PagedIndex = 0,
                PagedSize = 15
            });
        }
    }
}
