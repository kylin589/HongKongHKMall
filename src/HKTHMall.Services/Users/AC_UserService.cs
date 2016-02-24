using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.User;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.User;
using Simple.Data;
using System.Collections.Generic;

namespace HKTHMall.Services.Users
{
    /// <summary>
    /// 用户服务类
    /// zhoub 20150707
    /// </summary>
    public class AC_UserService : BaseService, IAC_UserService
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="model">用户模型</param>
        public ResultModel AddAC_User(AC_UserModel model)
        {
            ResultModel result = new ResultModel(); 
            var count=_database.Db.AC_User.GetCount(_database.Db.AC_User.UserName==model.UserName);
            if (count > 0)
            {
                result.IsValid = false;
                result.Messages = new List<string> { "User name already exists." };//用户名已经存在.
            }
            else
            {
                result.Data = _database.Db.AC_User.Insert(model);
                result.Messages = new List<string> { "Add user success" };//添加用户成功
            }
            return result;
        }

        /// <summary>
        /// 根据用户id获取用户
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns>用户模型</returns>
        public ResultModel GetAC_UserById(long id)
        {
            var result = new ResultModel
            {
                Data = _database.Db.AC_User.FindByUserID(id)
            };
            return result;
        }


        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="model">用户模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateAC_User(AC_UserModel model)
        {

            var result = new ResultModel();
            _database.Db.AC_User.UpdateByUserID(UserID: model.UserID, RealName: model.RealName, IDNumber: model.IDNumber, UserMode: model.UserMode, ID: model.ID, RoleID: model.RoleID, UpdateUser: model.UpdateUser, UpdateDt: model.UpdateDt,Sex:model.Sex);
            return result;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户Id</param>
        /// <returns>是否删除成功</returns>
        public ResultModel DeleteAC_UserById(long id)
        {
            var result = new ResultModel();
            _database.Db.AC_User.DeleteByUserID(id);
            return result;
        }

        /// <summary>
        /// 分页获取用户列表
        /// </summary>
        /// <param name="model">用户搜索模型</param>
        /// <returns>用户列表数据</returns>
        public ResultModel GetPagingAC_Users(SearchUsersModel model)
        {
            var tb = _database.Db.AC_User;
            var whereExpr = tb.UserName.Like("%" + (model.UserName != null ? model.UserName.Trim() : model.UserName) + "%");

            if (model.RealName != null)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.RealName.Like("%" + model.RealName.Trim() + "%"), SimpleExpressionType.And);
            }

            if (model.UserMode.ToString() != "0")
            {
                whereExpr = new SimpleExpression(whereExpr, tb.UserMode == model.UserMode, SimpleExpressionType.And);
            }

            if (model.ID.ToString() != "0")
            {
                whereExpr = new SimpleExpression(whereExpr, tb.ID == model.ID, SimpleExpressionType.And);
            }

            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<AC_UserModel>(_database.Db.AC_User.All().LeftJoin(_database.Db.AC_Department).On(_database.Db.AC_User.ID == _database.Db.AC_Department.ID).LeftJoin(_database.Db.AC_Role).On(_database.Db.AC_User.RoleID == _database.Db.AC_Role.RoleID).Select(_database.Db.AC_User.UserID, _database.Db.AC_User.UserName, _database.Db.AC_User.RealName, _database.Db.AC_User.CreateDT, _database.Db.AC_User.UserMode, _database.Db.AC_Department.DeptName, _database.Db.AC_Role.RoleName).Where(whereExpr),
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }


        public ResultModel SearchUsers(SearchUsersModel model)
        {
            var tb = _database.Db.AC_User;
            var whereExpr = tb.UserName.Like("%" + model.UserName + "%");

            if (model.RealName != null)
            {
                whereExpr = new SimpleExpression(whereExpr, tb.UserName.Like("%" + model.RealName + "%"), SimpleExpressionType.And);
            }

            if (model.UserMode.ToString() != "0")
            {
                whereExpr = new SimpleExpression(whereExpr, tb.UserMode == model.UserMode, SimpleExpressionType.And);
            }

            if (model.ID.ToString() != "0")
            {
                whereExpr = new SimpleExpression(whereExpr, tb.ID == model.ID, SimpleExpressionType.And);
            }

            var result = new ResultModel
            {
                Data =
                    new SimpleDataPagedList<AC_UserModel>(_database.Db.AC_User.FindAll(whereExpr),
                        model.PagedIndex, model.PagedSize)
            };

            return result;
        }

        /// <summary>
        /// 根据用户名查询用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户列表数据</returns>
        public ResultModel GetAC_UserByUserName(string userName)
        {
            var result = new ResultModel
            {
                Data = _database.Db.AC_User.Find(_database.Db.AC_User.UserName == userName)

            };
            return result;
        }

        /// <summary>
        /// 根据用户名查询用户信息和部门信息(吴育富)
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>用户列表数据</returns>
        public ResultModel GetAC_UserDepartmentByUserName(SearchUsersModel model)
        {
            var tb = _database.Db.YH_User;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal); //

            if (!string.IsNullOrEmpty(model.UserName) && model.UserName.Trim() != "")
            {
                //用户真实姓名
                where = new SimpleExpression(where,
                   tb.UserName.Like("%" + model.UserName + "%"), SimpleExpressionType.And);
            }
            dynamic pc;
           

            var query = tb
                .Query()
                
                .LeftJoin(_database.Db.AC_Department, out pc)
                .On(_database.Db.YH_User.ID == _database.Db.AC_Department.ID)
                .Select(
                    tb.UserID,
                    tb.RoleID,
                    tb.ID,
                    tb.UserName,
                    tb.RealName,
                    tb.Password,
                    tb.Sex,
                    tb.IDNumber,
                    tb.UserMode,


                    pc.DeptName,
                    pc.ParentID,
                    pc.IsActive
                )
                .Where(where)
                .OrderByUserIDDescending();

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<AC_UserDepartment>(query,
                    model.PagedIndex, model.PagedSize)
            };
            
            return result;
        }


        /// <summary>
        /// 用户密码重置
        /// zhoub 20150707
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResultModel ReSetAC_UserPassword(long userId, string password)
        {
            var result = new ResultModel();
            _database.Db.AC_User.UpdateByUserID(UserID: userId, Password: password);
            return result;
        }

    }
}
