using BrCms.Framework.Collections;
using HKTHMall.Domain.AdminModel.Models.Products;
using HKTHMall.Domain.Models;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKTHMall.Services.Products
{
    public class SuppliersService : BaseService, ISuppliersService
    {
        /// <summary>
        /// 添加供应商表（wuyf）
        /// </summary>
        /// <param name="model">供应商表模型</param>
        /// <returns>是否成功</returns>

        public ResultModel AddSuppliers(SuppliersModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.Suppliers.Insert(model)
            };
            return result;
        }

        /// <summary>
        /// 获取供应商列表（wuyf）
        /// </summary>
        /// <param name="parentId">父Id</param>
        /// <returns>供应商列表</returns>
        public ResultModel GetSuppliers(SalesSuppliersModel model)
        {
            var tb = _database.Db.Suppliers;
            var where = new SimpleExpression(1, 1, SimpleExpressionType.Equal); //

            if (model.SupplierId > 0)
            {
                //ID
                where = new SimpleExpression(where, tb.SupplierId == model.SupplierId, SimpleExpressionType.And);
            }

            if (!string.IsNullOrEmpty(model.LinkMan) && model.LinkMan.Trim() != "")
            {
                //联系人
                where = new SimpleExpression(where,
                    tb.LinkMan.Like("%" + model.LinkMan.Trim() + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.SupplierName) && model.SupplierName.Trim() != "")
            {
                //供应商名称
                where = new SimpleExpression(where,
                    tb.SupplierName.Like("%" + model.SupplierName.Trim() + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.Telephone) && model.Telephone.Trim() != "")
            {
                //电话
                where = new SimpleExpression(where,
                    tb.Telephone.Like("%" + model.Telephone.Trim() + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.Mobile) && model.Mobile.Trim() != "")
            {
                //手机
                where = new SimpleExpression(where,
                    tb.Mobile.Like("%" + model.Mobile.Trim() + "%"), SimpleExpressionType.And);
            }
            if (!string.IsNullOrEmpty(model.UserName) && model.UserName.Trim() != "")
            {
                //用户名
                where = new SimpleExpression(where,
                    tb.UserName== model.UserName.Trim(), SimpleExpressionType.And);
            }

            dynamic one,pc;
            dynamic two;

            var query = tb
                .Query()
                .LeftJoin(_database.Db.THArea.As("t1"), out one).On(one.THAreaID == tb.THAreaID)
                .LeftJoin(_database.Db.THArea.As("t2"), out two).On(two.THAreaID == one.ParentID)
                .LeftJoin(_database.Db.THArea_lang.As("t3"), out pc).On(pc.THAreaID == tb.THAreaID && pc.LanguageID == model.Lang)
                //.LeftJoin(_database.Db.THArea, out pc)
                //.On(_database.Db.THArea.THAreaID == tb.THAreaID)
                .Select(
                    tb.SupplierId,
                    tb.SupplierName,
                    tb.THAreaID,
                    tb.Address,
                    tb.LinkMan,
                    tb.Telephone,
                    tb.Mobile,
                    tb.Remark,
                    tb.CreateBy,
                    tb.CreateDT,
                    tb.UpdateBy,
                    tb.UpdateDT,
                    tb.UserName,
                    tb.PassWord,

                    pc.AreaName,
                    one.ParentID.As("ShiTHAreaID"), 
                    two.ParentID.As("ShengTHAreaID")
                )
                .Where(where)
                .OrderByCreateDTDescending();

            var result = new ResultModel
            {
                Data = new SimpleDataPagedList<SuppliersModel>(query,
                    model.PagedIndex, model.PagedSize)
            };

            return result;
        }


        /// <summary>
        /// 更新供应商表（wuyf）
        /// </summary>
        /// <param name="model">供应商表模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateSuppliers(SuppliersModel model)
        {
            var result = new ResultModel()
            {
                Data =
                    base._database.Db.Suppliers.UpdateBySupplierId(SupplierId: model.SupplierId, Telephone: model.Telephone, THAreaID: model.THAreaID, Address: model.Address,
                        LinkMan: model.LinkMan, Mobile: model.Mobile, Remark: model.Remark, SupplierName: model.SupplierName, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT)
            };



            return result;
        }

        /// <summary>
        /// 重置密码（wuyf）
        /// </summary>
        /// <param name="model">供应商表模型</param>
        /// <returns>是否修改成功</returns>
        public ResultModel UpdateSuppliersPassWord(SuppliersModel model)
        {
            var result = new ResultModel()
            {
                Data =
                    base._database.Db.Suppliers.UpdateBySupplierId(SupplierId: model.SupplierId,PassWord: model.PassWord, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT)
            };
            return result;
        }

        /// <summary>
        /// 删除供应商表（wuyf）
        /// </summary>
        /// <param name="model">供应商表模型</param>
        /// <returns>是否删除成功</returns>
        public ResultModel DeleteSuppliers(SuppliersModel model)
        {
            var result = new ResultModel
            {
                Data = _database.Db.Suppliers.Delete(SupplierId: model.SupplierId)
            };

            return result;
        }

        /// <summary>
        /// 根据供应商表id获取供应商表（wuyf）
        /// </summary>
        /// <param name="id">类别id</param>
        /// <returns>供应商表</returns>
        public ResultModel GetSuppliersById(long SupplierId)
        {
            var result = new ResultModel {
                Data = _database.Db.Suppliers.Find(_database.Db.Suppliers.SupplierId == SupplierId)
            };
            return result;
        }

        /// <summary>
        /// 根据手机号获取供应商信息
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns></returns>
        /// <remarks>added by jimmy,2015-9-25</remarks>
        public ResultModel GetSuppliersByPhone(string phone)
        {
            var result = new ResultModel
            {
                Data = _database.Db.Suppliers.FindByMobile(phone)
            };
            return result;
        }

        /// <summary>
        /// 更新供应商密码
        /// </summary>
        /// <param name="model">供应商表模型</param>
        /// <returns>是否修改成功</returns>
        /// <remarks>added by jimmy,2015-9-25</remarks>
        public ResultModel UpdatePwd(SuppliersModel model)
        {
            var result = new ResultModel()
            {
                Data =
                    base._database.Db.Suppliers.UpdateBySupplierId(SupplierId: model.SupplierId, PassWord:model.PassWord, UpdateBy: model.UpdateBy, UpdateDT: model.UpdateDT)
            };
            return result;
        }
    }
}
