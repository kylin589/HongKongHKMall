using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using HKTHMall.Services.Banner;
using BrCms.Framework.Infrastructure;
using HKTHMall.Domain.Models.banner;
using HKTHMall.Core;

namespace HKTHMall.Services.Tests.Banners
{
    [TestFixture]
    public class bannerProductService_Test
    {
        private IbannerProductService _bannerProductService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._bannerProductService = BrEngineContext.Current.Resolve<IbannerProductService>();
        }
        [Test]
        public void BannerProductService_AddBannerProduct_Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var resultModel = this._bannerProductService.AddBannerProduct(new bannerProductModel()
                {
                    bannerProductId = MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                    productId = DateTime.Now.Second,
                     
                    PlaceCode = 0,
                    IdentityStatus = 1,
                    Sorts = MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                    CreateBy = "阿斯顿" + DateTime.Now.Second.ToString(),
                    CreateDT = DateTime.Now.AddDays(-1),
                    UpdateBy = "玩儿" + DateTime.Now.Second.ToString(),
                    UpdateDT = DateTime.Now.AddDays(-1),
                    PicAddress="www.360.com"
                });

                Assert.IsTrue(resultModel.IsValid);
            }

        }

        [Test]
        public void BannerProductService_GetBannerProductById_Test()
        {
            bannerProductModel resultModel = this._bannerProductService.GetBannerProductById(973840249).Data;
            Assert.IsTrue(true);
        }

        [Test]
        public void BannerProductService_GetBannerProduct_Test()
        {
            int tol = 1;
            var r = this._bannerProductService.GetBannerProduct(new SearchbannerProductModel() { PagedIndex = 0, PagedSize = 100 },out tol);
            //分页查询,没有数据
            List<bannerProductModel> resultModel = this._bannerProductService.GetBannerProduct(new SearchbannerProductModel() { PagedIndex = 0, PagedSize = 100 }).Data;
            Assert.IsTrue(true);
            resultModel = this._bannerProductService.GetBannerProduct(new SearchbannerProductModel() { PagedIndex = 0, PagedSize = 1,  IdentityStatus=1 }).Data;
            Assert.IsTrue(true);
            resultModel = this._bannerProductService.GetBannerProduct(new SearchbannerProductModel() { PagedIndex = 0, PagedSize = 1, PlaceCode=1}).Data;
            Assert.IsTrue(true);
            resultModel = this._bannerProductService.GetBannerProduct(new SearchbannerProductModel() { PagedIndex = 0, PagedSize = 2,  PlaceCode=1 , IdentityStatus=1 }).Data;
            Assert.IsTrue(true);
            
        }

        [Test]
        public void BannerProductService_UpdateBannerProduct_Test()
        {
            var resultModel = this._bannerProductService.UpdateBannerProduct(new bannerProductModel()
            {
                bannerProductId = 973840249,
                productId = DateTime.Now.Second,

                PlaceCode = 1,
                IdentityStatus = 1,
                Sorts = DateTime.Now.Second,
                CreateBy = "阿斯顿" + DateTime.Now.Second.ToString(),
                CreateDT = DateTime.Now.AddDays(-1),
                UpdateBy = "玩儿" + DateTime.Now.Second.ToString(),
                UpdateDT = DateTime.Now.AddDays(-1),
                PicAddress = "www.360.com"
            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void BannerProductService_DeleteBannerProduct_Test()
        {
            var resultModel = this._bannerProductService.DeleteBannerProduct(new bannerProductModel()
            {
                bannerProductId = 973840258


            });

            Assert.IsTrue(resultModel.IsValid);
        }

    }
}
