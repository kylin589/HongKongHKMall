using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Domain.Models;
using HKTHMall.Services.SKU;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.SKU_Tests
{
    [TestFixture]
    public class SKU_AttributesService_Test
    {
        private ISKU_AttributesService _SKU_AttributesService;
        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._SKU_AttributesService = BrEngineContext.Current.Resolve<ISKU_AttributesService>();
        }

        [Test]
        public void AddSKU_Attributes_Test()
        {

            SKU_AttributesModel model = new SKU_AttributesModel()
            {
                AttributeName = "网络格式",
                AttributeType = 0,
                CreateBy = "admin",
                CreateDT = DateTime.Now,
                IsSearch = 0,
                IsSKU = 1,
                Remark = "手机",
                RowStatus = 0,
                UsageMode = 0,
                UpdateBy = "admin",
                UpdateDT = DateTime.Now,
                SKU_AttributeValuesModels = new List<SKU_AttributeValuesModel>()
                {
                    new SKU_AttributeValuesModel()
                    {
                        DisplaySequence=1,
                        ImageUrl="",
                        ValuesGroup="",
                        ValueStr="移动"

                    },
                    new SKU_AttributeValuesModel()
                    {
                        DisplaySequence=1,
                        ImageUrl="",
                        ValuesGroup="",
                        ValueStr="联通"

                    }
                }
            };
            ResultModel result = _SKU_AttributesService.AddStandardSKU_Attributes(model);
            Assert.IsTrue(result.IsValid);


        }

        [Test]
        public void UpdateSKU_Attributes_Test()
        {

            SKU_AttributesModel model = new SKU_AttributesModel()
            {
                AttributeId = 4,
                AttributeName = "网络格式111",
                AttributeType = 0,
                CreateBy = "admin",
                CreateDT = DateTime.Now,
                IsSearch = 0,
                IsSKU = 1,
                Remark = "手机",
                RowStatus = 0,
                UsageMode = 0,
                UpdateBy = "admin",
                UpdateDT = DateTime.Now,
                SKU_AttributeValuesModels = new List<SKU_AttributeValuesModel>()
                {
                    new SKU_AttributeValuesModel()
                    {
                        AttributeId=4,
                        ValueId=1,
                        DisplaySequence=1,
                        ImageUrl="",
                        ValuesGroup="",
                        RowStatus = 0,
                        ValueStr="移动1"

                    },
                    new SKU_AttributeValuesModel()
                    {
                        AttributeId=4,
                        ValueId=2,
                        DisplaySequence=1,
                        ImageUrl="",
                        ValuesGroup="",
                        RowStatus = -1,
                        ValueStr="联通2"

                    }
                }
            };
            ResultModel result = _SKU_AttributesService.UpdateStandardSKU_Attributes(model);
            Assert.IsTrue(result.IsValid);


        }

        [Test]
        public void DeleteSKU_AttributesById_Test()
        {
            var result = _SKU_AttributesService.DeleteSKU_AttributesById(1);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void GetSKU_AttributesById_Test()
        {
            var model = _SKU_AttributesService.GetSKU_AttributesById(4);
            Assert.NotNull(model.Data);
            Assert.IsTrue(model.Data.AttributeId > 0);
        }

        [Test]
        public void GetPagingSKU_SKU_AttributesService_Test()
        {
            SearchSKU_AttributesModel searchModel = new SearchSKU_AttributesModel()
            {
                PagedIndex = 1,
                PagedSize = 10
            };
            var result = this._SKU_AttributesService.GetPagingSKU_Attributess(searchModel);
            Assert.IsTrue(result.Data.TotalCount > 0);
        }

        [Test]
        public void GetAllSKU_AttributesBy_Test()
        {
            var result = this._SKU_AttributesService.GetAllSKU_AttributesBy(true);
            Assert.IsTrue(result.IsValid);
            Assert.NotNull(result.Data);
            Assert.IsTrue(result.Data.Count > 0);

        }

        [Test]
        public void GetAttributeValuesById_Test()
        {
            long id = 17;
            var result = this._SKU_AttributesService.GetAttributeValuesById(id);
            Assert.IsTrue(result.IsValid);
            Assert.NotNull(result.Data);
            Assert.IsTrue(result.Data.Count > 0);
        }
    }
}
