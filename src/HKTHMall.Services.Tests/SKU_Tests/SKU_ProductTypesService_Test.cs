using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using BrCms.Framework.Infrastructure;
using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Services.SKU;
using HKTHMall.Services.Users;
using NUnit.Framework;

namespace HKTHMall.Services.Tests.SKU_Tests
{

    [TestFixture]
    public class SKU_ProductTypesService_Test
    {
        private ISKU_ProductTypesService _SKU_ProductTypesService;

        private IAC_UserService _AC_UserService;

        [SetUp]
        public void Init()
        {
            BrEngineContext.Init(null);
            this._SKU_ProductTypesService = BrEngineContext.Current.Resolve<ISKU_ProductTypesService>();
            this._AC_UserService = BrEngineContext.Current.Resolve<IAC_UserService>();


        }

        [Test]
        public void AddSKU_Attributes_Test()
        {
            var result = _SKU_ProductTypesService.GetSKU_ProductTypesById(1);
            Assert.NotNull(result.Data);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void GetPagingSKU_ProductTypes_Test()
        {
            SearchSKU_ProductTypesModel searchModel = new SearchSKU_ProductTypesModel()
            {
                PagedIndex = 1,
                PagedSize = 10
            };
            var result = _SKU_ProductTypesService.GetPagingSKU_ProductTypes(searchModel);
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Data.TotalCount > 0);
        }

        [Test]
        public void AddSKU_ProductTypes1_Test()
        {
            var resultModel = _AC_UserService.GetAC_UserById(973840311);

            //测试1
            SKU_ProductTypesModel productTypesModel = new SKU_ProductTypesModel()
            {
                IsGoods = 1,
                IsUse = 0,
                CreateBy = resultModel.Data.UserName,
                CreateDT = DateTime.Now,
                Name = "手机",
                Remark = "",
                RowStatus = 0,
                UpdateBy = resultModel.Data.UserName,
                UpdateDT = DateTime.Now,
                UseExtend = 0,
                UseParameter = 0,
                StandardAttributeModels = new List<SKU_ProductTypeAttributeModel>()
                 {
                     new SKU_ProductTypeAttributeModel()
                     {
                         AttributeGroup="",
                         AttributeId=35,
                         AttributeType=0,
                         CreateBy=resultModel.Data.UserName,
                         CreateDT=DateTime.Now,
                         DisplaySequence=1,
                         RowStatus=0,
                         UpdateBy=resultModel.Data.UserName,
                         UpdateDT = DateTime.Now
                        
                     },
                     new SKU_ProductTypeAttributeModel()
                     {
                         AttributeGroup="",
                         AttributeId=36,
                         AttributeType=0,
                         CreateBy=resultModel.Data.UserName,
                         CreateDT=DateTime.Now,
                         DisplaySequence=1,
                         RowStatus=0,
                         UpdateBy=resultModel.Data.UserName,
                         UpdateDT = DateTime.Now
                     }
                 }
            };

            var result = _SKU_ProductTypesService.AddSKU_ProductTypes(productTypesModel);
            Assert.IsTrue(result.IsValid);


        }

