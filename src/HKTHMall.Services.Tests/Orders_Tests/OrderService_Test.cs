using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BrCms.Framework.Data;
using BrCms.Framework.Infrastructure;
using HKTHMall.Core.Security;
using HKTHMall.Domain.Enum;
using HKTHMall.Domain.WebModel.Models.Orders;
using HKTHMall.Services.Orders;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.Orders_Tests
{
    [TestFixture]
    public class OrderService_Test
    {
        private IOrderService _OrderService;

        private IEncryptionService _enctyptionService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._OrderService = BrEngineContext.Current.Resolve<IOrderService>();
            this._enctyptionService = BrEngineContext.Current.Resolve<IEncryptionService>();
        }

        [Test]
        public void GetOrderDetailIntoWebBy_Test()
        {


            var result = _OrderService.GetOrderDetailIntoWebBy(new HKTHMall.Domain.WebModel.Models.Orders.SearchOrderDetailView() { OrderID = "100", UserID = 116070, LanguageID = 1 });
            Assert.IsTrue(result.IsValid);
            Assert.NotNull(result.Data);
        }

        [Test]
        public void CancelOrderBy_Test()
        {

            HKTHMall.Domain.WebModel.Models.Orders.SearchOrderDetailView searchView = new Domain.WebModel.Models.Orders.SearchOrderDetailView()
            {
                OrderID = "100",
                UserID = 116070,
                OrderStatus = (int)OrderEnums.OrderStatus.Obligation,
                LanguageID = 1

            };

            var result = _OrderService.CancelOrderBy(searchView);
            Assert.IsTrue(result.IsValid);

        }

        [Test]
        public void GennerateOrder()
        {
            AddOrderInfoView view = new AddOrderInfoView()
            {
                LanguageId = 1,
                MerchantViews = new List<AddOrderInfoView.MerchantView>()
                {
                 new AddOrderInfoView.MerchantView()
                 {
                     MerchantID="1",
                     Remark="我来买东西咯",
                 }
                },
                ReceiverAddressId = 12,
                UserId = 1,
                PayType = (int)OrderEnums.PayType.ThirdPay,
                PayChannel = (int)OrderEnums.PayChannel.PayPal,
                PaidType = (int)OrderEnums.PaidType.Mall,
                OrderSource = (int)OrderEnums.OrderSource.Web
            };

            var result = _OrderService.GenerateNormalOrder(view);
            Assert.IsTrue(result.IsValid);


        }


        [Test]
        public void GennerateOrder2()
        {
            List<int> list = new List<int>() { 1, 2, 3, 4, 5 };
            list.Insert(0, 0);
            Assert.IsTrue(list.Count == 6);
            int a = 1;
        }



        [Test]
        public void GennerateOrderForPayment()
        {
            AddOrderInfoView view = new AddOrderInfoView()
            {
                LanguageId = 4,
                MerchantViews = new List<AddOrderInfoView.MerchantView>()
                {
                 new AddOrderInfoView.MerchantView()
                 {
                     MerchantID="1",
                     Remark="我来买东西咯",
                 }
                },
                ReceiverAddressId = 5728462508,
                UserId = 6600000012,
                PayType = (int)OrderEnums.PayType.ThirdPay,
                PayChannel = (int)OrderEnums.PayChannel.PayPal,
                PaidType = (int)OrderEnums.PaidType.Mall,
                OrderSource = (int)OrderEnums.OrderSource.Web
            };

            var result = _OrderService.GenerateNormalOrder(view);
            Assert.IsTrue(result.IsValid);


        }

   
    }
}
