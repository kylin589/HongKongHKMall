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
    public class UserLoginLogService_Test
    {
        private IUserLoginLogService _userLoginLogService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._userLoginLogService = BrEngineContext.Current.Resolve<IUserLoginLogService>();
        }

        [Test]
        public void UserLoginLogService_AddUserLoginLog_Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var resultModel = this._userLoginLogService.AddUserLoginLog(new UserLoginLogModel()
                {
                    UserID = 14,
                    UserName = "苏打粉",
                    LoginSource = DateTime.Now.Second,
                    IP = "192.168.2.4",
                    LoginAddress = "测试w",
                    LoginTime = DateTime.Now.AddDays(-1)
                });

                Assert.IsTrue(resultModel.IsValid);
            }
            
        }

        [Test]
        public void UserLoginLogService_GetUserLoginLogById_Test()
        {
            UserLoginLogModel resultModel = this._userLoginLogService.GetUserLoginLogById(1).Data;
            Assert.IsTrue(true);
        }

        [Test]
        public void UserLoginLogService_GetUserLoginLog_Test()
        {
            //分页查询,没有数据
            List<UserLoginLogModel> resultModel = this._userLoginLogService.GetUserLoginLog(new SearchUserLoginLogModel() { PagedIndex = 0, PagedSize = 100 }).Data;
            Assert.IsTrue(true);
            resultModel = this._userLoginLogService.GetUserLoginLog(new SearchUserLoginLogModel() { PagedIndex = 0, PagedSize = 1, UserName = "苏打" }).Data;
            Assert.IsTrue(true);
            resultModel = this._userLoginLogService.GetUserLoginLog(new SearchUserLoginLogModel() { PagedIndex = 0, PagedSize = 1, UserName = "苏打", BeginLoginTime = Convert.ToDateTime("2015-07-02 16:07:50.593") }).Data;
            Assert.IsTrue(true);
            resultModel = this._userLoginLogService.GetUserLoginLog(new SearchUserLoginLogModel() { PagedIndex = 0, PagedSize = 2, BeginLoginTime = Convert.ToDateTime("2015-07-02 16:07:50.593"),EndLoginTime=DateTime.Now }).Data;
            Assert.IsTrue(true);
            resultModel = this._userLoginLogService.GetUserLoginLog(new SearchUserLoginLogModel() { PagedIndex = 0, PagedSize = 2,  EndLoginTime = DateTime.Now }).Data;
            Assert.IsTrue(true);
        }

        [Test]
        public void UserLoginLogService_UpdateUserLoginLog_Test()
        {
            var resultModel = this._userLoginLogService.UpdateUserLoginLog(new UserLoginLogModel()
            {
                ID = 1,
                UserID = DateTime.Now.Second,
                LoginSource = DateTime.Now.Second,
                IP = "192.168.1.12",
                LoginAddress = "单111111独",
                LoginTime = DateTime.Now
            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void UserLoginLogService_DeleteUserLoginLog_Test()
        {
            var resultModel = this._userLoginLogService.DeleteUserLoginLog(new UserLoginLogModel()
            {
                ID = 6,


            });

            Assert.IsTrue(resultModel.IsValid);
        }
    }
}
