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
using HKTHMall.Domain.Models.Bra;
using HKTHMall.Services.Products;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.Products_Tests
{
    [TestFixture]
    public class BrandServiceTest
    {
        private IBrandService _baseService;
        private IBcDbContext _dbContext;

        [TestFixtureSetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._dbContext = BrEngineContext.Current.Resolve<IBcDbContext>();
            this._baseService = BrEngineContext.Current.Resolve<IBrandService>();
        }

        [Test]
        public void BrndService_Add_Test()
        {
            var resultModel = this._baseService.Add(new BrandModel
            {
                BrandID = (int)MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                BrandState = 0,
                BrandUrl = "www.baidu.com",
                FirstPY = "1",
                Remark = "非常棒的商品品牌",
                UpdateBy = "admin",
                UpdateDT = DateTime.Now,
                AddTime = DateTime.Now,
                AddUsers = "admin",
                Brand_Lang = new List<Brand_Lang>() { 
                    new Brand_Lang(){ LanguageID = 1, BrandName="aaa" },
                      new Brand_Lang(){ LanguageID = 2, BrandName="ccc" },
                        new Brand_Lang(){ LanguageID = 3, BrandName="sssss" }
                }
            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void BrndService_Select_Test()
        {
            var resultModel = this._baseService.Select(new SearchBrandModel()
            {
                PagedIndex = 0,
                PagedSize = 15,
            });

            //Assert.IsTrue(resultModel.Data != null);
        }
        [Test]
        public void BrndService_GetBrandById_Test()
        {
            var resultModel = this._baseService.GetBrandById(973840655);

            //Assert.IsTrue(resultModel.Data != null);
        }
        [Test]
        public void BrndService_Update_Test()
        {
            var resultModel = this._baseService.Update(new BrandModel()
            {
                BrandID = 973840060,
                BrandUrl = "wwwwwwww",
                 UpdateDT = DateTime.Now,
                  AddTime = DateTime.Now,
                   AddUsers ="admin",
                    BrandState = 0,
                Brand_Lang = new List<Brand_Lang>() { new Brand_Lang() { BrandID = 973840060 , LanguageID = 1, BrandName="飞利浦笔记本"} }
            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void BrndService_Delete_Test()
        {
            var resultModel = this._baseService.Delete(996297121);
        }
    }
}
