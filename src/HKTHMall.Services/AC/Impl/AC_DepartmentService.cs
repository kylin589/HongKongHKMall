using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.AC;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.AC;
using Simple.Data;

namespace HKTHMall.Services.AC.Impl
{
    /// <summary>
    ///     部门服务类
    /// </summary>
    public class AC_DepartmentService : BaseService, IAC_DepartmentService
    {
        /// <summary>
        ///     添加部门
        /// </summary>
        /// <param name="model">部门模型</param>
        public ResultModel AddAC_Department(AC_DepartmentModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.AC_Department.Insert(model)
            };
            return result;
        }

        /// <summary>
        ///     根据部门id获取部门
        /// </summary>
        /// <param name="id">部门id</param>
        /// <returns>部门模型</returns>
        public ResultModel GetAC_DepartmentById(int id)
        {
            var result = new ResultModel
            {
                Data = _database.Db.AC_Department.FindByID(id)
            };
            return result;
        }

        /// <summary>
        ///     获取部门列表
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>部门列表</returns>
        public ResultModel GetAC_DepartmentsBy()
        {
            var result = new ResultModel
            {
                Data = _database.Db.AC_Department.All().ToList<AC_DepartmentModel>()
            };
            return result;
        }

        /// <summary>
        ///     更新部门
        /// </summary>
        /// <param name="model">部门模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateAC_Department(AC_DepartmentModel model)
        {
            var result = new ResultModel();
            dynamic updateRecord = new SimpleRecord();
            updateRecord.ID = model.ID;
            updateRecord.DeptName = model.DeptName;
            updateRecord.IsActive = model.IsActive;
            updateRecord.OrderNumber = model.OrderNumber;
            updateRecord.UpdateBy = model.UpdateBy;
            updateRecord.UpdateDT = model.UpdateDT;
            _database.Db.AC_Department.UpdateByID(updateRecord);
            return result;
        }

        /// <summary>
        ///     删除部门
        /// </summary>
        /// <param name="id">部门Id</param>
        /// <returns>是否删除成功</returns>
        public ResultModel DeleteAC_DepartmentById(int id)
        {
            var result = new ResultModel();
            _database.Db.AC_Department.UpdateByID(id);
            return result;
        }

        /// <summary>
        ///     分页获取部门列表
        /// </summary>
        /// <param name="model">部门搜索模型</param>
        /// <returns>部门列表数据</returns>
        public ResultModel GetPagingAC_Departments(SearchAC_DepartmentModel model)
        {
            var whereExpr = _database.Db.AC_Department.DeptName.Like("%" + model.DeptName + "%");
            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<AC_DepartmentModel>(_database.Db.AC_Department.FindAll(whereExpr).OrderBy(_database.Db.AC_Department.OrderNumber),
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }
    }
}