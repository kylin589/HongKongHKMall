using System;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Domain.Models.LoginLog;
using HKTHMall.Services.LoginLog;
using NUnit.Framework;
using System.Collections.Generic;

namespace HKTHMall.Services.Tests.LoginLog
{
    [TestFixture]
    public class YH_UserLoginLogService_Test
    {
        private IYH_UserLoginLogService _userLoginLogService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._userLoginLogService = BrEngineContext.Current.Resolve<IYH_UserLoginLogService>();
        }

        [Test]
        public void YH_UserLoginLogService_AddCategory_Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var resultModel = this._userLoginLogService.AddYH_UserLogin(new YH_UserLoginLogModel()
                {
                    UserID = DateTime.Now.Second,
                    LoginSource = Convert.ToByte(DateTime.Now.Second),
                    IP = "192.168.2." + DateTime.Now.Second.ToString(),
                    LoginAddress = "测试w",
                    LoginTime = DateTime.Now.AddDays(-1)
                });

                Assert.IsTrue(resultModel.IsValid);
            }
            
        }

        [Test]
        public void YH_UserLoginLogService_GetYH_UserLoginLogById_Test()
        {
            YH_UserLoginLogModel resultModel = this._userLoginLogService.GetYH_UserLoginLogById(1).Data;
            Assert.IsTrue(true);

            
        }

        [Test]
        public void YH_UserLoginLogService_GetYH_UserLogin_Test()
        {
            //分页查询,没有数据
            List<YH_UserLoginLogModel> resultModel = this._userLoginLogService.GetYH_UserLogin(new SearchYH_UserLoginLogModel() { PagedIndex = 0, PagedSize = 100 }).Data;
            Assert.IsTrue(true);
            resultModel = this._userLoginLogService.GetYH_UserLogin(new SearchYH_UserLoginLogModel() { PagedIndex = 2, PagedSize = 1 }).Data;
            Assert.IsTrue(true);
            resultModel = this._userLoginLogService.GetYH_UserLogin(new SearchYH_UserLoginLogModel() { PagedIndex = 1, PagedSize = 2 }).Data;
            Assert.IsTrue(true);
        }

        [Test]
        public void YH_UserLoginLogService_UpdateYH_UserLogin_Test()
        {
            var resultModel = this._userLoginLogService.UpdateYH_UserLogin(new YH_UserLoginLogModel()
            {
                ID=1,
                UserID = DateTime.Now.Second,
                LoginSource =Convert.ToByte( DateTime.Now.Second),
                IP = "192.168.1.12",
                LoginAddress = "11单独2222",
                LoginTime = DateTime.Now
            });

            Assert.IsTrue(resultModel.IsValid);



            
        }

        [Test]
        public void YH_UserLoginLogService_DeleteYH_UserLogin_Test()
        {
            var resultModel = this._userLoginLogService.DeleteYH_UserLogin(new YH_UserLoginLogModel()
            {
                ID = 14,
                

            });

            Assert.IsTrue(resultModel.IsValid);
        }
    }
}
