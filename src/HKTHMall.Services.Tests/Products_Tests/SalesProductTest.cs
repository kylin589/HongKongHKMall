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
    public class SalesProductTest
    {
        private IDatabaseHelper _database;
        private ISalesProductService _salesProductService;

        [TestFixtureSetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
            _salesProductService = BrEngineContext.Current.Resolve<ISalesProductService>();
        }

        [Test]
        public void BrndService_Select_Test()
        {
            var resultModel = this._salesProductService.Select(new SearchSalesProductModel() { 
             PagedIndex =0,
              PagedSize = 15,
              LanguageID = 1
            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void BrndService_GetSalesProductById_Test()
        {
            var resultModel = this._salesProductService.GetSalesProductById(1215880325);

            Assert.IsTrue(resultModel.IsValid);
        }
    }
}
