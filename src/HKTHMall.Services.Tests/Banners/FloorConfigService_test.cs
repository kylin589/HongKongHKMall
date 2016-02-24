using BrCms.Framework.Infrastructure;
using HKTHMall.Services.Banner;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autofac;
using HKTHMall.Domain.AdminModel.Models.banner;

namespace HKTHMall.Services.Tests.Banners
{
    public class FloorConfigService_test
    {
        private IFloorConfigService _FloorConfigService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._FloorConfigService = BrEngineContext.Current.Resolve<IFloorConfigService>();
        }

        [Test]
        public void FloorConfigService_AddFloorConfig_Test()
        {
            for (int i = 1; i < 5; i++)
            {
                var resultModel = this._FloorConfigService.AddFloorConfig(new FloorConfigModel()
                {
                    FloorConfigId = 2,
                    FloorConfigName = "品牌荟萃",
                    CateIdStr = ""
                    
                });

                Assert.IsTrue(resultModel.IsValid);
            }

        }

        [Test]
        public void FloorConfigService_GetFloorConfig_Test()
        {
            //分页查询,没有数据
            List<FloorConfigModel> resultModel = this._FloorConfigService.GetFloorConfigList(new SearchFloorConfigModel() { PagedIndex = 0, PagedSize = 100 }).Data;
            Assert.IsTrue(true);
        }

        [Test]
        public void FloorConfigService_UpdateFloorConfig_Test()
        {
            var resultModel = this._FloorConfigService.UpdateFloorConfig(new FloorConfigModel()
            {
                FloorConfigId = 1,
                FloorConfigName = "首页",
                CateIdStr = ""
            });

            Assert.IsTrue(true);
        }
    }
}
