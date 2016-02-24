
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BrCms.Framework.Infrastructure;
using HKTHMall.Services.Users;
using NUnit.Framework;
using Autofac;
using HKTHMall.Domain.AdminModel.Models.User;

namespace HKTHMall.Services.Tests.Users_Tests
{
    [TestFixture]
    public class ZJ_UserBalanceChangeLogService_test
    {
        private IZJ_UserBalanceChangeLogService _zjUserBalanceChangeLogService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._zjUserBalanceChangeLogService = BrEngineContext.Current.Resolve<IZJ_UserBalanceChangeLogService>();

        }

        [Test]
        public void ZJ_UserBalanceChangeLog_AddZJ_UserBalance()
        {

            var model = new ZJ_UserBalanceChangeLogModel
            {
                UserID = 192228,
                AddOrCutAmount = 100,
                IsAddOrCut = 50,
                OldAmount = 1,
                NewAmount = 192228,
                AddOrCutType = 100,
                OrderNo = "23423432432423",
                Remark = "asdf",
                IsDisplay = 0,
                CreateBy = "admin",
                CreateDT = DateTime.Now
            };



            var result = this._zjUserBalanceChangeLogService.AddZJ_UserBalanceChangeLog(model);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void ZJ_UserBalanceService_UpdateZJ_UserBalance()
        {

            //var model = new ZJ_UserBalanceModel
            //{
            //    UserID = 116070,
            //    ConsumeBalance = 100,
            //    Vouchers = 50,
            //    AccountStatus = 2,
            //    CreateBy = "admin",
            //    CreateDT = DateTime.Now
            //};



            //var result = this._zjUserBalanceChangeLogService.UpdateZJ_UserBalanceChangeLog(model);
            //Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void ZJ_UserBalanceChangeLog_GetZJ_UserBalanceList()
        {
            var result = this._zjUserBalanceChangeLogService.GetZJ_UserBalanceChangeLogList(new SearchZJ_UserBalanceChangeLogModel()
            {
                PagedSize = 15,
                PagedIndex = 0
            });

            var result1 = this._zjUserBalanceChangeLogService.GetZJ_UserBalanceChangeLogList(new SearchZJ_UserBalanceChangeLogModel()
            {
                PagedSize = 15,
                PagedIndex = 0,
                Account = "spark126"

            });

            var result2 = this._zjUserBalanceChangeLogService.GetZJ_UserBalanceChangeLogList(new SearchZJ_UserBalanceChangeLogModel()
            {
                PagedSize = 15,
                PagedIndex = 0,
                Phone = "13480801501"
            });

            var result3 = this._zjUserBalanceChangeLogService.GetZJ_UserBalanceChangeLogList(new SearchZJ_UserBalanceChangeLogModel()
            {
                PagedSize = 15,
                PagedIndex = 0,
                RealName = "测试1"
            });

            Assert.IsTrue(result.Data.Count != 0);
        }
    }
}
