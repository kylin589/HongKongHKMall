using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Core.Infrastructure;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models;
using HKTHMall.Domain.Models.AC;
using Simple.Data;
using HKTHMall.Core;

namespace HKTHMall.Services.AC.Impl
{
    public class AC_RoleService : BaseService, IAC_RoleService
    {

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="model">角色</param>
        public ResultModel Add(AC_RoleModel model)
        {
            var result = new ResultModel
            {
                Data = base._database.Db.AC_Role.Insert(model)
            };
            return result;
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="model">角色模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel Update(AC_RoleModel model)
        {
            var result = new ResultModel
            {
                //base._database.Db.AC_Role.UpdateByRoleID(model);
                Data = base._database.Db.AC_Role.UpdateByRoleID(RoleID: model.RoleID, RoleName: model.RoleName, RoleModuleValue: model.RoleModuleValue, RoleFuctionValue: model.RoleFuctionValue, UpdateUser: model.UpdateUser, UpdateDt: model.UpdateDt, RoleDescription:model.RoleDescription)
            };
            //_db.Users.UpdateById(Id: 1, Name: "Steve", Age: 50);  
            MemCacheFactory.GetCurrentMemCache().ClearCache("ModuleMenuList" + model.RoleID.ToString());
            MemCacheFactory.GetCurrentMemCache().ClearCache("FunctionList" + model.RoleID.ToString());
            return result;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns>是否删除成功</returns>
        public ResultModel Delete(int id)
        {
            var result = new ResultModel();
            base._database.Db.AC_Role.DeleteByRoleID(id);
            // base._database.Db.AC_Role.Delete(RoleID: id);     
            return result;
        }
        /// <summary>
        /// 根据ID取角色信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ResultModel GetAC_RolesById(int id)
        {
            var result = new ResultModel
            {
                Data = base._database.Db.AC_Role.FindByRoleID(id)
            };
            return result;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="model">搜索模型</param>
        /// <returns>列表数据</returns>
        public ResultModel GetPagingList(SearchAC_RoleModel model)
        {
            var whereParam = new SimpleExpression(1, 1, SimpleExpressionType.Equal);
            if (!string.IsNullOrEmpty(model.RoleName))
            {
                whereParam = new SimpleExpression(whereParam, _database.Db.FloorKeyword.KeyWordName.Like("%" + model.RoleName + "%"), SimpleExpressionType.And);
            }
            var result = new ResultModel
            {
                //db.Product.All().OrderByFactoryName();
                //db.Product.All().OrderByFactoryNameDescending();
                //db.Product.All().OrderBy(db.Product.FactoryName);
                //db.Product.All().OrderByDescending(db.Product.FactoryName);
                Data =   new SimpleDataPagedList<AC_RoleModel>(
                            _database.Db.AC_Role.FindAll(whereParam).OrderByDescending(_database.Db.AC_Role.RoleID),
                            model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 获取角色列表
        /// zhoub 20150707
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>角色列表</returns>
        public ResultModel GetAC_RolesBy()
        {
            var result = new ResultModel()
            {
                Data = this._database.Db.AC_Role.All().ToList<AC_RoleModel>()
            };
            return result;
        }

        /// <summary>
        /// 根据角色模块值取菜单
        /// </summary>
        /// <param name="Idstr"></param>
        /// <returns></returns>
        public ResultModel GetAC_ModuleByIDstr(string Idstr)
        {
            int[] output = Array.ConvertAll<string, int>(Idstr.Split(','), delegate(string s) { return int.Parse(s); });
            var result = new ResultModel()
            {
                Data = this._database.Db.AC_Module.FindAll(this._database.Db.AC_Module.ModuleID == output).ToList<AC_ModuleModel>()
                // IEnumerable<User> users = db.Users.FindAllByName(new[] { "Bob", "UnknownUser" }).Cast<User>();
            };
            return result;
        }
        /// <summary>
        /// 根据角色功能值取功能点权限
        /// </summary>
        /// <param name="Idstr"></param>
        /// <returns></returns>
        public ResultModel GetAC_FunctionListByIDstr(string Idstr)
        {
            int[] output = Array.ConvertAll<string, int>(Idstr.Split(','), delegate(string s) { return int.Parse(s); });
            var result = new ResultModel()
            {
                Data = this._database.Db.AC_Function.FindAll(this._database.Db.AC_Function.FunctionID == output).ToList<AC_FunctionModel>()
                // IEnumerable<User> users = db.Users.FindAllByName(new[] { "Bob", "UnknownUser" }).Cast<User>();
            };
            return result;
        }
        /// <summary>
        /// 根据角色ID取功能点权限
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public ResultModel GetFunctionList(int RoleId)
        {
            string key = "FunctionList" + RoleId;
            var funList = MemCacheFactory.GetCurrentMemCache().GetCache<List<AC_FunctionModel>>(key);
            var result = new ResultModel();
            if (funList!=null&&funList.Count > 0)
            {
                result.Data = funList;
            }
            else
            {
                AC_RoleModel model = GetAC_RolesById(RoleId).Data;
                funList = GetAC_FunctionListByIDstr(model.RoleFuctionValue).Data;

                result.Data = funList;
                MemCacheFactory.GetCurrentMemCache().AddCache(key, funList,60);
            }
            return result;
        }

        /// <summary>
        /// 根据角色ID取菜单权限
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public ResultModel GetModuleMenuList(int RoleId)
        {
            string key = "ModuleMenuList" + RoleId;
            var menuList = MemCacheFactory.GetCurrentMemCache().GetCache<List<AC_ModuleModel>>(key);
            var result = new ResultModel();
            if (menuList != null && menuList.Count > 0)
            {
                result.Data = menuList;
            }
            else
            {
                AC_RoleModel model = GetAC_RolesById(RoleId).Data;
                menuList = GetAC_ModuleByIDstr(model.RoleModuleValue).Data;

                result.Data = menuList;
            //    MemCacheFactory.GetCurrentMemCache().AddCache<List<AC_ModuleModel>>(key, menuList, 60);
                MemCacheFactory.GetCurrentMemCache().AddCache(key, menuList, 60);
           
            }
            return result;
        }
    }
}
