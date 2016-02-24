using System;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Core;
using HKTHMall.Core.Data;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Services.Users;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.Users_Tests
{
     [TestFixture]
    public class YH_UserBankAccountTest
    {
        private IBcDbContext _dbContext;
        private IYH_UserBankAccountService _yH_UserBankAccountService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._dbContext = BrEngineContext.Current.Resolve<IBcDbContext>();
            this._yH_UserBankAccountService = BrEngineContext.Current.Resolve<IYH_UserBankAccountService>();
        }

        [Test]
        public void YH_UserBankAccount_Select_Test()
        {
            var result = _yH_UserBankAccountService.Select(new SearchYH_UserBankAccountModel() { 
               PagedIndex = 0,
               PagedSize = 1
            });
            Assert.IsTrue(result.IsValid);
        }

    }
}
