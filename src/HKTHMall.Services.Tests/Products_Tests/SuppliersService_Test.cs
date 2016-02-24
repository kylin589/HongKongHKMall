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
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Services.Products;
using NUnit.Framework;
using BrCms.Framework.Data;

namespace HKTHMall.Services.Tests.Products_Tests
{
    [TestFixture]
    public class SuppliersService_Test
    {
        private IDatabaseHelper _database;
        private ISuppliersService _suppliersService;

        [TestFixtureSetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
            _suppliersService = BrEngineContext.Current.Resolve<ISuppliersService>();
        }

        [Test]
        public void SuppliersService_UpdatePwd_Test()
        {
            var resultModel = this._suppliersService.UpdatePwd(new SuppliersModel() { SupplierId = 5728471708, PassWord = "2B07DD7FF1618ED4C7601E6FCAA38E0A" });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void BrndService_Select_Test()
        {
            SuppliersModel resultModel = this._suppliersService.GetSuppliersByPhone("0900000001").Data;

            Assert.IsTrue(resultModel != null);
        }

    }
}
