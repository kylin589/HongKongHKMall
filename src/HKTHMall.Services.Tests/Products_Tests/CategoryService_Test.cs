using System;
using System.Collections.Generic;
using Autofac;
using BrCms.Framework.Data;
using BrCms.Framework.Infrastructure;
using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.Categoreis;
using HKTHMall.Services.Products;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.Products_Tests
{
    [TestFixture]
    public class CategoryServiceTest
    {
        private IDatabaseHelper _database;
        private ICategoryService _categoryService;

        [TestFixtureSetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
            this._categoryService = BrEngineContext.Current.Resolve<ICategoryService>();
        }

        [Test]
        public void CategoryService_AddCategory_Test()
        {
            var resultModel = this._categoryService.AddCategory(new AddCategoryModel
            {
                AddDate = DateTime.Now,
                parentId = 0,
                AuditState = true,
                AddUser = "SimpleData",
                Grade = 1,
                Place = 0,
                Category_Lang = new List<CategoryLanguageModel>()
                {
                    new CategoryLanguageModel()
                    {
                        LanguageID = 1,
                        CategoryName = "中文1"
                    },
                    new CategoryLanguageModel()
                    {
                        LanguageID = 2,
                        CategoryName = "英文"
                    },
                    new CategoryLanguageModel()
                    {
                        LanguageID = 3,
                        CategoryName = "泰文"
                    },
                }
            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void CategoryService_GetCategoriesByCategoryToTree_Test()
        {
            var result = this._categoryService.GetCategoriesByCategoryToTree(1);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void CategoryService_GetCategoryById()
        {
            var model = new AddCategoryModel
            {
                AddDate = DateTime.Now,
                parentId = 0,
                AuditState = true,
                AddUser = "SimpleData",
                Grade = 1,
                Place = 0,
                Category_Lang = new List<CategoryLanguageModel>()
                {
                    new CategoryLanguageModel()
                    {
                        LanguageID = 1,
                        CategoryName = "中文1"
                    },
                    new CategoryLanguageModel()
                    {
                        LanguageID = 2,
                        CategoryName = "英文"
                    },
                    new CategoryLanguageModel()
                    {
                        LanguageID = 3,
                        CategoryName = "泰文"
                    },
                }
            };

            var result = this._categoryService.AddCategory(model);

            var result1 = this._categoryService.GetCategoryById(result.Data);

            Assert.IsTrue(result1.IsValid);
        }

        [Test]
        public void CategoryService_UpdateCategory_Test()
        {
            var resultModel = this._categoryService.AddCategory(new AddCategoryModel
            {
                AddDate = DateTime.Now,
                parentId = 0,
                AuditState = true,
                AddUser = "test",
                Grade = 1,
                CategoryId = (int) MemCacheFactory.GetCurrentMemCache().Increment("commonId")
            });

            var result = this._categoryService.UpdateCategory(new UpdateCategoryModel()
            {
                UpdateBy = "test1",
                CategoryId = resultModel.Data,
                parentId = 1,
                AuditState = true,
                Grade = 1,
                UpdateDT = DateTime.Now
            });

            Assert.IsTrue(result.IsValid);

            var category = this._categoryService.GetCategoryById(resultModel.Data);

            Assert.AreEqual(category.Data.AddUser, "test1");
            Assert.AreEqual(category.Data.parentId, 1);
            Assert.AreEqual(category.Data.AuditState, 1);
            Assert.AreEqual(category.Data.Grade, 1);
        }

        [Test]
        public void CategoryService_GetCategoryListByChlidId()
        {
            var result = this._categoryService.GetParentCategoryListByChildernCategoryId(973840580, 1);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Data.Count > 0);
        }
    }
}