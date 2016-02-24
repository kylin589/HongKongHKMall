using BrCms.Framework.Infrastructure;
using HKTHMall.Services.Orders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.Orders;

namespace HKTHMall.Services.Tests.Products_Tests
{
    [TestFixture]
    public class ReturnProductInfoService_Test
    {
        private IReturnProductInfoService _return_GoodsService;

        [TestFixtureSetUp]
        public void SetUp()
        {
            BrEngineContext.Init(null);
            this._return_GoodsService = BrEngineContext.Current.Resolve<IReturnProductInfoService>();
        }

        [Test]
        public void Return_GoodsService_AddReturn_Goods_Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var resultModel = this._return_GoodsService.AddReturnProductInfo(new ReturnProductInfoModel()
                {
                    ReturnOrderID = MemCacheFactory.GetCurrentMemCache().Increment("logId").ToString(),
                    OrderID = "100",
                    UserID = 1,
                    ProductId = 1,
                    ProductSnapshotID=1,
                    ReturnType = 1,
                    ReturnStatus = 1, //ToolUtil.GetRealIP(),
                    TradeAmount=10,
                    RefundAmount=1,
                    ReturntNumber=1,
                    ReasonType=1,
                    Discription="sdfa",
                    //ReturnType=2,
                    ReturnAddress="asdfasdf",
                    ReceiverName="阿道夫",
                    ReceiverMobile="135552554",
                    ReceiverTel="2025555",
                    //ReturnStatus=1,
                    MerchantReturnAddress="asdfas阿斯蒂芬",
                    ReturnText="阿斯蒂芬",
                    CreateTime=DateTime.Now,
                    UpdateTime = DateTime.Now
                    
                },1);

                Assert.IsTrue(resultModel.IsValid);
            }

        }

        [Test]
        public void Return_GoodsService_GetReturn_GoodsById_Test()
        {
            List<ReturnProductInfoModel> resultModel = this._return_GoodsService.GetReturnProductInfoById("1560574260").Data;
            Assert.IsTrue(true);
        }

        [Test]
        public void Return_GoodsService_GetReturn_GoodsList_Test()
        {
            //分页查询,没有数据
            List<ReturnProductInfoModel> resultModel = this._return_GoodsService.GetReturnProductInfoList(new SearchReturnProductInfoModel() { PagedIndex = 0, PagedSize = 1 }).Data;
            List<ReturnProductInfoModel> resultModel1 = this._return_GoodsService.GetReturnProductInfoList(new SearchReturnProductInfoModel() { PagedIndex = 1, PagedSize = 1 }).Data;
            Assert.IsTrue(true);

        }

        [Test]
        public void Return_GoodsService_UpdateReturn_Goods_Test()
        {
            var resultModel = this._return_GoodsService.UpdateReturnProductInfo(new ReturnProductInfoModel()
            {
                ReturnOrderID = "1560574249",
                OrderID = "101",
                UserID = 1,
                ProductId = 1,
                ProductSnapshotID = 1,
                ReturnType = 1,
                ReturnStatus = 1, //ToolUtil.GetRealIP(),
                TradeAmount = 10,
                RefundAmount = 1,
                ReturntNumber = 1,
                ReasonType = 1,
                Discription = "sdfa",
                //ReturnType=2,
                ReturnAddress = "asdfasdf",
                ReceiverName = "阿道夫1111111111111111111111111111111",
                ReceiverMobile = "135552554",
                ReceiverTel = "2025555",
                //ReturnStatus=1,
                MerchantReturnAddress = "asdfas阿斯蒂芬1111111111111",
                ReturnText = "阿斯蒂芬",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void Return_GoodsService_DeleteReturn_Goods_Test()
        {
            var resultModel = this._return_GoodsService.DeleteReturnProductInfo(new ReturnProductInfoModel()
            {
                ReturnOrderID = "1560574268",


            });

            Assert.IsTrue(resultModel.IsValid);
        }

    }
}
