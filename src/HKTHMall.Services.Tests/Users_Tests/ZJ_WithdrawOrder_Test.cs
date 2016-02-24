using BrCms.Framework.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using HKTHMall.Services.Users;
using NUnit.Framework;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Core;

namespace HKTHMall.Services.Tests.Users_Tests
{
    [TestFixture]
    public class ZJ_WithdrawOrder_Test
    {
        private IZJ_WithdrawOrderService _zJ_WithdrawOrderService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._zJ_WithdrawOrderService = BrEngineContext.Current.Resolve<IZJ_WithdrawOrderService>();
        }

        [Test]
        public void ZJ_WithdrawOrder_Add_Test()
        {
            var result = _zJ_WithdrawOrderService.AddZJ_WithdrawOrder(192231, 100, Domain.Enum.IOrderSource.WebSite);
            Assert.IsTrue(result.IsValid);
        }
    }
}
