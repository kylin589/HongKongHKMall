using System;
using System.Collections.Generic;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.Sys;

namespace HKTHMall.Services.Sys
{
    /// <summary>
    /// 系统参数设置对象业务处理类
    /// </summary>
    public class ParameterSetService : BaseService, IParameterSetService
    {
        


        #region 通过Id查询系统参数设置对象
        /// <summary>
        /// 通过Id查询系统参数设置对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        public ResultModel GetParameterSetById(long id)
        {
            var result = new ResultModel() { Data = base._database.Db.ParameterSet.FindByParamenterID(id) };
            return result;
        }
        #endregion

        #region 系统参数对象分布查询
        /// <summary>
        /// 系统参数对象分布查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        public ResultModel Select(SearchParaSetModel model)
        {
            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<ParameterSetModel>(base._database.Db.ParameterSet.FindAll(base._database.Db.ParameterSet.keys.Like("%" + model.KeysName + "%")),
                            model.PagedIndex, model.PagedSize)
            };
            return result;
        }
        #endregion

        #region 添加系统参数设置
        /// <summary>
        /// 添加系统参数设置
        /// </summary>
        /// <param name="model">系统参数设置对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        public ResultModel Add(ParameterSetModel model)
        {
            var result = new ResultModel { Data = _database.Db.ParameterSet.Insert(model) };
            return result;
        }
        #endregion

        #region 通过Id删除系统参数设置
        /// <summary>
        /// 通过Id删除系统参数设置
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        public ResultModel Delete(long id)
        {
            var result = new ResultModel() { Data = base._database.Db.ParameterSet.DeleteByParamenterID(id) };
            return result;
        }
        #endregion

        #region 更新系统参数
        /// <summary>
        /// 更新系统参数
        /// </summary>
        /// <param name="model">系统参数对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        public ResultModel Update(ParameterSetModel model)
        {
            var result = new ResultModel() { Data = this._database.Db.ParameterSet.UpdateByParamenterID(ParamenterID: model.ParamenterID, keys: model.keys, PValue: model.PValue, Remark: model.Remark, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT) };
            return result;
        }
        #endregion

        #region 查询系统参数设置对象列表
        /// <summary>
        /// 查询系统参数设置对象列表
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-2</remarks>
        public ResultModel GetParameterSetList(long? id = null)
        {
            var result = new ResultModel();
            var list = this._database.Db.ParameterSet.All().with(base._database.Db.ParameterSet.ParameterSet_lang).ToList<ParameterSetModel>();
            return result;
        }
        #endregion

        #region 通过键名称查询系统参数设置对象列表
        /// <summary>
        /// 通过键名称查询系统参数设置对象列表
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-2</remarks>
        public ResultModel GetParameterSetListByName(ParameterSetModel model)
        {
            return null;
        }
        #endregion


        /// <summary>
        /// 查询系统参数
        /// </summary>
        /// <param name="key">主键Id</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public ResultModel GetParaSetByKeys(string key)
        {
            var tb = base._database.Db.ParameterSet;
            //  var where = new SimpleExpression(tb.keys == key, tb.PValue ==value, SimpleExpressionType.And);
            var result = new ResultModel()
            {
                Data = tb.FindBykeys(key)
            };
            return result;
        }

        /// <summary>
        /// 根据ID取系统参数的键值
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <remarks>added by liuhongwen,2015-7-23</remarks>
        public ResultModel GetParametePValueById(long id)
        {
            ParameterSetModel parameter = base._database.Db.ParameterSet.FindByParamenterID(id);
            var result = new ResultModel() { Data = parameter.PValue };
            return result;
        }

        /// <summary>
        /// 根据ids集合获取系统参数
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <returns></returns>
        public ResultModel GetParameterSetsBy(long[] ids)
        {
            return this.GetParameterSetsPrivate(ids, this._database.Db);
        }

        /// <summary>
        /// 根据ids集合获取系统参数
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="trans">事务对象</param>
        /// <returns></returns>
        public ResultModel GetParameterSetsBy(long[] ids, dynamic trans)
        {
            return this.GetParameterSetsPrivate(ids, trans);
        }

        /// <summary>
        /// 根据ids集合获取系统参数
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="db">数据Db</param>
        /// <returns></returns>
        private ResultModel GetParameterSetsPrivate(long[] ids, dynamic db)
        {
            if (ids == null)
            {
                throw new Exception("ids不能为空");
            }
            ResultModel resultModel = new ResultModel();
            resultModel.Data = base._database.Db.ParameterSet.FindAll(base._database.Db.ParameterSet.ParamenterID == ids).ToList<ParameterSetModel>();

            return resultModel;
        }

    }
}
