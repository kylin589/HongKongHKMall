using System;
using System.Globalization;
using System.Threading;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.AC;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Services.AC;
using NUnit.Framework;
using HKTHMall.Core;
//using Resources;

namespace HKTHMall.Services.Tests.AC_Tests
{
    [TestFixture]
    public class AC_DepartmentService_Test
    {
        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            _acDepartmentService = BrEngineContext.Current.Resolve<IAC_DepartmentService>();
        }

        private IAC_DepartmentService _acDepartmentService;

        //[Test]
        //public void AC_DepartmentService_AddAC_Department_Test()
        //{

        //    //var culture = CultureHelper.GetImplementedCulture("th_TH");
        //    //DateTime now = DateTime.Now;

        //    //string aa = DateTime.Now.ToString(new CultureInfo("zh-CN"));

        //    //for (int a = 0; a < 10; a++)
        //    //{
        //    //    long id = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
        //    //}
        //    //var entity = new AC_DepartmentModel
        //    //{
        //    //    DeptName = "技术部2222222222",
        //    //    IsActive = 1,
        //    //    OrderNumber = 0,
        //    //    ParentID = 0,
        //    //    CreateBy = "admin",
        //    //    CreateDT = DateTime.Now,
        //    //    UpdateBy = "admin",
        //    //    UpdateDT = DateTime.Now
        //    //};

        //    //var resultModel = _acDepartmentService.AddAC_Department(entity);
        //    Assert.IsTrue(resultModel.Data > 0);
        //}

        [Test]
        public void AC_DepartmentService_Delete_Test()
        {
            var resultModel = _acDepartmentService.DeleteAC_DepartmentById(2);
            Assert.IsTrue(resultModel.Data == true);
        }

        [Test]
        public void AC_DepartmentService_GetAC_DepartmentById_Test()
        {
            var resultModel = _acDepartmentService.GetAC_DepartmentById(2);
            var test = resultModel.Data.ToEntity();
            Assert.IsTrue(test != null);
            Assert.IsTrue(resultModel.Data.ID == 2);
        }

        [Test]
        public void AC_DepartmentService_GetPagingAC_Departments()
        {
            var searchModel = new SearchAC_DepartmentModel
            {
                DeptName = "",
                PagedIndex = 1,
                PagedSize = 10
            };
            var result = _acDepartmentService.GetPagingAC_Departments(searchModel);
            Assert.IsTrue(result.Data.TotalCount >= 0);
        }

        [Test]
        public void AC_DepartmentService_UpdateAC_Department_Test()
        {
            var entity = new AC_DepartmentModel
            {
                ID = 2,
                DeptName = "技术部22",
                IsActive = 1,
                OrderNumber = 0,
                ParentID = 0,
                CreateBy = "admin",
                CreateDT = DateTime.Now,
                UpdateBy = "admin",
                UpdateDT = DateTime.Now
            };

            var resultModel = _acDepartmentService.UpdateAC_Department(entity);
            Assert.IsTrue(resultModel.Data == true);

            var resuleModelUpdated = _acDepartmentService.GetAC_DepartmentById(entity.ID);
            Assert.IsTrue(resuleModelUpdated.Data.DeptName == "技术部22");
        }
    }
}