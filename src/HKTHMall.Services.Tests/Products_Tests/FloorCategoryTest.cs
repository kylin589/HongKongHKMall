using System;
using System.Collections.Generic;
using Autofac;
using BrCms.Framework.Data;
using BrCms.Framework.Infrastructure;
using HKTHMall.Core;
using HKTHMall.Core.Data;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Domain.Entities;
using HKTHMall.Services.Products;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.FloorCategoryTest
{
    [TestFixture]
    public class FloorCategoryTest
    {
        private IDatabaseHelper _database;
        private IFloorCategoryService _floorCategoryService;

        [TestFixtureSetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
            _floorCategoryService = BrEngineContext.Current.Resolve<IFloorCategoryService>();
        }

        [Test]
        public void FloorCategory_Add_Test()
        {
            var resultModel = this._floorCategoryService.Add(new FloorCategoryModel()
            {

                Place = MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                DCategoryId = 973840518,
                CategoryId = 973840522,
                FloorCategoryId = MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                AddUsers = "admin",
                AddTime = DateTime.Now
            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void FloorCategory_Select_Test()
        {
            var resultModel = this._floorCategoryService.Select(new SearchFloorCategoryModel()
            {
                PagedIndex = 0,
                PagedSize = 15,
                 LanguageID = 1,
                ParentID = 973840518
            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void FloorCategory_GetFloorCategoryList_Test()
        {
            var resultModel = this._floorCategoryService.GetFloorCategoryList(new SearchFloorCategoryModel()
            {
                PagedIndex = 0,
                PagedSize = 15,
                LanguageID = 1
            });

            Assert.IsTrue(resultModel.IsValid);
        }
    }
}