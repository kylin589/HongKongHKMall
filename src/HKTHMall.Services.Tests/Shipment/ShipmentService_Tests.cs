using System.Collections.Generic;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Shipment;
using HKTHMall.Services.Shipment;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.Shipment
{
    [TestFixture]
    public class ShipmentService_Tests
    {
        private IShipmentService shipmentService;
        [TestFixtureSetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this.shipmentService = BrEngineContext.Current.Resolve<IShipmentService>();
        }

        //[Test]
        //public void ShipmentService_AddShipment_Test()
        //{
        //    var result = this.shipmentService.AddShipment(new YF_FareTemplateModel()
        //    {
        //        MerchantID = 1,
        //        Name = "dddd",
        //        ShipmentTemplateModels = new List<ShipmentTemplateModel>()
        //        {
        //            new ShipmentTemplateModel()
        //            {
        //                CityIds = "12,65,789",
        //                Price1 = 100,
        //                Price2 = 200,
        //                Price3 = 300,
        //                Price4 = 400,
        //                Price5 = 500,
        //                THAreaID = 12
        //            }
        //        }
        //    });

        //    Assert.IsTrue(result.IsValid);
        //}

        [Test]
        public void ShipmentService_UpdateShipment_Test()
        {
            var result = this.shipmentService.UpdateShipment(new YF_FareTemplateModel());
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void ShipmentService_DeleteShipment_Test()
        {
            var result = this.shipmentService.DeleteShipment(new List<int>() {1, 2});
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void ShipmentService_Search_Test()
        {
            var result = this.shipmentService.SearchShipment(new SearchShipmentModel());
            Assert.IsTrue(result.IsValid);
        }
    }
}