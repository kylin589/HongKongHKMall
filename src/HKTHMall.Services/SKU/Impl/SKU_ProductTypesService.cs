using System;
using System.Collections.Generic;
using System.Linq;
using BrCms.Framework.Collections;
using HKTHMall.Core;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Domain.Models;
using Simple.Data;

namespace HKTHMall.Services.SKU.Impl
{
    /// <summary>
    ///     商品类型服务类
    /// </summary>
    public class SKU_ProductTypesService : BaseService, ISKU_ProductTypesService
    {
        /// <summary>
        ///     属性服务类
        /// </summary>
        private readonly SKU_AttributesService _SKU_AttributesService;

        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="skuAttributesService"></param>
        public SKU_ProductTypesService(SKU_AttributesService skuAttributesService)
        {
            this._SKU_AttributesService = skuAttributesService;
        }

        /// <summary>
        ///     获取商品类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel GetSKU_ProductTypesById(int id)
        {
            var productTypesModel = new SKU_ProductTypesModel();
            dynamic skuTypeAttr;
            dynamic skuAttr;
            dynamic skuAttrValues;
            var query = this._database.Db.SKU_ProductTypes
                .FindAllBySkuTypeId(id)
                //连接
                .Join(this._database.Db.SKU_ProductTypeAttribute, out skuTypeAttr)
                .On(skuTypeAttr.SkuTypeId == this._database.Db.SKU_ProductType.SkuTypeId)

                //连接属性表
                .Join(this._database.Db.SKU_Attributes, out skuAttr)
                .On(skuAttr.AttributeId == skuTypeAttr.AttributeId)

                ////连接属性值表
                .LeftJoin(this._database.Db.SKU_AttributeValues, out skuAttrValues)
                .On(this._database.Db.SKU_AttributeValues.AttributeId == skuAttr.AttributeId)
                .Select(
                    this._database.Db.SKU_ProductTypes.SkuTypeId,
                    this._database.Db.SKU_ProductTypes.Name,
                    this._database.Db.SKU_ProductTypes.IsGoods,
                    this._database.Db.SKU_ProductTypes.UseExtend,
                    this._database.Db.SKU_ProductTypes.UseParameter,
                    this._database.Db.SKU_ProductTypes.Remark,
                    this._database.Db.SKU_ProductTypes.IsUse,
                    this._database.Db.SKU_ProductTypes.CreateBy,
                    this._database.Db.SKU_ProductTypes.CreateDT,
                    this._database.Db.SKU_ProductTypes.UpdateBy,
                    this._database.Db.SKU_ProductTypes.UpdateDT,
                    skuTypeAttr.SKU_ProductTypeAttributeId.As("b_SKU_ProductTypeAttributeId"),
                    skuTypeAttr.SkuTypeId.As("b_SkuTypeId"),
                    skuTypeAttr.AttributeId.As("b_AttributeId"),
                    skuTypeAttr.AttributeType.As("b_AttributeType"),
                    skuTypeAttr.AttributeGroup.As("b_AttributeGroup"),
                    skuTypeAttr.DisplaySequence.As("b_DisplaySequence"),
                    skuTypeAttr.CreateBy.As("b_CreateBy"),
                    skuTypeAttr.CreateDT.As("b_CreateDT"),
                    skuTypeAttr.UpdateBy.As("b_UpdateBy"),
                    skuTypeAttr.UpdateDT.As("b_UpdateDT"),
                    skuAttr.AttributeId.As("c_AttributeId"),
                    skuAttr.AttributeName.As("c_AttributeName"),
                    skuAttr.AttributeType.As("c_AttributeType"),
                    skuAttr.IsSKU.As("c_IsSKU"),
                    skuAttr.UsageMode.As("c_UsageMode"),
                    skuAttr.IsSearch.As("c_IsSearch"),
                    skuAttr.Remark.As("c_Remark"),
                    skuAttr.CreateBy.As("c_CreateBy"),
                    skuAttr.CreateDT.As("c_CreateDT"),
                    skuAttr.UpdateBy.As("c_UpdateBy"),
                    skuAttr.UpdateDT.As("c_UpdateDT"),
                    skuAttrValues.ValueId.As("d_ValueId"),
                    skuAttrValues.AttributeId.As("d_AttributeId"),
                    skuAttrValues.DisplaySequence.As("d_DisplaySequence"),
                    skuAttrValues.ValueStr.As("d_ValueStr"),
                    skuAttrValues.ImageUrl.As("d_ImageUrl"),
                    skuAttrValues.ValuesGroup.As("d_ValuesGroup")
                );
            List<dynamic> queryResult = query.ToList();

            if (queryResult != null && queryResult.Count > 0)
            {
                productTypesModel = this.GenerateProductTypes(queryResult[0]);
                productTypesModel.StandardAttributeModels = this.GenerateProductTypeAttributes(queryResult, 0); //规格属性
                productTypesModel.UseExtendAttributeModels = this.GenerateProductTypeAttributes(queryResult, 1); //扩展属性
                productTypesModel.UseParamAttributeModels = this.GenerateProductTypeAttributes(queryResult, 2); //详细属性
            }

            var result = new ResultModel
            {
                Data = productTypesModel,
                IsValid = true
            };

            return result;
        }

