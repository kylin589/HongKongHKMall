using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Core.Data;
using HKTHMall.Domain.WebModel.Models.Login;
using HKTHMall.Services.Users;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.Users_Tests
{
    [TestFixture]
    public class UserAddress_Tests
    {


        private IBcDbContext _dbContext;
        private IUserAddressService _userAddressService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._dbContext = BrEngineContext.Current.Resolve<IBcDbContext>();
            this._userAddressService = BrEngineContext.Current.Resolve<IUserAddressService>();
        }

        [Test]
        public void GetUserAllAddress_Test()
        {
            SearchUserAddressModel model = new SearchUserAddressModel() { UserID = 116070 };
            int languageID = 1;
            var result = _userAddressService.GetUserAllAddress(model, languageID);
            Assert.IsTrue(result.IsValid);
            Assert.NotNull(result.Data);
        }
    }
}
