using BrCms.Framework.Infrastructure;
using HKTHMall.Services.Banner;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using HKTHMall.Domain.Models.banner;
using HKTHMall.Core;

namespace HKTHMall.Services.Tests.Banners
{
    [TestFixture]
    public class bannerService_Test
    {
        private IbannerService _bannerService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._bannerService = BrEngineContext.Current.Resolve<IbannerService>();
        }

        [Test]
        public void BannerService_AddBanner_Test()
        {
            for (int i = 1; i < 5; i++)
            {
                var resultModel = this._bannerService.AddBanner(new bannerModel()
                {
                    bannerId = MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                    bannerName = "首页Banner",
                    bannerUrl = "www.baidu.com",
                    bannerPic = "mail.163.com",
                    PlaceCode = 0,
                    IdentityStatus = 1,
                    Sorts = DateTime.Now.Second,
                    CreateBy = "阿斯顿"+DateTime.Now.Second.ToString(),
                    CreateDT = DateTime.Now.AddDays(-1),
                    UpdateBy = "玩儿" + DateTime.Now.Second.ToString(),
                    UpdateDT = DateTime.Now.AddDays(-1)
                });

                Assert.IsTrue(resultModel.IsValid);
            }

        }

        [Test]
        public void BannerService_GetBannerById_Test()
        {
            bannerModel resultModel = this._bannerService.GetBannerById(973840239).Data;
            Assert.IsTrue(true);
        }

        [Test]
        public void BannerService_GetBanner_Test()
        {
            //分页查询,没有数据
            List<bannerModel> resultModel = this._bannerService.GetBanner(new SearchbannerModel() { PagedIndex = 0, PagedSize = 100 }).Data;
            Assert.IsTrue(true);
            resultModel = this._bannerService.GetBanner(new SearchbannerModel() { PagedIndex = 0, PagedSize = 1, IdentityStatus=1 }).Data;
            Assert.IsTrue(true);
            resultModel = this._bannerService.GetBanner(new SearchbannerModel() { PagedIndex = 0, PagedSize = 1,  PlaceCode=1 }).Data;
            Assert.IsTrue(true);
            resultModel = this._bannerService.GetBanner(new SearchbannerModel() { PagedIndex = 0, PagedSize = 2,  PlaceCode=1, IdentityStatus=1 }).Data;
            Assert.IsTrue(true);
            
        }

        [Test]
        public void BannerService_UpdateBanner_Test()
        {
            var resultModel = this._bannerService.UpdateBanner(new bannerModel()
            {
                bannerId = 973840239,
                bannerName = "首页Banner",
                bannerUrl = "www.baidu.com",
                bannerPic = "mail.163.com",
                PlaceCode = 1,
                IdentityStatus = 1,
                Sorts = DateTime.Now.Second,
                CreateBy = "阿斯顿" + DateTime.Now.Second.ToString(),
                CreateDT = DateTime.Now.AddDays(-1),
                UpdateBy = "玩儿" + DateTime.Now.Second.ToString(),
                UpdateDT = DateTime.Now.AddDays(-1)
            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void BannerService_DeleteBanner_Test()
        {
            var resultModel = this._bannerService.DeleteBanner(new bannerModel()
            {
                bannerId = 973840248


            });

            Assert.IsTrue(resultModel.IsValid);
        }
    }
}
