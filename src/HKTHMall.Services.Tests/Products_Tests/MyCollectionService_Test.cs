using BrCms.Framework.Data;
using BrCms.Framework.Infrastructure;
using HKTHMall.Services.YHUser;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using HKTHMall.Core;

namespace HKTHMall.Services.Tests.Products_Tests
{
    [TestFixture]
    public class MyCollectionService_Test
    {
        private IDatabaseHelper _database;
        private IMyCollectionService _myCollectionService;

        [TestFixtureSetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            _database = BrEngineContext.Current.Resolve<IDatabaseHelper>();
            _myCollectionService = BrEngineContext.Current.Resolve<IMyCollectionService>();
        }

        [Test]
        public void MyCollection_Add_Test()
        {
            //long dd = MemCacheFactory.GetCurrentMemCache().Increment("commonId");
            //var resultModel = this._myCollectionService.AddFavorites(10001,10001);

            //Assert.IsTrue(resultModel.IsValid);
        }
    }
}
