using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrCms.Core.Infrastructure;
using HKTHMall.Domain.Models;
using BrCms.Framework.Collections;
using HKTHMall.Domain.Models.AC;
using Simple.Data;

namespace HKTHMall.Services.AC.Impl
{
    public class AC_ModuleService : BaseService, IAC_ModuleService
    {

        /// <summary>
        /// 通过Id查询系统模块对象
        /// </summary>
        /// <param name="id">主键Id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-6</remarks>
        public Domain.Models.ResultModel GetAC_ModuleById(int id)
        {
            var result = new ResultModel() { Data = base._database.Db.AC_Module.FindByModuleID(id) };
            return result;
        }

        /// <summary>
        ///系统模块分页查询
        /// </summary>
        /// <param name="model">输入查询参数对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-6</remarks>
        public Domain.Models.ResultModel Select(Domain.Models.AC.SearchAC_ModuleModel model)
        {
            var result = new ResultModel()
            {
                Data = new SimpleDataPagedList<AC_ModuleModel>(base._database.Db.AC_Module.FindAll(base._database.Db.AC_Module.ParentID == model.ParentID).OrderByPlace(),
                            model.PagedIndex, model.PagedSize)
            };
            return result;
        }

        /// <summary>
        /// 添加系统模块
        /// </summary>
        /// <param name="model">系统模块对象</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-6</remarks>
        public Domain.Models.ResultModel Add(Domain.Models.AC.AC_ModuleModel model)
        {
            var result = new ResultModel();
            var module = this._database.Db.AC_Module.Find(this._database.Db.AC_Module.ModuleName == model.ModuleName);
            if (module == null)
            {
                result.Data = this._database.Db.AC_Module.Insert(model);
            }
            else
            {
                result.Messages = new List<string>() { "Add failed, the menu name already exists" };//添加失败,菜单名称已经存在.
            }
            return result;
        }

        /// <summary>
        /// 通过Id删除系统模块
        /// </summary>
        /// <param name="id">系统参数id</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-6</remarks>
        public Domain.Models.ResultModel Delete(int id)
        {
            var result = new ResultModel();
            var module = this._database.Db.AC_Module.Find(this._database.Db.AC_Module.ParentID == id);
            if (module == null)
            {
                result.Data = base._database.Db.AC_Module.DeleteByModuleID(id);
            }
            else
            {
                result.Messages = new List<string>() { "Delete failed, please remove the menu under the sub menu." };//删除失败,请先移除该菜单下的子菜单.
            }
            return result;
        }

        /// <summary>
        /// 更新系统模块
        /// </summary>
        /// <param name="model">系统模块对象</param>
        /// <returns>返回true时,表示更新成功；反之,表示更新失败</returns>
        /// <remarks>added by jimmy,2015-7-6</remarks>
        public Domain.Models.ResultModel Update(Domain.Models.AC.AC_ModuleModel model)
        {
            var result = new ResultModel();
            var module = this._database.Db.AC_Module.Find(this._database.Db.AC_Module.ModuleName == model.ModuleName && this._database.Db.AC_Module.ModuleID != model.ModuleID);
            if (module == null)
            {
                result.Data = this._database.Db.AC_Module.UpdateByModuleID(ModuleID: model.ModuleID, ModuleName: model.ModuleName, ParentID: model.ParentID, Controller: model.Controller, Action: model.Action, Icon: model.Icon);
            }
            else
            {
                result.Messages = new List<string>() { "Failed to add, the menu name already exists." };//添加失败,菜单名称已经存在.
            }
            return result;
        }

        /// <summary>
        ///查询系统模块列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        public ResultModel GetAC_ModuleList()
        {
            var result = new ResultModel()
            {
                Data = this._database.Db.AC_Module.FindAll(this._database.Db.AC_Module.ModuleID > 0).OrderByPlace().ToList<AC_ModuleModel>()
            };
            return result;
        }
        ///// <summary>
        ///// 根据角色模块值取菜单
        ///// </summary>
        ///// <param name="Idstr"></param>
        ///// <returns></returns>
        //public Result GetAC_ModuleByIDstr(string Idstr)
        //{
        //    int[] output = Array.ConvertAll<string, int>(Idstr.Split(','), delegate(string s) { return int.Parse(s); });
        //    var result = new Result()
        //    {
        //        Data = this._database.Db.AC_Module.FindAll(this._database.Db.AC_Module.ModuleID == output).ToList<AC_ModuleModel>()
        //        // IEnumerable<User> users = db.Users.FindAllByName(new[] { "Bob", "UnknownUser" }).Cast<User>();
        //    };
        //    return result;
        //}
        /// <summary>
        ///查询系统模块列表
        /// </summary>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-7-7</remarks>
        public ResultModel GetAC_ModuleList(int parentId)
        {
            var result = new ResultModel()
            {
                Data = this._database.Db.AC_Module.FindAll(this._database.Db.AC_Module.ParentID == parentId).OrderByPlace().ToList<AC_ModuleModel>()
            };
            return result;
        }


        /// <summary>
        /// 获取菜单数据
        /// zhoub 20150709
        /// </summary>
        /// <returns></returns>
        public ResultModel GetAC_ModuleToTree()
        {
            var data = _database.Db.AC_Module
                .Query().Select(
                    _database.Db.AC_Module.ModuleID.As("id")
                    , _database.Db.AC_Module.ModuleName.As("text")
                    , _database.Db.AC_Module.ParentID
                ).OrderBy(_database.Db.AC_Module.ParentID, _database.Db.AC_Module.Place)
                .ToList();

            return new ResultModel { Data = CreateTree(data, 0) };
        }

        /// <summary>
        ///  递归创建树
        /// </summary>
        /// <param name="categories"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        private dynamic CreateTree(List<dynamic> ac_Module, int parentId)
        {
            var list = ac_Module.FindAll(m => m.parentId == parentId);

            dynamic nodes = null;

            if (list.Any())
            {
                nodes = list.Select(m => new
                {
                    m.id,
                    m.text,
                    nodes = CreateTree(ac_Module, m.id)
                });
            }
            return nodes;
        }

        /// <summary>
        /// 更新菜单排序
        /// zhoub 20150713
        /// </summary>
        /// <param name="modeleId"></param>
        /// <param name="place"></param>
        /// <returns></returns>
        public ResultModel UpdatePlace(int modeleId, long place)
        {
            var result = new ResultModel()
            {
                Data = this._database.Db.AC_Module.UpdateByModuleID(ModuleID: modeleId, Place: place)
            };
            return result;
        }
    }
}
