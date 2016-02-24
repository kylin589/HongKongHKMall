using BrCms.Framework.Infrastructure;
using HKTHMall.Services.New;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using HKTHMall.Domain.AdminModel.Models.New;
using HKTHMall.Core;

namespace HKTHMall.Services.Tests.New
{
    public class NewInfoService_Test
    {
        private INewInfoService _newInfoService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._newInfoService = BrEngineContext.Current.Resolve<INewInfoService>();
        }

        [Test]
        public void NewInfoService_AddNewInfo_Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var resultModel = this._newInfoService.AddNewInfo(new NewInfoModel()
                {
                    NewInfoId = MemCacheFactory.GetCurrentMemCache().Increment("logId"),
                    NewType=1,
                    NewTitle="测试",
                    NewContent="测试000000",
                    IsRecommend=0,
                    NewImage="",
                    CreateBy="admin",
                    CreateDT=DateTime.Now,
                    UpdateBy="admin",
                    UpdateDT=DateTime.Now
                });

                Assert.IsTrue(resultModel.IsValid);
            }

        }

        [Test]
        public void NewInfoService_GetNewInfoList_Test()
        {
            SearchNewInfoModel snmodel = new SearchNewInfoModel();
            snmodel.PagedIndex = 0;
            snmodel.PagedSize = 10;
            List<NewInfoModel> resultModel = this._newInfoService.GetNewInfoList(snmodel).Data;
            Assert.IsTrue(true);
        }

        [Test]
        public void NewInfoService_UpdateNewInfo_Test()
        {
            
                var resultModel = this._newInfoService.UpdateNewInfo(new NewInfoModel()
                {
                    NewInfoId = 1560575003,
                    NewType = 1,
                    NewTitle = "测试",
                    NewContent = "测试000000",
                    IsRecommend = 1,
                    NewImage = "",
                    CreateBy = "admin",
                    CreateDT = DateTime.Now,
                    UpdateBy = "admin",
                    UpdateDT = DateTime.Now
                });

                Assert.IsTrue(resultModel.IsValid);
            

        }
    }
}
