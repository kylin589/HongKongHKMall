using System;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Services.Products;
using NUnit.Framework;
using HKTHMall.Services.Orders;
using HKTHMall.Domain.Models.Orders;

namespace HKTHMall.Services.Tests.Products_Tests
{
    [TestFixture]
    public class ComplaintsTest
    {
        private IComplaintsService _complaintsService;

        [TestFixtureSetUp]
        public void SetUp()
        {
            BrEngineContext.Init(null);
            this._complaintsService = BrEngineContext.Current.Resolve<IComplaintsService>();
        }

        [Test]
        public void ProductService_Select_Test()
        {
            var result = this._complaintsService.Select(new SearchComplaintsModel() { 
             PagedIndex = 0,
              PagedSize = 15
            });

            Assert.IsTrue(result.IsValid);
        }
    }
}
