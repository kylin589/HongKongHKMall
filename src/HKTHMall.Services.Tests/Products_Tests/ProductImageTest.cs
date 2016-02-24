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
    public class ProductImageTest
    {
        private IDatabaseHelper _database;
        private IProductImageService _productImageService;

        [TestFixtureSetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
            _productImageService = BrEngineContext.Current.Resolve<IProductImageService>();
        }
        [Test]
        public void ProductImage_Add_Test()
        {
            var resultModel = this._productImageService.Add(new ProductImageModel()
            {
                ProductImageId = MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                ProductName = "小人妖",
                ImageUrl = "https://ss1.baidu.com/6ONXsjip0QIZ8tyhnq/it/u=763725892,311696505&fm=58",
                CreateBy = "admin",
                CreateDT = DateTime.Now,
                UpdateDT = DateTime.Now,
                UpdateBy = "admin"
            });

            Assert.IsTrue(resultModel.IsValid);
        }

    }
}
