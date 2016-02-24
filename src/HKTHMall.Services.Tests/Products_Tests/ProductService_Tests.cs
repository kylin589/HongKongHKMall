using System;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Services.Products;
using NUnit.Framework;
using BrCms.Framework.Data;
using System.Collections.Generic;
using System.Linq;

namespace HKTHMall.Services.Tests.Products_Tests
{
    [TestFixture]
    public class ProductService_Tests
    {
        private IProductService _productService;
        private dynamic _databaseDb;

        [TestFixtureSetUp]
        public void SetUp()
        {
            BrEngineContext.Init(null);
            this._productService = BrEngineContext.Current.Resolve<IProductService>();
            this._databaseDb = BrEngineContext.Current.Resolve<IDatabaseHelper>().Db;
        }

        [Test]
        public void ProductService_Add_Test()
        {
            var result = this._productService.Add(new AddProductModel
            {
                ArtNo = "sdfasd",
                ActivityBottomPrice = 0,
                AllowBackInStockSubscriptions = true,
                AllowCustomerReviews = true,
                BrandID = 1,
                CreateDT = DateTime.Now,
                CreateBy = "sdfa",
                CategoryId = 1,
                ExtensionPropertiesText = "sdfa",
                FareTemplateID = 45
            });

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void ProductService_Search_Test()
        {
            var model = new SearchProductModel
            {
                PagedIndex = 0,
                PagedSize = 10,
                //LanguageId = 1,
                ProductId = 1
            };

            var result = this._productService.Search(model);

            Assert.IsTrue(result.IsValid);
            if (model.ProductId.HasValue)
            {
                Assert.IsTrue(result.Data != null);
            }
            
        }

        [Test]
        public void ProductService_Update_Test()
        {
            var result = this._productService.Update(new UpdateProductModel
            {
                ArtNo = "sdfasd",
                ActivityBottomPrice = 0,
                AllowBackInStockSubscriptions = true,
                AllowCustomerReviews = true,
                BrandID = 1,
                //CreateDT = DateTime.Now,
                //CreateBy = "sdfa",
                CategoryId = 1,
                ExtensionPropertiesText = "sdfa",
                FareTemplateID = 45
            });
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void ProductService_Delete_Test()
        {
            IList<long> ids = this._databaseDb.Product.Query().Select(this._databaseDb.Product.ProductId).ToScalarList<long>();
            var result = this._productService.DeleteList(ids);
            Assert.IsTrue(result.IsValid);
        }
    }
}