        [Test]
        public void AddSKU_ProductTypes2_Test()
        {
            var resultModel = _AC_UserService.GetAC_UserById(973840311);

            //测试2,启用扩展属性

            SKU_ProductTypesModel productTypesModel = new SKU_ProductTypesModel()
            {
                IsGoods = 1,
                IsUse = 0,
                CreateBy = resultModel.Data.UserName,
                CreateDT = DateTime.Now,
                Name = "篮球",
                Remark = "",
                RowStatus = 0,
                UpdateBy = resultModel.Data.UserName,
                UpdateDT = DateTime.Now,
                UseExtend = 1,
                UseParameter = 0,
                StandardAttributeModels = new List<SKU_ProductTypeAttributeModel>()
                {
                    new SKU_ProductTypeAttributeModel()
                    {
                        AttributeGroup="",
                        AttributeId=35,
                        AttributeType=0,
                        CreateBy=resultModel.Data.UserName,
                        CreateDT=DateTime.Now,
                        DisplaySequence=1,
                        RowStatus=0,
                        UpdateBy=resultModel.Data.UserName,
                        UpdateDT = DateTime.Now
                        
                    },
                    new SKU_ProductTypeAttributeModel()
                    {
                        AttributeGroup="",
                        AttributeId=35,
                        AttributeType=0,
                        CreateBy=resultModel.Data.UserName,
                        CreateDT=DateTime.Now,
                        DisplaySequence=1,
                        RowStatus=-1,
                        UpdateBy=resultModel.Data.UserName,
                        UpdateDT = DateTime.Now
                    }
                },
                UseExtendAttributeModels = new List<SKU_ProductTypeAttributeModel>()
                {
                    new SKU_ProductTypeAttributeModel()
                    {
                        RowStatus=-1,
                        DisplaySequence=1,
                        
                        SKU_AttributesModel=new SKU_AttributesModel()
                        {
                            AttributeName="气门",
                            AttributeType=0,
                            IsSearch=0,
                            IsSKU=0,
                            RowStatus=0,
                            UsageMode=0,
                            SKU_AttributeValuesModels=new List<SKU_AttributeValuesModel>()
                            {
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=0,
                                    RowStatus=0,
                                    ValueStr="新科技"
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=2,
                                    RowStatus=0,
                                    ValueStr="传统"
                                }
                            }
                        }
                    },
                    new SKU_ProductTypeAttributeModel()
                    {
                        RowStatus=0,
                        DisplaySequence=1,
                        
                        
                        SKU_AttributesModel=new SKU_AttributesModel()
                        {
                            AttributeName="皮质",
                            AttributeType=0,
                            IsSearch=0,
                            IsSKU=0,
                            RowStatus=0,
                            UsageMode=0,
                            SKU_AttributeValuesModels=new List<SKU_AttributeValuesModel>()
                            {
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=0,
                                    RowStatus=0,
                                    ValueStr="牛皮"
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=2,
                                    RowStatus=0,
                                    ValueStr="仿真皮"
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=1,
                                    RowStatus=-1,
                                    ValueStr="塑料"
                                }
                            }
                        }
                    }
                }
            };

