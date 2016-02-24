using System;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Core;
using HKTHMall.Domain.Models.LoginLog;
using HKTHMall.Services.LoginLog;
using NUnit.Framework;
using System.Collections.Generic;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;

namespace HKTHMall.Services.Tests.LoginLog
{
    [TestFixture]
    public class AC_OperateLogService_Test
    {
        private IAC_OperateLogService _userLoginLogService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._userLoginLogService = BrEngineContext.Current.Resolve<IAC_OperateLogService>();
        }

        [Test]
        public void AC_OperateLogService_AddAC_OperateLog_Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var resultModel = this._userLoginLogService.AddAC_OperateLog(new AC_OperateLogModel()
                {
                    OperateID = MemCacheFactory.GetCurrentMemCache().Increment("logId"),
                    UserID = 12,
                    OperateName = "111",
                    IP = "1111", //ToolUtil.GetRealIP(),
                    OperateContent = "测试w",
                    Remark = "阿斯蒂芬",
                    OperateTime = DateTime.Now.AddDays(-1)
                });

                Assert.IsTrue(resultModel.IsValid);
            }
            
        }

        [Test]
        public void AC_OperateLogService_GetAC_OperateLogById_Test()
        {
            AC_OperateLogModel resultModel = this._userLoginLogService.GetAC_OperateLogById(546145124).Data;
            Assert.IsTrue(true);
        }

        [Test]
        public void AC_OperateLogService_GetAC_OperateLogList_Test()
        {
            //分页查询,没有数据
            List<AC_OperateLogModel> resultModel = this._userLoginLogService.GetAC_OperateLogList(new SearchAC_OperateLogModel() { PagedIndex = 0, PagedSize = 2, EndOperateTime = DateTime.Now }).Data;
            Assert.IsTrue(true);
            
        }

        [Test]
        public void AC_OperateLogService_UpdateAC_OperateLog_Test()
        {
            var resultModel = this._userLoginLogService.UpdateAC_OperateLog(new AC_OperateLogModel()
            {
                OperateID = 546145114,
                UserID = 10,
                OperateName = "122",
                IP ="192.168.2.5", //ToolUtil.getUserIp(),
                OperateContent = "测试得到",
                Remark = "的反对法",
                OperateTime = DateTime.Now
            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void AC_OperateLogService_DeleteAC_OperateLog_Test()
        {
            var resultModel = this._userLoginLogService.DeleteAC_OperateLog(new AC_OperateLogModel()
            {
                OperateID = 546145124,


            });

            Assert.IsTrue(resultModel.IsValid);
        }
    }
}
