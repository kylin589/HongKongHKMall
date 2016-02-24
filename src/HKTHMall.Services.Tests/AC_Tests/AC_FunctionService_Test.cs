using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Services.AC;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.AC_Tests
{
    [TestFixture]
    public class AC_FunctionService_Test
    {
        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            _aC_FunctionService = BrEngineContext.Current.Resolve<IAC_FunctionService>();
        }

        private IAC_FunctionService _aC_FunctionService;

        [Test]
        public void AC_FunctionService_Add_Test()
        {
            var model = new AC_FunctionModel
            {
                Action = "aaaaa",
                Controller = "bbbbb",
                FunctionName = "cccc",
                ModuleID = 1
            };

            var result = _aC_FunctionService.Add(model);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void AC_FunctionService_Delete_Test()
        {
            var result = _aC_FunctionService.Delete(10);
            Assert.IsTrue(result.Data);
        }

        [Test]
        public void AC_FunctionService_GetParameterSetById_Test()
        {
            var result = _aC_FunctionService.GetAC_FunctionById(10);
            Assert.IsTrue(result.Data != null);
        }

        [Test]
        public void AC_FunctionService_Select2_Test()
        {
            var result = _aC_FunctionService.Select(new SearchAC_FunctionModel {PagedIndex = 0, PagedSize = 15});
            Assert.IsTrue(result.Data != null);
        }

        [Test]
        public void AC_FunctionService_Update_Test()
        {
            var result = _aC_FunctionService.Update(new AC_FunctionModel
            {
                ModuleID = 10,
                FunctionName = "eeee"
            });
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void AC_FunctionServicee_Select_Test()
        {
            var result = _aC_FunctionService.Select(new SearchAC_FunctionModel {PagedIndex = 0, PagedSize = 15});
            Assert.IsTrue(result.Data != null);
        }
    }
}