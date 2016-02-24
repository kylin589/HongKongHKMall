using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.SKU;
using HKTHMall.Domain.Entities;
using HKTHMall.Domain.Models;
using Simple.Data;
using HKTHMall.Services.Common.MultiLangKeys;


namespace HKTHMall.Services.SKU.Impl
{
    /// <summary>
    ///     规格服务类
    /// </summary>
    public class SKU_AttributesService : BaseService, ISKU_AttributesService
    {
        /// <summary>
        ///     添加规格
        /// </summary>
        /// <param name="model">规格模型</param>
        public ResultModel AddStandardSKU_Attributes(SKU_AttributesModel model)
        {
            var result = new ResultModel();


            SKU_AttributesModel existAttributes = _database.Db.SKU_Attributes.FindBy(
                AttributeName: model.AttributeName, IsSKU: 1);

            //判断是否已存在相同名称的规格项
            if (existAttributes != null && existAttributes.AttributeId > 0)
            {
                result.IsValid = false;
                result.Messages.Add("The same name of the commodity specifications, not allowed to add!");//已存在相同名称的商品规格,不允许新增！
            }
            else
            {
                //开启事务
                var trans = _database.Db.BeginTransaction();
                try
                {
                    result = this.AddSKU_Attributes(model, trans);

                    if (result.IsValid)
                    {
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback();

                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    //todo 日志记录
                    throw ex;
                }
                result.Messages.Add(result.IsValid ? "The success of adding product specifications！" : "Failed to add product specifications！");//添加商品规格成功--添加商品规格失败
            }

            return result;
        }


        /// <summary>
        ///  添加商品属性
        /// </summary>
        /// <param name="model">属性模型</param>
        /// <param name="trans">事务对象</param>
        /// <returns>结果模型</returns>
        internal ResultModel AddSKU_Attributes(SKU_AttributesModel model, dynamic trans)
        {
            var isSuccess = false;
            long id = 0;
            //构建记录
            dynamic attributesRecord = BuildAttributesRecord(model);
            attributesRecord.CreateBy = model.CreateBy;
            attributesRecord.CreateDT = model.CreateDT;

            var tempAttributes = trans.SKU_Attributes.Insert(attributesRecord);
            if (tempAttributes != null && tempAttributes.AttributeId > 0)
            {
                id = tempAttributes.AttributeId;
                var insertValues =
                    model.SKU_AttributeValuesModels.Where(x => x.RowStatus == 0 && x.ValueId == 0).ToList();
                this.AddAttributeItemValue(insertValues, tempAttributes.AttributeId, trans);
                isSuccess = true;
            }
            var result = new ResultModel { IsValid = isSuccess, Data = id };
            return result;
        }


        /// <summary>
        ///     根据规格id获取规格
        /// </summary>
        /// <param name="id">规格id</param>
        /// <returns>规格模型</returns>
        public ResultModel GetSKU_AttributesById(long id)
        {
            SKU_AttributesModel model = _database.Db.SKU_Attributes.FindBy(AttributeId: id);
            if (model != null)
            {
                model.SKU_AttributeValuesModels = _database.Db.SKU_AttributeValues.FindAllBy(AttributeId: id);
                model.SKU_AttributeValuesModels =
                    model.SKU_AttributeValuesModels.OrderBy(x => x.DisplaySequence).ToList();
            }

            var result = new ResultModel
            {
                Data = model,
                IsValid = true
            };

            return result;
        }

        /// <summary>
        ///     更新规格
        /// </summary>
        /// <param name="model">规格模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateStandardSKU_Attributes(SKU_AttributesModel model)
        {
            bool isSuccess = false;
            string message = "Modify commodity specifications";//修改商品规格成功
            var result = new ResultModel();

            SKU_AttributesModel existAttributes = _database.Db.SKU_Attributes.Find(
                _database.Db.SKU_Attributes.AttributeName == model.AttributeName
                && _database.Db.SKU_Attributes.AttributeId != model.AttributeId
                && _database.Db.SKU_Attributes.IsSKU == 1
                );

            //如果已存在相同名称的商品规格,则不允许修改！
            if (existAttributes != null && existAttributes.AttributeId > 0)
            {
                isSuccess = false;
                message = "Has the same name as the commodity specification, does not allow modification!";//已存在相同名称的商品规格,不允许修改！
            }
            else
            {

                //开启事务
                var trans = _database.Db.BeginTransaction();
                try
                {

                    result = this.UpdateSKU_Attributes(model, trans);
                    if (result.IsValid)
                    {
                        trans.Commit();
                        isSuccess = true;
                    }
                    else
                    {
                        trans.Rollback();
                    }
                }
                catch
                {
                    trans.Rollback();
                    message = "Failed to modify commodity specifications";//修改商品规格失败
                    //todo 日志记录
                }
            }
            result.IsValid = isSuccess;
            result.Messages.Add(message);
            return result;
        }

        /// <summary>
        /// 更新商品属性
        /// </summary>
        /// <param name="model"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal ResultModel UpdateSKU_Attributes(SKU_AttributesModel model, dynamic trans)
        {
            var isSuccess = false;

            SKU_AttributesModel useAttributes =
                _database.Db.SKU_Attributes.FindBy(AttributeId: model.AttributeId);


            var isChanged = useAttributes.AttributeName != model.AttributeName
                            || useAttributes.AttributeType != model.AttributeType
                            || useAttributes.UsageMode != model.UsageMode;

            var updateSuccess = false;

            //如果有变动,就更新规格项
            if (isChanged)
            {
                //构建记录
                dynamic attributesRecord = BuildAttributesRecord(model);

                //更新规格
                updateSuccess = trans.SKU_Attributes.UpdateByAttributeId(attributesRecord) > 0;
            }

            if (!isChanged || updateSuccess)
            {
                //新增规格项值
                var insertValues =
                    model.SKU_AttributeValuesModels.Where(x => x.RowStatus == 0 && x.ValueId == 0).ToList();
                this.AddAttributeItemValue(insertValues, model.AttributeId, trans);

                //更新规格项值
                var updateValues =
                    model.SKU_AttributeValuesModels.Where(x => x.RowStatus == 0 && x.ValueId != 0).ToList();
                this.UpdateAttributeItemValue(updateValues, model.AttributeId, trans);

                //删除规格项值
                var deleteValues =
                    model.SKU_AttributeValuesModels.Where(x => x.RowStatus == -1 && x.ValueId != 0).ToList();
                this.DeleteAttributeItemValue(deleteValues, trans);
                isSuccess = true;
            }
            var result = new ResultModel { IsValid = isSuccess };
            return result;
        }

        /// <summary>
        /// 删除商品属性
        /// </summary>
        /// <param name="model"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        internal ResultModel Delete_SKU_Attributes(SKU_AttributesModel model, dynamic trans)
        {

            var values = model.SKU_AttributeValuesModels.Where(x => x.RowStatus == -1 && x.ValueId > 0).ToList();

            //如果没有需要删除的,则去数据库查询需要删除的属性值
            if (values.Count == 0)
            {
                values = trans.SKU_AttributeValues.FindAllByAttributeId(model.AttributeId);
            }

            if (values.Count > 0)
            {
                foreach (var valueModel in values)
                {
                    trans.SKU_AttributeValues.DeleteByValueId(valueModel.ValueId);
                }
            }

            trans.SKU_Attributes.DeleteByAttributeId(model.AttributeId);

            return new ResultModel() { IsValid = true };
        }


        /// <summary>
        ///     判断规格值是否已经被使用
        /// </summary>
        /// <param name="valueId">规格值Id</param>
        /// <returns>是否已经存在</returns>
        public ResultModel CheckValueIsUsed(int valueId)
        {
            var count = _database.Db.SKU_ProductAttributes.FindAllByValueId(valueId).ToList().Count;
            var result = new ResultModel
            {
                IsValid = count > 0
            };
            return result;
        }

        /// <summary>
        ///     删除规格
        /// </summary>
        /// <param name="id">规格Id</param>
        /// <returns>是否删除成功</returns>
        public ResultModel DeleteSKU_AttributesById(long id)
        {
            var result = new ResultModel();
            _database.Db.SKU_Attributes.UpdateByAttributeId(id);
            return result;
        }

        /// <summary>
        ///     分页获取规格列表
        /// </summary>
        /// <param name="model">规格搜索模型</param>
        /// <returns>规格列表数据</returns>
        public ResultModel GetPagingSKU_Attributess(SearchSKU_AttributesModel model)
        {

            var whereExpr = _database.Db.SKU_Attributes.AttributeName.Like("%" + model.AttributeName + "%");
            if (model.IsSKU.HasValue)
            {
                whereExpr = new SimpleExpression(whereExpr, new SimpleExpression(_database.Db.SKU_Attributes.IsSKU, model.IsSKU.Value, SimpleExpressionType.Equal), SimpleExpressionType.And);
            }

            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<SKU_AttributesModel>(_database.Db.SKU_Attributes.FindAll(whereExpr).OrderByUpdateDTDescending(),
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 获取所有属性名称列表
        /// </summary>
        /// <param name="isSKU">是否SKU规格</param>
        /// <returns>属性列表</returns>
        public ResultModel GetAllSKU_AttributesBy(bool? isSKU)
        {
            var result = new ResultModel();

            var whereExpr = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            if (isSKU.HasValue)
            {
                whereExpr = new SimpleExpression(whereExpr, new SimpleExpression(_database.Db.SKU_Attributes.IsSKU, isSKU.Value, SimpleExpressionType.Equal), SimpleExpressionType.And);
            }

            result.Data = _database
                .Db.SKU_Attributes.FindAll(whereExpr)
                    .OrderBy(_database.Db.SKU_Attributes.UpdateDT, OrderByDirection.Descending)
                    .ToList<SKU_AttributesModel>();
            result.IsValid = true;
            return result;

        }

        /// <summary>
        /// 根据属性名Id获取属性值列表
        /// </summary>
        /// <param name="id">属性名Id</param>
        /// <returns>属性值列表</returns>
        public ResultModel GetAttributeValuesById(long id)
        {
            var result = new ResultModel();
            result.Data =
                _database.Db.SKU_AttributeValues.FindAllBy(AttributeId: id)
                    .OrderBy(_database.Db.SKU_AttributeValues.DisplaySequence, OrderByDirection.Ascending)
                    .ToList<SKU_AttributeValuesModel>();
            result.IsValid = true;
            return result;
        }

        /// <summary>
        ///     构建规格项 动态记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private dynamic BuildAttributesRecord(SKU_AttributesModel model)
        {
            dynamic attributesRecord = new SimpleRecord();
            attributesRecord.AttributeId = model.AttributeId;
            attributesRecord.AttributeName = model.AttributeName;
            attributesRecord.AttributeType = model.AttributeType;
            attributesRecord.IsSKU = model.IsSKU;
            attributesRecord.UsageMode = model.UsageMode;
            attributesRecord.IsSearch = model.IsSearch;
            attributesRecord.Remark = model.Remark;
            attributesRecord.UpdateBy = model.UpdateBy;
            attributesRecord.UpdateDT = model.UpdateDT;
            return attributesRecord;
        }

        #region SKU_AttributeValues

        /// <summary>
        ///     添加规格项值
        /// </summary>
        /// <param name="values">规格项值集合</param>
        /// <param name="attributeId">规格项Id</param>
        /// <param name="transDb">事务对象</param>
        /// <returns>是否成功</returns>
        private bool AddAttributeItemValue(List<SKU_AttributeValuesModel> values, long attributeId, dynamic transDb)
        {
            if (values != null & values.Count > 0)
            {
                foreach (var valueEntity in values)
                {
                    var tempValueEntity = BuildAttributeItemValueReocrd(valueEntity);
                    tempValueEntity.AttributeId = attributeId;
                    transDb.SKU_AttributeValues.Insert(tempValueEntity);
                }
            }
            return true;
        }

        /// <summary>
        ///     更新规格项值
        /// </summary>
        /// <param name="values">规格项值集合</param>
        /// <param name="attributeId">规格项Id</param>
        /// <param name="transDb">事务对象</param>
        /// <returns>是否成功</returns>
        private bool UpdateAttributeItemValue(List<SKU_AttributeValuesModel> values, long attributeId, dynamic transDb)
        {
            var isSuccess = false;
            if (values != null && values.Count > 0)
            {
                var valueIds = values.Select(x => x.ValueId).ToArray();
                List<SKU_SKUItems> usedValues = transDb.SKU_SKUItems.FindAllByValueId(valueIds);
                foreach (var value in values)
                {
                    if (usedValues != null && usedValues.Exists(x => x.ValueId == value.ValueId))
                    {
                        var valueRecord = BuildAttributeItemValueReocrd(value);
                        transDb.SKU_AttributeValues.UpdateByValueId(valueRecord);
                    }
                }
                isSuccess = true;
            }
            return isSuccess;
        }

        /// <summary>
        ///     构建规格值 动态记录
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private dynamic BuildAttributeItemValueReocrd(SKU_AttributeValuesModel model)
        {
            dynamic valueRecord = new SimpleRecord();
            valueRecord.ValueId = model.ValueId;
            valueRecord.DisplaySequence = model.DisplaySequence;
            valueRecord.ValueStr = model.ValueStr;
            valueRecord.ImageUrl = model.ImageUrl;
            valueRecord.ValuesGroup = model.ValuesGroup;
            return valueRecord;
        }

        /// <summary>
        ///     删除规则项值
        /// </summary>
        /// <param name="values">规格项值集合</param>
        /// <param name="transDb">事务对象</param>
        /// <returns>是否成功</returns>
        private bool DeleteAttributeItemValue(List<SKU_AttributeValuesModel> values, dynamic transDb)
        {
            var isSuccess = false;
            if (values != null && values.Count > 0)
            {
                var count = transDb.SKU_AttributeValues.DeleteByValueId(values.Select(x => x.ValueId).ToArray());
                isSuccess = count > 0;
            }
            return isSuccess;
        }

        #endregion

        #region 查询商品规格参数

        /// <summary>
        /// 查询商品规格参数(刘文宁)
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ResultModel GetSKU_ProductSpecificationParameterById(long productId, int lang)
        {
            var result = new ResultModel();
            try
            {
                SKU_ProductSpecificationParameterModel lstPspm = new SKU_ProductSpecificationParameterModel();
                var pl = _database.Db.Product_Lang
                       .Query()
                       .Select(
                        _database.Db.Product_Lang.Introduction
                        )
                       .Where(_database.Db.Product_Lang.ProductId== productId&& _database.Db.Product_Lang.LanguageID== lang).ToList();

                if (pl.Count>0)
                {     
                    lstPspm.introduction = pl[0].Introduction;
                }

                var tb = _database.Db.SKU_SKUItems;
                dynamic one; dynamic two;
                lstPspm.attributeArray= tb
                .All()

                .Join(_database.Db.SKU_Attributes, out one)
                .On(one.AttributeId == tb.AttributeId)          
                .Select(
                    tb.AttributeId.Distinct(),
                    one.attributeName,
                    one.attributeType,
                    one.usageMode
                )
                .Where(tb.ProductId == productId).ToList<SKU_AttributeArrayModel>();

                foreach (SKU_AttributeArrayModel attribute in lstPspm.attributeArray)
                {
                    attribute.childAttributeArray = tb
                    .Query()

                    .LeftJoin(_database.Db.SKU_AttributeValues, out two)
                    .On(two.AttributeId == tb.AttributeId && two.ValueId == tb.ValueId)
                    .Select(
                        two.valuestr,
                        two.imageurl,
                        two.ValuesGroup.As("group")
                    ).Where(tb.ProductId == productId && two.AttributeId == attribute.attributeId).ToList<SKU_ChildAttributeArrayModel>();
                }
                result.Data = lstPspm;
                if (result.Data != null)
                {
                    result.IsValid = true;
                    return result;
                }
                else
                {
                    result.Messages.Add(CultureHelper.GetAPPLangSgring("NO_DATA", lang));
                    result.IsValid = true;
                }
                return result;
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Messages.Add(CultureHelper.GetAPPLangSgring("USER_SEARCH_FAILED", lang) + ":" + ex.Message);
            }
            return result;
        }
        #endregion
    }
}