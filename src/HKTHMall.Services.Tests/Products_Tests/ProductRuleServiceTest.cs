using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Core;
using HKTHMall.Core.Data;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Services.Products;
using NUnit.Framework;
using BrCms.Framework.Data;

namespace HKTHMall.Services.Tests.Products_Tests
{
    [TestFixture]
    public class ProductRuleServiceTest
    {
        private IDatabaseHelper _database;
        private IProductRuleService _productRuleService;

        [TestFixtureSetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
            _productRuleService = BrEngineContext.Current.Resolve<IProductRuleService>();
        }

        [Test]
        public void BrndService_Add_Test()
        {
            var resultModel = this._productRuleService.Add(new ProductRuleModel
            {
                ProductRuleId = MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                BuyQty = 1,
                Discount = 96,
                EndDate = DateTime.Now,
                PrdoctRuleName = "促销信息",
                Price = 10,
                ProductId = 1,
                SendQty = 2,
                SalesRuleId = 1215865087,
                StarDate = DateTime.Now,


            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void BrndService_Select_Test()
        {
            var result = this._productRuleService.Select(new SearchProductRuleModel()
            {
                //ProductId = 0,
                //SalesRuleId = 0,
                PagedIndex = 0,
                PagedSize = 15,
                LanguageID =1
            });

            Assert.IsTrue(result.Data != null);
        }
    }
}