            var result = _SKU_ProductTypesService.AddSKU_ProductTypes(productTypesModel);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void AddSKU_ProductTypes3_Test()
        {
            var resultModel = _AC_UserService.GetAC_UserById(973840311);

            //测试2,启用扩展属性

            SKU_ProductTypesModel productTypesModel = new SKU_ProductTypesModel()
            {
                IsGoods = 1,
                IsUse = 0,
                CreateBy = resultModel.Data.UserName,
                CreateDT = DateTime.Now,
                Name = "运动鞋",
                Remark = "",
                RowStatus = 0,
                UpdateBy = resultModel.Data.UserName,
                UpdateDT = DateTime.Now,
                UseExtend = 1,
                UseParameter = 1,
                StandardAttributeModels = new List<SKU_ProductTypeAttributeModel>()
                {
                    new SKU_ProductTypeAttributeModel()
                    {
                        AttributeGroup="",
                        AttributeId=35,
                        AttributeType=0,
                        CreateBy=resultModel.Data.UserName,
                        CreateDT=DateTime.Now,
                        DisplaySequence=1,
                        RowStatus=0,
                        UpdateBy=resultModel.Data.UserName,
                        UpdateDT = DateTime.Now
                        
                    },
                    new SKU_ProductTypeAttributeModel()
                    {
                        AttributeGroup="",
                        AttributeId=35,
                        AttributeType=0,
                        CreateBy=resultModel.Data.UserName,
                        CreateDT=DateTime.Now,
                        DisplaySequence=1,
                        RowStatus=-1,
                        UpdateBy=resultModel.Data.UserName,
                        UpdateDT = DateTime.Now
                    }
                },
                UseExtendAttributeModels = new List<SKU_ProductTypeAttributeModel>()
                {
                    new SKU_ProductTypeAttributeModel()
                    {
                        RowStatus=0,
                        DisplaySequence=1,
                        
                        
                        SKU_AttributesModel=new SKU_AttributesModel()
                        {
                            AttributeName="气味",
                            AttributeType=0,
                            IsSearch=0,
                            IsSKU=0,
                            RowStatus=0,
                            UsageMode=0,
                            SKU_AttributeValuesModels=new List<SKU_AttributeValuesModel>()
                            {
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=0,
                                    RowStatus=0,
                                    ValueStr="防臭"
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=2,
                                    RowStatus=0,
                                    ValueStr="按摩"
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=1,
                                    RowStatus=0,
                                    ValueStr="传统"
                                }
                            }
                        }
                    }
                },
                UseParamAttributeModels = new List<SKU_ProductTypeAttributeModel>()
                {

                    new SKU_ProductTypeAttributeModel()
                    {
                        AttributeGroup="主体",
                        RowStatus=0,
                        DisplaySequence=0,
                        SKU_AttributesModel=new SKU_AttributesModel()
                        {
                            AttributeName="品牌",
                            UsageMode=0,
                            AttributeType=0,
                            SKU_AttributeValuesModels=new List<SKU_AttributeValuesModel>()
                            {
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=1,
                                    ValueStr="耐克",
                                    RowStatus=0
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=2,
                                    ValueStr="特步",
                                    RowStatus=0
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=3,
                                    ValueStr="安踏",
                                    RowStatus=-1
                                }
                            }
                        }


                    },
                    new SKU_ProductTypeAttributeModel()
                    {
                        AttributeGroup="主体",
                        RowStatus=0,
                        DisplaySequence=0,
                        SKU_AttributesModel=new SKU_AttributesModel()
                        {
                            AttributeName="码数",
                            UsageMode=0,
                            AttributeType=0,
                            SKU_AttributeValuesModels=new List<SKU_AttributeValuesModel>()
                            {
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=1,
                                    ValueStr="22",
                                    RowStatus=0
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=2,
                                    ValueStr="30",
                                    RowStatus=0
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=3,
                                    ValueStr="40",
                                    RowStatus=-1
                                }
                            }
                        }
                    },
                    new SKU_ProductTypeAttributeModel()
                    {
                        AttributeGroup="款式",
                        RowStatus=0,
                        DisplaySequence=0,
                        SKU_AttributesModel=new SKU_AttributesModel()
                        {
                            AttributeName="款式",
                            UsageMode=0,
                            AttributeType=0,
                            SKU_AttributeValuesModels=new List<SKU_AttributeValuesModel>()
                            {
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=1,
                                    ValueStr="网布",
                                    RowStatus=0
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=2,
                                    ValueStr="板鞋",
                                    RowStatus=0
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=3,
                                    ValueStr="布鞋",
                                    RowStatus=0
                                }
                            }
                        }
                    }
                }
            };

            var result = _SKU_ProductTypesService.AddSKU_ProductTypes(productTypesModel);
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        public void UpdateSKU_ProductTypes1_Test()
        {
            var userResultModel = _AC_UserService.GetAC_UserById(973840311);

            var tempResult = _SKU_ProductTypesService.GetSKU_ProductTypesById(20);

            SKU_ProductTypesModel model = tempResult.Data;

            model.StandardAttributeModels[0].DisplaySequence = 2;

            model.StandardAttributeModels.Add(new SKU_ProductTypeAttributeModel()
            {

                AttributeId = 42,
                AttributeType = 0,
                CreateBy = userResultModel.Data.UserName,
                CreateDT = DateTime.Now,
                DisplaySequence = 3,
                RowStatus = 0,
                UpdateBy = userResultModel.Data.UserName,
                UpdateDT = DateTime.Now
            });

            model.StandardAttributeModels.Add(new SKU_ProductTypeAttributeModel()
            {

                AttributeGroup = "",
                AttributeId = 42,
                AttributeType = 0,
                CreateBy = userResultModel.Data.UserName,
                CreateDT = DateTime.Now,
                DisplaySequence = 1,
                RowStatus = -1,
                UpdateBy = userResultModel.Data.UserName,
                UpdateDT = DateTime.Now
            });

            var result = _SKU_ProductTypesService.UpdateSKU_ProductTypes(model);

        }


        [Test]
        public void UpdateSKU_ProductTypes2_Test()
        {
            var userResultModel = _AC_UserService.GetAC_UserById(973840311);

            var tempResult = _SKU_ProductTypesService.GetSKU_ProductTypesById(22);

            SKU_ProductTypesModel model = tempResult.Data;

            model.StandardAttributeModels[0].DisplaySequence = 2;
            model.Name = "儿童球类";

            model.UseExtendAttributeModels.Add(new SKU_ProductTypeAttributeModel()
            {

                AttributeId = 42,
                AttributeType = 0,
                CreateBy = userResultModel.Data.UserName,
                CreateDT = DateTime.Now,
                DisplaySequence = 3,
                RowStatus = 2,
                UpdateBy = userResultModel.Data.UserName,
                UpdateDT = DateTime.Now,
                SKU_AttributesModel = new SKU_AttributesModel()
                {
                    AttributeName = "气门",
                    AttributeType = 0,
                    IsSearch = 0,
                    IsSKU = 0,
                    RowStatus = 0,
                    UsageMode = 0,
                    SKU_AttributeValuesModels = new List<SKU_AttributeValuesModel>()
                            {
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=0,
                                    RowStatus=0,
                                    ValueStr="新科技"
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=2,
                                    RowStatus=0,
                                    ValueStr="传统"
                                }
                            }
                }

            });

            model.UseExtendAttributeModels.Add(new SKU_ProductTypeAttributeModel()
            {

                AttributeId = 42,
                AttributeType = 0,
                CreateBy = userResultModel.Data.UserName,
                CreateDT = DateTime.Now,
                DisplaySequence = 1,
                RowStatus = -1,
                UpdateBy = userResultModel.Data.UserName,
                UpdateDT = DateTime.Now,
                SKU_AttributesModel = new SKU_AttributesModel()
                {
                    AttributeName = "气门2",
                    AttributeType = 0,
                    IsSearch = 0,
                    IsSKU = 0,
                    RowStatus = 0,
                    UsageMode = 0,
                    SKU_AttributeValuesModels = new List<SKU_AttributeValuesModel>()
                            {
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=0,
                                    RowStatus=0,
                                    ValueStr="新科技2"
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=2,
                                    RowStatus=0,
                                    ValueStr="传统2"
                                }
                            }
                }
            });

            var result = _SKU_ProductTypesService.UpdateSKU_ProductTypes(model);

        }

        [Test]
        public void UpdateSKU_ProductTypes3_Test()
        {
            var userResultModel = _AC_UserService.GetAC_UserById(973840311);

            var tempResult = _SKU_ProductTypesService.GetSKU_ProductTypesById(23);

            SKU_ProductTypesModel model = tempResult.Data;

            model.UseParamAttributeModels.Add(new SKU_ProductTypeAttributeModel()
            {
                AttributeGroup = "主体",
                AttributeType = 0,
                CreateBy = userResultModel.Data.UserName,
                CreateDT = DateTime.Now,
                DisplaySequence = 3,
                RowStatus = 2,
                UpdateBy = userResultModel.Data.UserName,
                UpdateDT = DateTime.Now,
                SKU_AttributesModel = new SKU_AttributesModel()
                {
                    AttributeName = "鞋带",
                    AttributeType = 0,
                    IsSearch = 0,
                    IsSKU = 0,
                    RowStatus = 0,
                    UsageMode = 0,
                    SKU_AttributeValuesModels = new List<SKU_AttributeValuesModel>()
                            {
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=0,
                                    RowStatus=0,
                                    ValueStr="双鞋带"
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=2,
                                    RowStatus=0,
                                    ValueStr="单鞋带"
                                },
                                new SKU_AttributeValuesModel()
                                {
                                    DisplaySequence=2,
                                    RowStatus=0,
                                    ValueStr="无鞋带"
                                }
                            }
                }

            });


            model.UseParamAttributeModels[0].RowStatus = -1;
            model.UseParamAttributeModels[1].SKU_AttributesModel.AttributeName = "尺码";
            model.UseParamAttributeModels[2].SKU_AttributesModel.SKU_AttributeValuesModels[2].RowStatus = -1;

            var result = _SKU_ProductTypesService.UpdateSKU_ProductTypes(model);

        }

        [Test]
        public void GetSku_ProductTypesByCategoryId_Test()
        {
            var result = this._SKU_ProductTypesService.GetSku_ProductTypesByCategoryId(3);
            Assert.IsTrue(result.IsValid);
        }
    }
}

