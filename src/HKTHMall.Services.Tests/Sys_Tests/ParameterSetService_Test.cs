using System;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Core;
using HKTHMall.Core.Data;
using HKTHMall.Domain.Models.Sys;
using HKTHMall.Services.Sys;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.Sys_Tests
{
    [TestFixture]
    public class ParameterSetService_Test
    {
        private IBcDbContext _dbContext;
        private IParameterSetService _parameterSetService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._dbContext = BrEngineContext.Current.Resolve<IBcDbContext>();
            this._parameterSetService = BrEngineContext.Current.Resolve<IParameterSetService>();
        }

        [Test]
        public void ParameterSetService_Add_Test()
        {
            var model = new ParameterSetModel
            {
                ParamenterID = MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                CreateBy = "张三aaaaabbbbb",
                CreateDT = DateTime.Now,
                keys = "一级aaaa",
                PValue = "10aaaa",
                Remark = "aaa",
                UpdateBy = "张三aaaa",
                UpdateDT = DateTime.Now

            };

            var result = this._parameterSetService.Add(model);
            Assert.IsTrue(result.IsValid);
        }
        [Test]
        public void ParameterSetService_GetParameterSetById_Test()
        {
           
            var result = this._parameterSetService.GetParameterSetById(4627258717);
            Assert.IsTrue(result.Data != null);
        }

        [Test]
        public void ParameterSetService_Select_Test()
        {
            var result = this._parameterSetService.Select(new SearchParaSetModel() { PagedIndex = 0, PagedSize = 15, KeysName = "" });
            Assert.IsTrue(result.Data != null);
        }

        [Test]
        public void ParameterSetService_Select2_Test()
        {
            var result = this._parameterSetService.Select(new SearchParaSetModel() { PagedIndex = 0, PagedSize = 15, KeysName = "李四" });
            Assert.IsTrue(result.Data != null);
        }

        [Test]
        public void ParameterSetService_GetParameterSetList_Test()
        {
            var result = this._parameterSetService.GetParameterSetList();
            Assert.IsTrue(result.Data != null);
        }

        [Test]
        public void ParameterSetService_Update_Test()
        {
            var result = this._parameterSetService.Update(new ParameterSetModel()
            {
                ParamenterID = 4627258717,
                keys = "二级11111",
                PValue = "100"
            });
            Assert.IsTrue(result.IsValid);
        }
        [Test]
        public void ParameterSetService_Delete_Test()
        {
            var result = this._parameterSetService.Delete(4627258711);
            Assert.IsTrue(result.Data);
        }

        [Test]
        public void ParameterSetService_GetParameterSetList1_Test()
        {
            var result = this._parameterSetService.GetParameterSetListByName(new ParameterSetModel() { });
            Assert.IsTrue(result.Data != null);
        }

    }
}
