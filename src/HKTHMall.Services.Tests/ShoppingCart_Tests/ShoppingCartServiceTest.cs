using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.WebModel.Models.ShoppingCart;
using HKTHMall.Services.Products;
using HKTHMall.Services.ShoppingCart;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.ShoppingCart_Tests
{
    [TestFixture]
    public class ShoppingCartServiceTest
    {
        private IShoppingCartService _shoppingCartService;

        int languageId = 3;

        [TestFixtureSetUp]
        public void SetUp()
        {
            BrEngineContext.Init(null);
            
            this._shoppingCartService = BrEngineContext.Current.Resolve<IShoppingCartService>();
        }
        
        [Test]
        public void ShoppingCartService_AddShoppingCart_Test()
        {
            var result = this._shoppingCartService.AddShoppingCart(new ShoppingCartModel()
            {
                ShoppingCartId = 10001,
                ProductID = 10001,
                SKU_ProducId = 10001,
                UserID = 10001,
                Quantity = 1,
                CartDate = DateTime.Now
            });

            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void ShoppingCartService_Add_Test()
        {
            long userId = 1;
            var result = this._shoppingCartService.GetShoppingCartByUserId(userId.ToString(), languageId);
            Assert.IsTrue(result.IsValid);
        }

    }
}
