using BrCms.Framework.Infrastructure;
using HKTHMall.Services.Products;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using HKTHMall.Core;
using HKTHMall.Domain.Models.Categoreis;
using HKTHMall.Domain.WebModel.Models.Product;

namespace HKTHMall.Services.Tests.Products_Tests
{
    public class SP_ProductCommentService_test
    {
        [TestFixtureSetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._sp_productCommentService = BrEngineContext.Current.Resolve<ISP_ProductCommentService>();

        }

        private ISP_ProductCommentService _sp_productCommentService;

        [Test]
        public void SP_ProductCommentService_AddSP_ProductComment_Test()
        {
            for (int i = 0; i < 10; i++)
            {
                var resultModel = this._sp_productCommentService.AddSP_ProductComment(new SP_ProductCommentModel()
                {
                    ProductId = 2789830556,
                    OrderId = 2776304866,
                    UserID = 12,
                    CommentLevel = 1,
                    CommentContent = "111",
                    CommentDT = DateTime.Now, //ToolUtil.GetRealIP(),
                    IsAnonymous = 0,
                    CheckStatus = 1,
                    CheckBy = "",
                    CheckDT = DateTime.Now,
                    ReplyBy = "水电费",
                    ReplyDT = DateTime.Now,
                    ReplyContent = "阿斯蒂芬"
                });

                Assert.IsTrue(resultModel.IsValid);
            }

        }

        [Test]
        public void SP_ProductCommentService_GetSP_ProductCommentById_Test()
        {
            SP_ProductCommentModel resultModel = this._sp_productCommentService.GetSP_ProductCommentById(5).Data;
            Assert.IsTrue(true);
        }

        [Test]
        public void SP_ProductCommentService_GetSP_ProductCommentList_Test()
        {
            //分页查询,没有数据
            List<SP_ProductCommentModel> resultModel = this._sp_productCommentService.GetSP_ProductCommentList(new SearchSP_ProductCommentModel() { PagedIndex = 0, PagedSize = 2 }).Data;
            Assert.IsTrue(true);

        }

        [Test]
        public void SP_ProductCommentService_UpdateSP_ProductComment_Test()
        {
            var resultModel = this._sp_productCommentService.UpdateSP_ProductComment(new SP_ProductCommentModel()
            {
                ProductCommentId = 5,
                ProductId = 1,
                OrderId = MemCacheFactory.GetCurrentMemCache().Increment("logId"),
                UserID = 12,
                CommentLevel = 1,
                CommentContent = "阿斯蒂芬" + DateTime.Now.Second,
                CommentDT = DateTime.Now.AddDays(-1), //ToolUtil.GetRealIP(),
                IsAnonymous = 0,
                CheckStatus = 0,
                CheckBy = "",
                CheckDT = DateTime.Now.AddDays(-1),
                ReplyBy = "水电费",
                ReplyDT = DateTime.Now.AddDays(-1),
                ReplyContent = "阿斯蒂芬"
            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void SP_ProductCommentService_DeleteSP_ProductComment_Test()
        {
            var resultModel = this._sp_productCommentService.DeleteSP_ProductComment(new SP_ProductCommentModel()
            {
                ProductCommentId = 10,


            });

            Assert.IsTrue(resultModel.IsValid);
        }

        [Test]
        public void BatchAddSP_ProductComment_Test()
        {
            List<SP_ProductCommentModel> comments = new List<SP_ProductCommentModel>() 
            {
                new SP_ProductCommentModel()
                    {
                        ProductId = 1,
                        OrderId = 973840311123,
                        UserID = 973840311,
                        CommentLevel = 5,
                        CommentContent = "东西不错,值得购买",
                        CommentDT = DateTime.Now,
                        IsAnonymous = 0,
                        CheckStatus = 1,
                        CheckBy = "",
                        CheckDT = DateTime.Now
                    }
            };

            var result = _sp_productCommentService.BatchAddSP_ProductComment(comments);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.IsValid);

        }

        [Test]
        public void GetPaingCommentsIntoWeb_Test()
        {
            SearchOrderProductCommentView searchModel = new SearchOrderProductCommentView()
            {
                UserID = 973840311,
                page = 1,
                pageSize = 2,
                LanguageID = 1
            };

            var result = _sp_productCommentService.GetPaingCommentsIntoWeb(searchModel);
            Assert.IsTrue(result.IsValid);
            Assert.NotNull(result.Data);
            Assert.IsTrue(result.Data.TotalCount);

        }
    }
}
