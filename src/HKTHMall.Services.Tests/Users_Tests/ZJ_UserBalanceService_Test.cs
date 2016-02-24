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
    public class ZJ_UserBalanceService_Test
    {

        private IZJ_UserBalanceService _zjUserBalanceService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._zjUserBalanceService = BrEngineContext.Current.Resolve<IZJ_UserBalanceService>();
            
        }

        [Test]
        public void ZJ_UserBalanceService_AddZJ_UserBalance()
        {
            
                var model = new ZJ_UserBalanceModel
                {
                    UserID = 192228,
                    ConsumeBalance = 100,
                    Vouchers = 50,
                    AccountStatus = 1,
                    CreateBy = "admin",
                    CreateDT = DateTime.Now
                };
            
            

            var result = this._zjUserBalanceService.AddZJ_UserBalance(model);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void ZJ_UserBalanceService_UpdateZJ_UserBalance()
        {

            var model = new ZJ_UserBalanceModel
            {
                UserID = 116070,
                ConsumeBalance = 100,
                Vouchers = 50,
                AccountStatus = 2,
                CreateBy = "admin",
                CreateDT = DateTime.Now
            };



            var result = this._zjUserBalanceService.UpdateZJ_UserBalance(model);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void ZJ_UserBalanceService_GetZJ_UserBalanceList()
        {
            var result = this._zjUserBalanceService.GetZJ_UserBalanceList(new SearchZJ_UserBalanceModel()
            {
                PagedSize = 15,
                PagedIndex = 0
            });

            var result1 = this._zjUserBalanceService.GetZJ_UserBalanceList(new SearchZJ_UserBalanceModel()
            {
                PagedSize = 15,
                PagedIndex = 0,
                Account = "spark126"

            });

            var result2 = this._zjUserBalanceService.GetZJ_UserBalanceList(new SearchZJ_UserBalanceModel()
            {
                PagedSize = 15,
                PagedIndex = 0,
                Phone="13480801501"
            });

            var result3 = this._zjUserBalanceService.GetZJ_UserBalanceList(new SearchZJ_UserBalanceModel()
            {
                PagedSize = 15,
                PagedIndex = 0,
                RealName = "测试1"
            });

            Assert.IsTrue(result.Data.Count != 0);
        }

    }
}
