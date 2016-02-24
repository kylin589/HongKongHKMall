using System;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Core;
using HKTHMall.Core.Data;
using HKTHMall.Domain.Models.AC;
using HKTHMall.Domain.Models.User;
using HKTHMall.Services.Users;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.Users_Tests
{
    [TestFixture]
    public class AC_UserService_Test
    {
        private IBcDbContext _dbContext;
        private IAC_UserService _acUserService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._dbContext = BrEngineContext.Current.Resolve<IBcDbContext>();
            this._acUserService = BrEngineContext.Current.Resolve<IAC_UserService>();
        }

        [Test]
        public void AC_UserService_Add_Test()
        {
            var model = new AC_UserModel
            {
                RealName = "ddd",
                //RoleID = 0,
                UserName = "sss",
                Password = "dddd",
                IDNumber = "sss",
                CreateUser = "ddd",
                UpdateUser = "ssss",
                CreateDT = DateTime.Now,
                //LastLoginTime = DateTime.Now,
                UserID = (int)MemCacheFactory.GetCurrentMemCache().Increment("commonId"),
                //AC_Role = new AC_RoleModel()
                //{
                //    //RoleID = 1,
                //    RoleName = "ddd",
                //    RoleModuleValue = "dddd",
                //    RoleDescription = "dddd",
                //    RoleFuctionValue = "ddd",
                //    CreateDT = DateTime.Now,
                //    CreateUser = "sss",
                //    UpdateDt = DateTime.Now,
                //    UpdateUser = "dssss"
                //},
                //AC_Department = new AC_DepartmentModel()
                //{
                //    ParentID = 0,
                //    CreateBy = "ddd",
                //   // CreateDT = DateTime.Now,
                //    DeptName = "sss",
                //    IsActive = 0,
                //    OrderNumber = 24214,
                //    UpdateBy = "ssss",
                //    //UpdateDT = DateTime.Now
                //}
            };

            var result = this._acUserService.AddAC_User(model);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void AC_UserService_Search()
        {
            var result = this._acUserService.GetPagingAC_Users(new SearchUsersModel()
            {
                PagedSize = 15,
                PagedIndex = 0
            });
            Assert.IsTrue(result.Data.Count != 0);
        }

        //[Test]
        //public void TestXml()
        //{
        //    var xml = new XmlDocument();
        //    xml.Load(@"E:\HKTHMall\src\HKTHMall.Domain\Entities\Mapping\../Model1.edmx");
        //    var xmlNodes = xml.Schemas;
        //    //.SelectNodes("//EntityType");



        //    var xdoc = XDocument.Load(@"E:\HKTHMall\src\HKTHMall.Domain\Entities\Mapping\../Model1.edmx");
        //    XNamespace ns = "http://schemas.microsoft.com/ado/2009/11/edm/ssdl";
        //    var xes = xdoc.Descendants(ns + "EntityType");
        //    foreach (var xe in xes)
        //    {
        //        var key = xe.Descendants(ns + "PropertyRef").FirstOrDefault();
        //        var props = xe.Elements(ns + "Property");
        //    }
        //}
    }
}