        /// <summary>
        ///     分页获取商品类型列表
        /// </summary>
        /// <param name="model">规格商品类型模型</param>
        /// <returns>规格商品类型数据</returns>
        public ResultModel GetPagingSKU_ProductTypes(SearchSKU_ProductTypesModel model)
        {
            var whereExpr = this._database.Db.SKU_ProductTypes.Name.Like("%" + model.Name + "%");

            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<SKU_ProductTypesModel>(
                        this._database.Db.SKU_ProductTypes.FindAll(whereExpr).OrderByUpdateDTDescending(),
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        ///     添加商品类型
        /// </summary>
        /// <param name="model">需要添加的商品类型</param>
        /// <returns>操作结果</returns>
        public ResultModel AddSKU_ProductTypes(SKU_ProductTypesModel model)
        {
            var result = new ResultModel();
            var isSuccess = false;
            var message = "Failed to add product type!";//添加商品类型失败！

            SKU_ProductTypesModel existProductModel = this._database.Db.SKU_ProductTypes.FindBy(Name: model.Name);

            //判断是否已存在相同名称的商品类型
            if (existProductModel != null && existProductModel.SkuTypeId > 0)
            {
                isSuccess = false;
                message = "Type of commodity that already exists for the same name, and is not allowed to add!";//已存在相同名称的商品类型,不允许新增！
            }
            else
            {
                //开启事务
                var trans = this._database.Db.BeginTransaction();
                try
                {
                    //构建商品类型记录
                    dynamic productTypeRecord = this.BuildProductTypeRecord(model);
                    productTypeRecord.CreateBy = model.CreateBy;
                    productTypeRecord.CreateDT = model.CreateDT;

                    var tempProductModel = trans.SKU_ProductTypes.Insert(productTypeRecord);

                    if (tempProductModel != null && tempProductModel.SkuTypeId > 0)
                    {
                        model.SkuTypeId = tempProductModel.SkuTypeId;
                        result = this.SaveStandardProductTypeAttributes(model.StandardAttributeModels, model, trans);

                        if (!result.IsValid)
                        {
                            trans.Rollback();
                            return result;
                        }

                        //启用扩展属性
                        if (model.UseExtend > 0)
                        {
                            result = this.SaveProductTypeAttributes(model.UseExtendAttributeModels, model, 1, trans);

                            if (!result.IsValid)
                            {
                                trans.Rollback();
                                return result;
                            }
                        }

                        //启用详细参数
                        if (model.UseParameter > 0)
                        {
                            result = this.SaveProductTypeAttributes(model.UseParamAttributeModels, model, 2, trans);
                            if (!result.IsValid)
                            {
                                trans.Rollback();
                                return result;
                            }
                        }


                        trans.Commit();
                        message = "The success of adding product types！";//添加商品类型成功
                        isSuccess = true;
                    }
                    else
                    {
                        trans.Rollback();
                        message = "Failed to add product type！";//添加商品类型失败
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    message = "Failed to add product type！";//
                    //todo 日志记录
                    throw ex;
                }
            }
            result.IsValid = isSuccess;
            result.Messages.Add(message);
            return result;
        }

        /// <summary>
        ///     修改商品类型
        /// </summary>
        /// <param name="model">需要修改的商品类型</param>
        /// <returns>操作结果</returns>
        public ResultModel UpdateSKU_ProductTypes(SKU_ProductTypesModel model)
        {
            var result = new ResultModel();
            var isSuccess = false;
            var message = "Failed to modify the commodity type！";//修改商品类型失败

            SKU_ProductTypesModel existProductModel =
                this._database.Db.SKU_ProductTypes.Find(this._database.Db.SKU_ProductTypes.Name == model.Name &&
                                                        (this._database.Db.SKU_ProductTypes.SkuTypeId != model.SkuTypeId));


            //判断是否已存在相同名称的规格项
            if (existProductModel != null && existProductModel.SkuTypeId > 0)
            {
                isSuccess = false;
                message = "The same name of the commodity type, which is not allowed to change!";//已存在相同名称的商品类型,不允许修改！
            }
            else
            {
                //开启事务
                var trans = this._database.Db.BeginTransaction();
                try
                {
                    model.UpdateDT = DateTime.Now;
                    //构建商品类型记录
                    dynamic productTypeRecord = this.BuildProductTypeRecord(model);

                    var row = trans.SKU_ProductTypes.UpdateBySkuTypeId(productTypeRecord);

                    if (row > 0)
                    {
                        result = this.SaveStandardProductTypeAttributes(model.StandardAttributeModels, model, trans);

                        if (!result.IsValid)
                        {
                            trans.Rollback();
                            return result;
                        }

                        //启用扩展属性
                        if (model.UseExtend > 0)
                        {
                            result = this.SaveProductTypeAttributes(model.UseExtendAttributeModels, model, 1, trans);

                            if (!result.IsValid)
                            {
                                trans.Rollback();
                                return result;
                            }
                        }

                        //启用详细参数
                        if (model.UseParameter > 0)
                        {
                            result = this.SaveProductTypeAttributes(model.UseParamAttributeModels, model, 2, trans);

                            if (!result.IsValid)
                            {
                                trans.Rollback();
                                return result;
                            }
                        }

                        trans.Commit();
                        message = "Modify commodity type Success";//修改商品类型成功！
                        isSuccess = true;
                    }
                    else
                    {
                        trans.Rollback();
                        message = "Failed to modify the commodity type!";//修改商品类型失败！
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    message = "Failed to modify the commodity type！";//修改商品类型失败
                    //todo 日志记录
                    throw ex;
                }
            }
            result.IsValid = isSuccess;
            result.Messages.Add(message);
            return result;
        }

        #region private Method

        /// <summary>
        ///     保存规格属性
        /// </summary>
        /// <param name="models">商品规格属性集合</param>
        /// <param name="productTypesModel">商品类型对象</param>
        /// <param name="trans">事务对象</param>
        /// <returns>操作结果</returns>
        private ResultModel SaveStandardProductTypeAttributes(List<SKU_ProductTypeAttributeModel> models,
            SKU_ProductTypesModel productTypesModel, dynamic trans)
        {
            var result = new ResultModel();


            //需要添加的规格值
            var addAttrs = models.Where(x => x.RowStatus == 0 && x.SKU_ProductTypeAttributeId == 0).ToList();

            if (addAttrs.Count > 0)
            {
                addAttrs.ForEach(x =>
                {
                    var tempAttr = x;

                    tempAttr.SkuTypeId = productTypesModel.SkuTypeId;
                    tempAttr.CreateBy = productTypesModel.CreateBy;
                    tempAttr.CreateDT = DateTime.Now;
                    tempAttr.UpdateBy = x.CreateBy;
                    tempAttr.UpdateDT = x.CreateDT;
                    tempAttr.AttributeId = x.AttributeId;

                    //构建添加记录
                    var tempRecord = this.BuildAddProductTypeAttrRecord(tempAttr);
                    tempRecord.CreateBy = tempAttr.CreateBy;
                    tempRecord.CreateDT = tempAttr.CreateDT;
                    tempRecord.SKU_ProductTypeAttributeId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");

                    trans.SKU_ProductTypeAttribute.Insert(tempRecord);
                });
            }

            //需要更新的规格值
            var updateAttrs = models.Where(x => x.RowStatus == 0 && x.SKU_ProductTypeAttributeId != 0).ToList();
            if (updateAttrs.Count > 0)
            {
                updateAttrs.ForEach(x =>
                {
                    var tempAttr = x;
                    tempAttr.UpdateBy = productTypesModel.CreateBy;
                    tempAttr.UpdateDT = DateTime.Now;
                    tempAttr.SkuTypeId = productTypesModel.SkuTypeId;

                    var tempRecord = this.BuildAddProductTypeAttrRecord(tempAttr);
                    trans.SKU_ProductTypeAttribute.UpdateBySKU_ProductTypeAttributeId(tempRecord);
                });
            }

            //如果该商品属性还为使用则可以删除规格属性
            if (productTypesModel.IsUse == 0)
            {
                //需要删除的规格属性
                var deleteAttrs =
                    models.Where(x => x.RowStatus == -1 && x.SKU_ProductTypeAttributeId != 0)
                        .ToList();
                if (deleteAttrs.Count > 0)
                {
                    deleteAttrs.ForEach(
                        x =>
                            trans.SKU_ProductTypeAttribute.DeleteBySKU_ProductTypeAttributeId(
                                x.SKU_ProductTypeAttributeId));
                }
            }
            result.IsValid = true;
            return result;
        }

        /// <summary>
        ///     保存商品类型属性
        /// </summary>
        /// <param name="models">需要保存的商品类型属性集合</param>
        /// <param name="productTypesModel">商品类型对象</param>
        /// <param name="attributeType">商品属性类型</param>
        /// <param name="trans">事务对象</param>
        /// <returns>操作是否成功</returns>
        private ResultModel SaveProductTypeAttributes(List<SKU_ProductTypeAttributeModel> models,
            SKU_ProductTypesModel productTypesModel, int attributeType, dynamic trans)
        {
            var errorCount = 0;

            var result = new ResultModel();

            //需要添加的扩展属性
            var addAttrs = models.Where(x => x.RowStatus == 0 && x.SKU_ProductTypeAttributeId == 0).ToList();

            if (addAttrs.Count > 0)
            {
                addAttrs.ForEach(x =>
                {
                    if (x.SKU_AttributesModel != null)
                    {
                        //商品类型属性对象
                        var tempAttr = x;

                        tempAttr.SkuTypeId = productTypesModel.SkuTypeId;
                        tempAttr.CreateBy = productTypesModel.CreateBy;
                        tempAttr.CreateDT = DateTime.Now;
                        tempAttr.UpdateBy = x.CreateBy;
                        tempAttr.UpdateDT = x.CreateDT;
                        tempAttr.AttributeType = attributeType;

                        //属性名对象
                        var attrModel = x.SKU_AttributesModel;
                        attrModel.IsSKU = 0;
                        attrModel.CreateBy = tempAttr.CreateBy;
                        attrModel.CreateDT = tempAttr.CreateDT;
                        attrModel.UpdateBy = tempAttr.CreateBy;
                        attrModel.UpdateDT = tempAttr.CreateDT;


                        result = this._SKU_AttributesService.AddSKU_Attributes(attrModel, trans);
                        if (!result.IsValid || result.Data == 0)
                        {
                            errorCount++;
                            return;
                        }
                        tempAttr.AttributeId = result.Data;

                        //构建添加记录
                        var tempRecord = this.BuildAddProductTypeAttrRecord(tempAttr);
                        tempRecord.CreateBy = tempAttr.CreateBy;
                        tempRecord.CreateDT = tempAttr.CreateDT;

                        tempRecord.SKU_ProductTypeAttributeId = MemCacheFactory.GetCurrentMemCache().Increment("commonId");

                        trans.SKU_ProductTypeAttribute.Insert(tempRecord);
                    }
                });
            }

            //需要更新的规格值
            var updateAttrs = models.Where(x => x.RowStatus == 0 && x.SKU_ProductTypeAttributeId != 0).ToList();
            if (updateAttrs.Count > 0)
            {
                updateAttrs.ForEach(x =>
                {
                    var tempAttr = x;
                    tempAttr.UpdateBy = productTypesModel.CreateBy;
                    tempAttr.UpdateDT = DateTime.Now;
                    tempAttr.AttributeId = x.SKU_AttributesModel.AttributeId;
                    tempAttr.SkuTypeId = productTypesModel.SkuTypeId;
                    tempAttr.AttributeType = attributeType;

                    //属性名对象
                    var attrModel = x.SKU_AttributesModel;
                    attrModel.IsSKU = 0;
                    attrModel.UpdateBy = tempAttr.CreateBy;
                    attrModel.UpdateDT = tempAttr.CreateDT;


                    result = this._SKU_AttributesService.UpdateSKU_Attributes(attrModel, trans);
                    if (!result.IsValid)
                    {
                        errorCount++;
                        return;
                    }
                    var tempRecord = this.BuildAddProductTypeAttrRecord(tempAttr);

                    int rows = trans.SKU_ProductTypeAttribute.UpdateBySKU_ProductTypeAttributeId(tempRecord);
                    if (rows <= 0)
                    {
                        errorCount++;
                    }
                });
            }

            //如果该商品类型属性还未使用则可以删除商品类型属性
            if (productTypesModel.IsUse == 0)
            {
                //需要删除的规格属性
                var deleteAttrs =
                    models.Where(x => x.RowStatus == -1 && x.SKU_ProductTypeAttributeId != 0)
                        .ToList();
                if (deleteAttrs.Count > 0)
                {
                    deleteAttrs.ForEach(x =>
                    {
                        int rows =
                            trans.SKU_ProductTypeAttribute.DeleteBySKU_ProductTypeAttributeId(
                                x.SKU_ProductTypeAttributeId);
                        if (rows <= 0)
                        {
                            errorCount++;
                            return;
                        }

                        result = this._SKU_AttributesService.Delete_SKU_Attributes(x.SKU_AttributesModel, trans);

                        if (!result.IsValid)
                        {
                            errorCount++;
                        }
                    });
                }
            }

            result.IsValid = errorCount == 0;

            return result;
        }

        /// <summary>
        ///     构建商品类型属性记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private dynamic BuildAddProductTypeAttrRecord(SKU_ProductTypeAttributeModel model)
        {
            dynamic record = new SimpleRecord();
            record.SKU_ProductTypeAttributeId = model.SKU_ProductTypeAttributeId;
            record.AttributeId = model.AttributeId;
            record.SkuTypeId = model.SkuTypeId;
            record.AttributeType = model.AttributeType;
            record.AttributeGroup = model.AttributeGroup;
            record.DisplaySequence = model.DisplaySequence;
            record.UpdateBy = model.UpdateBy;
            record.UpdateDT = model.UpdateDT;
            return record;
        }

        /// <summary>
        ///     构建商品类型记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private dynamic BuildProductTypeRecord(SKU_ProductTypesModel model)
        {
            dynamic record = new SimpleRecord();
            record.Name = model.Name;
            record.IsGoods = model.IsGoods;
            record.UseExtend = model.UseExtend;
            record.UseParameter = model.UseParameter;
            record.Remark = model.Remark;
            record.IsUse = model.IsUse;
            record.UpdateBy = model.UpdateBy;
            record.UpdateDT = model.UpdateDT;
            record.SkuTypeId = model.SkuTypeId;

            return record;
        }

        /// <summary>
        ///     构建商品类型
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        private SKU_ProductTypesModel GenerateProductTypes(dynamic record)
        {
            var tempModel = new SKU_ProductTypesModel
            {
                SkuTypeId = record.SkuTypeId,
                Name = record.Name,
                IsGoods = record.IsGoods,
                UseExtend = record.UseExtend,
                UseParameter = record.UseParameter,
                Remark = record.Remark,
                IsUse = record.IsUse,
                CreateBy = record.CreateBy,
                CreateDT = record.CreateDT,
                UpdateBy = record.UpdateBy,
                UpdateDT = record.UpdateDT
            };
            return tempModel;
        }

        /// <summary>
        ///     构建商品类型属性
        /// </summary>
        /// <param name="queryResult">查询结果</param>
        /// <param name="attrbuteType">属性类型</param>
        /// <returns></returns>
        private List<SKU_ProductTypeAttributeModel> GenerateProductTypeAttributes(List<dynamic> queryResult,
            int attrbuteType)
        {
            var productTypeAttributeModels = new List<SKU_ProductTypeAttributeModel>();


            var pTypeIds = new List<long>();

            //0:SKU属性,1:扩展属性,2:参数表

            if (queryResult != null && queryResult != null)
            {
                SKU_ProductTypeAttributeModel tempModel = null;

                foreach (var record in queryResult)
                {
                    if (record.b_AttributeType == attrbuteType)
                    {
                        //不存在该记录
                        if (!pTypeIds.Contains(record.b_SKU_ProductTypeAttributeId))
                        {
                            tempModel = new SKU_ProductTypeAttributeModel
                            {
                                SKU_ProductTypeAttributeId = record.b_SKU_ProductTypeAttributeId,
                                AttributeId = record.b_AttributeId,
                                SkuTypeId = record.b_SkuTypeId,
                                AttributeType = record.b_AttributeType,
                                AttributeGroup = record.b_AttributeGroup,
                                DisplaySequence = record.b_DisplaySequence,
                                CreateBy = record.b_CreateBy,
                                CreateDT = record.b_CreateDT,
                                UpdateBy = record.b_UpdateBy,
                                UpdateDT = record.b_UpdateDT
                            };

                            //属性名
                            SKU_AttributesModel attributeModel = this.GenerateAttributesReocrd(record);
                            tempModel.SKU_AttributesModel = attributeModel;

                            if (record.d_ValueId != null)
                            {
                                //添加属性值
                                attributeModel.SKU_AttributeValuesModels.Add(this.GenerateAttributesValueReocrd(record));
                            }
                            pTypeIds.Add(record.b_SKU_ProductTypeAttributeId);

                            productTypeAttributeModels.Add(tempModel);

                        }
                        else
                        {
                            tempModel =
                                productTypeAttributeModels.FirstOrDefault(
                                    x => x.SKU_ProductTypeAttributeId == record.b_SKU_ProductTypeAttributeId);
                            if (record.d_ValueId != null)
                            {
                                tempModel.SKU_AttributesModel.SKU_AttributeValuesModels.Add(
                                    this.GenerateAttributesValueReocrd(record));
                            }
                        }
                    }
                }
            }

            productTypeAttributeModels.ForEach(x =>
            {
                x.SKU_AttributesModel.SKU_AttributeValuesModels =
                    x.SKU_AttributesModel.SKU_AttributeValuesModels.OrderBy(y => y.DisplaySequence).ToList();
                x.SKU_AttributesModel.ValuesString =
                    string.Join(",", x.SKU_AttributesModel.SKU_AttributeValuesModels.Select(y => y.ValueStr));
            });

            productTypeAttributeModels = productTypeAttributeModels.OrderBy(x => x.DisplaySequence).ToList();

            return productTypeAttributeModels;
        }

        /// <summary>
        ///     构建属性模型
        /// </summary>
        /// <param name="record">数据记录</param>
        /// <returns></returns>
        private SKU_AttributesModel GenerateAttributesReocrd(dynamic record)
        {
            var tempModel = new SKU_AttributesModel
            {
                AttributeId = record.c_AttributeId,
                AttributeName = record.c_AttributeName,
                AttributeType = record.c_AttributeType,
                IsSKU = record.c_IsSKU,
                UsageMode = record.c_UsageMode,
                IsSearch = record.c_IsSearch,
                Remark = record.c_Remark,
                CreateBy = record.c_CreateBy,
                CreateDT = record.c_CreateDT,
                UpdateBy = record.c_UpdateBy,
                UpdateDT = record.c_UpdateDT
            };
            return tempModel;
        }

        /// <summary>
        ///     构建属性值模型
        /// </summary>
        /// <param name="record">数据记录</param>
        private SKU_AttributeValuesModel GenerateAttributesValueReocrd(dynamic record)
        {
            var tempModel = new SKU_AttributeValuesModel
            {
                ValueId = record.d_ValueId,
                AttributeId = record.d_AttributeId,
                DisplaySequence = record.d_DisplaySequence,
                ValueStr = record.d_ValueStr,
                ImageUrl = record.d_ImageUrl,
                ValuesGroup = record.d_ValuesGroup
            };
            return tempModel;
        }

        #endregion


        public ResultModel GetSku_ProductTypesByCategoryId(int id)
        {
            var result = new ResultModel();

            result.Data = this._database.RunQuery(db =>
            {
                dynamic ct, sptattr, sattr, saval;

                IList<SKU_ProductTypesModel> data = db.SKU_ProductTypes
                    .Query()
                    .Join(db.CategoryType, out ct)
                    .On(ct.SkuTypeId == db.SKU_ProductTypes.SkuTypeId)
                    .LeftJoin(db.SKU_ProductTypeAttribute.As("SKU_ProductTypeAttributeModel"), out sptattr)
                    .On(sptattr.SkuTypeId == db.SKU_ProductTypes.SkuTypeId)
                    .LeftJoin(db.SKU_Attributes.As("SKU_AttributesModels"), out sattr)
                    .On(sattr.AttributeId == sptattr.AttributeId)
                    .LeftJoin(db.SKU_AttributeValues.As("SKU_AttributeValuesModels"), out saval)
                    .On(saval.AttributeId == sattr.AttributeId)
                    .WithMany(sptattr)
                    .WithMany(sattr)
                    .WithMany(saval)
                    .Where(ct.CategoryId == id)
                    .ToList<SKU_ProductTypesModel>()
                    ;

                foreach (var model in data)
                {
                    IList<SKU_AttributesModel> salist = model.SKU_AttributesModels;
                    IList<SKU_AttributeValuesModel> sav = model.SKU_AttributeValuesModels;

                    foreach (var skuProductTypeAttributeModel in model.SKU_ProductTypeAttributeModel)
                    {
                        skuProductTypeAttributeModel.SKU_AttributesModel = salist
                            .SingleOrDefault(m => m.AttributeId == skuProductTypeAttributeModel.AttributeId);

                        if (skuProductTypeAttributeModel.SKU_AttributesModel != null)
                        {
                            skuProductTypeAttributeModel.SKU_AttributesModel.SKU_AttributeValuesModels =
                                sav.Where(m => m.AttributeId == skuProductTypeAttributeModel.AttributeId).ToList();
                        }
                    }
                }

                return data;
            });

            return result;
        }
    }
}