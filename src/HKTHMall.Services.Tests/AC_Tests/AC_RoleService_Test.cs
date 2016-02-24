using System;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Services.AC;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.AC_Tests
{
    [TestFixture]
    public class AC_RoleService_Test
    {
        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            _RoleService = BrEngineContext.Current.Resolve<IAC_RoleService>();
            //BrMvcEngineContext.Init(null);
            //this._RoleService =
            //    BrMvcEngineContext.Current.Resolve<IAC_RoleService>();
        }

        private IAC_RoleService _RoleService;

        [Test]
        public void Add_Test()
        {
            var entity = new AC_RoleModel
            {
                RoleName = "技术部",
                RoleFuctionValue = "1,1,1",
                RoleModuleValue = "1,1,1",
                RoleDescription = "",
                CreateUser = "admin",
                CreateDT = DateTime.Now,
                UpdateUser = "admin",
                UpdateDt = DateTime.Now
            };

            var resultModel = _RoleService.Add(entity);
            Assert.IsTrue(resultModel.Data.RoleID > 0);
            // Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void GetFunctionList_Test()
        {
            var resultModel = _RoleService.GetFunctionList(6);
            Assert.IsTrue(resultModel.Data.Count > 0);
        }

        /// <summary>
        ///     根据角色ID取菜单权限
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [Test]
        public void GetModuleMenuList_Test()
        {
            var resultModel = _RoleService.GetFunctionList(6);
            Assert.IsTrue(resultModel.Data.Count > 0);
        }

        [Test]
        public void Update_Test()
        {
            var entity = new AC_RoleModel
            {
                RoleID = 1,
                RoleName = "技术部",
                RoleFuctionValue = "1,1,1,0,0",
                RoleModuleValue = "1,1,1",
                RoleDescription = "",
                CreateUser = "admin",
                CreateDT = DateTime.Now,
                UpdateUser = "admin",
                UpdateDt = DateTime.Now
            };

            var resultModel = _RoleService.Update(entity);
            Assert.IsTrue(resultModel.Data > 0);
            //Assert.IsTrue(resultModel.Data == true);

            //var resuleModelUpdated = this._acDepartmentService.GetAC_DepartmentById(entity.ID);
            //Assert.IsTrue(resuleModelUpdated.Data.DeptName == "技术部22");
        }
    }
}