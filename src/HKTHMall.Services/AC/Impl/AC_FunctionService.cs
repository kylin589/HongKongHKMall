using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.AC;
using Simple.Data;

namespace HKTHMall.Services.AC.Impl
{
    public class AC_FunctionService : BaseService, IAC_FunctionService
    {
        /// <summary>
        ///     通过Id查询系统功能对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        public ResultModel GetAC_FunctionById(int id)
        {
            var result = new ResultModel {Data = _database.Db.AC_Function.FindByFunctionID(id)};
            return result;
        }

        /// <summary>
        ///     系统功能分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        public ResultModel Select(SearchAC_FunctionModel model)
        {
            var aC_Function = _database.Db.AC_Function;
            var aC_Module = _database.Db.AC_Module;
            dynamic am;
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            if (!string.IsNullOrEmpty(model.FunctionName))
            {
                whereParam = new SimpleExpression(whereParam, aC_Function.FunctionName.Like("%" + model.FunctionName + "%"), SimpleExpressionType.And);
            }
            if (model.ParentID != 0)
            {
                whereParam = new SimpleExpression(whereParam, aC_Function.ModuleID == model.ParentID || aC_Module.ParentID == model.ParentID, SimpleExpressionType.And);
            }

            var query = aC_Function.All().LeftJoin(aC_Module, out am).On(am.ModuleID == aC_Function.ModuleID).Select(
                aC_Function.FunctionID, aC_Function.FunctionName, aC_Function.Controller, aC_Function.Action, am.ModuleName)
                .Where(whereParam).OrderBy(aC_Function.ModuleID);

            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<AC_FunctionModel>(query,
                        model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        ///     添加系统功能
        /// </summary>
        /// <param name="model">系统功能对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        public ResultModel Add(AC_FunctionModel model)
        {
            var result = new ResultModel {Data = _database.Db.AC_Function.Insert(model)};
            return result;
        }

        /// <summary>
        ///     通过Id删除系统功能
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        public ResultModel Delete(int id)
        {
            var result = new ResultModel {Data = _database.Db.AC_Function.DeleteByFunctionID(id)};
            return result;
        }

        /// <summary>
        ///     更新系统功能
        /// </summary>
        /// <param name="model">系统功能对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-1</remarks>
        public ResultModel Update(AC_FunctionModel model)
        {
            var result = new ResultModel {Data = _database.Db.AC_Function.UpdateByFunctionID(model)};
            return result;
        }

        /// <summary>
        ///     查询功能点列表
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public ResultModel GetAC_FunList()
        {
            var result = new ResultModel
            {
                Data =
                    _database.Db.AC_Function.FindAll(_database.Db.AC_Function.FunctionID > 0).ToList<AC_FunctionModel>()
            };
            return result;
        }
    }
}