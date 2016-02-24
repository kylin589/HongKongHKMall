using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using HKTHMall.Domain.Entities;
using HKTHMall.Services.AC;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;
using BrCms.Framework.Infrastructure;
using HKTHMall.Domain.Models.AC;

namespace HKTHMall.Services.Tests.AC_Tests
{
    [TestFixture]
    public class AC_ModuleService_Test
    {
        private IAC_ModuleService _acModuleService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._acModuleService = BrEngineContext.Current.Resolve<IAC_ModuleService>();
        }

        [Test]
        public void AC_ModuleService_FirstOrDefault_Test()
        {
            //var resultModel = this._acModuleService.FirstOrDefault<AC_Module>(x => x.ModuleID == 1);
            //Assert.IsTrue(resultModel.Data.ModuleID == 1);
        }

        [Test]
        public void AC_ModuleService_Add_Test()
        {
            var entity = new AC_ModuleModel()
            {
                Controller = "Home",
                Action = "Index",
                ModuleName = "首页",
                ParentID = 0,
                Place = 0,
                Icon = ""
            };

            var resultModel = this._acModuleService.Add(entity);
            Assert.IsTrue(resultModel.Data == true);
            Assert.IsTrue(entity.ModuleID > 0);
        }

        [Test]
        public void AC_ModuleService_Update_Test()
        {
            //var resultModel = this._acModuleService.Update<AC_Module>(x => x.ModuleID == 2, x => new AC_Module() { Controller = "Home1" });
            //Assert.IsTrue(resultModel.Data == true);
        }

        [Test]
        public void AC_ModuleService_Delete_Test()
        {

            //var resultModel = this._acModuleService.DeleteBy<AC_Module>(x => x.ModuleID == 1);
            //Assert.IsTrue(resultModel.Data == true);
        }

        [Test]
        public void AC_ModuleService_Select_Test()
        {

            var result = this._acModuleService.GetAC_ModuleList();
            //Assert.IsTrue(resultModel.Data == true);
        }

        [Test]
        public void AC_ModuleService_GetAC_ModuleByIDstr_Test()
        {
            //var result = this._acModuleService.GetAC_ModuleByIDstr("1,2,3,4");
            //Assert.IsTrue(result.Data.Count > 0);
        }

    }
}
